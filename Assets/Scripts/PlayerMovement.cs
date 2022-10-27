using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    private Spawner currentSpawner;
    private bool isReady = false;
    private bool wokeUp = false;
    private bool tryJump = false;
    private Vector2 axis;

    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GroundChecker groundChecker;
    [Header("Run")]
    [SerializeField] private float maxRunSpeed = 5;
    [Header("Jump")]
    [SerializeField] private float jumpSpeed = 5;

    public static Action started;

    void Start()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        spriteRenderer.enabled = false;
    }
    void OnEnable()
    {
        ProgressionSystem.won += Won;
        ProgressionSystem.chosePair += ChosePair;
        Orb.gotOrb += GotOrb;
        ResetSystem.onReset += OnReset;
    }
    void OnDisable()
    {
        ProgressionSystem.won -= Won;
        ProgressionSystem.chosePair -= ChosePair;
        Orb.gotOrb -= GotOrb;
        ResetSystem.onReset -= OnReset;
    }
    void ChosePair(Spawner spawner, Orb orb)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        currentSpawner = spawner;

        // This code is also in ProgressionSystem for now. Doesn't rly matter
        transform.position = spawner.transform.position;
        spriteRenderer.enabled = true;
    }
    void GotOrb()
    {
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        spriteRenderer.enabled = false;
        isReady = false;
        wokeUp = false;
    }
    void Update()
    {
        if (!isReady)
            return;

        // Changed axis
        axis.x = Input.GetAxisRaw("Horizontal");
        axis.y = Input.GetAxisRaw("Vertical");
        // Tried jumping
        if (Input.GetKeyDown(KeyCode.C))
            tryJump = true;

        if (!wokeUp && (axis != Vector2.zero || tryJump))
        {
            started?.Invoke();
            wokeUp = true;
        }
    }

    // Ensure it's constant framerate for good movement
    void FixedUpdate()
    {
        if (!isReady)
            return;

        // For now set it constant
        rb.velocity = new Vector2(axis.x * maxRunSpeed, rb.velocity.y);

        // Ensure it's on FixedUpdate
        if (tryJump)
        {
            tryJump = false;
            TryJump();
        }
    }
    void TryJump()
    {
        if (groundChecker.IsGrounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }
    void OnReset(ResetType type, bool post)
    {
        if (type == ResetType.Death || type == ResetType.ManualReset)
            transform.position = currentSpawner.transform.position;

        if (post)
            isReady = true;
        else
        {
            wokeUp = false;
            isReady = false;
            rb.velocity = Vector2.zero;
        }
    }
    void Won()
    {
        // Just so the player is gone from the screen lol
        transform.position = Vector3.one * 9999f;
    }
}
