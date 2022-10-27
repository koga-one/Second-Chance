using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    // DESCRIPTION ============================================

    // The movement script
    // Improve the movement later!

    // VARIABLES ==============================================

    private Spawner currentSpawner;
    private bool isReady = false;
    private bool wokeUp = false;
    private bool tryJump = false;
    private Vector2 axis;

    // ACTIONS ================================================

    public static Action started;

    // PUBLIC VARIABLES =======================================

    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GroundChecker groundChecker;
    [Header("Run")]
    [SerializeField] private float maxRunSpeed = 5;
    [Header("Jump")]
    [SerializeField] private float jumpSpeed = 5;

    // ACTION SUBSCRIPTIONS ===================================

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

    // ACTION FUNCTIONS =======================================

    void Won()
    {
        // Just so the player is gone from the screen lol
        transform.position = Vector3.one * 9999f;
    }
    // Nothing to explain here
    void ChosePair(Spawner spawner, Orb orb)
    {
        currentSpawner = spawner;

        // This code is also in ProgressionSystem for now. Doesn't rly matter
        transform.position = spawner.transform.position;
        spriteRenderer.enabled = true;
    }
    // Got orb so we change a bunch of stuff
    // Nothing fancy
    void GotOrb()
    {
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        spriteRenderer.enabled = false;
        isReady = false;
        wokeUp = false;
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

    // MONOBEHAVIOUR ==========================================

    // Set the player to kinematic bc he shouldn't move yet
    // Set a better visual cue to show he's disabled laters
    void Start()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        spriteRenderer.enabled = false;
    }
    void Update()
    {
        // Only get inputs if we are ready
        if (!isReady)
            return;

        // Changed axis
        axis.x = Input.GetAxisRaw("Horizontal");
        axis.y = Input.GetAxisRaw("Vertical");
        // Tried jumping
        if (Input.GetKeyDown(KeyCode.C))
            tryJump = true;

        // Player woke up so we set their bodyType to dynamic
        if (!wokeUp && (axis != Vector2.zero || tryJump))
        {
            started?.Invoke();
            wokeUp = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
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

    // HELPER FUNCTIONS =======================================

    void TryJump()
    {
        if (groundChecker.IsGrounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }

    // (✿◡‿◡) ================================================
}
