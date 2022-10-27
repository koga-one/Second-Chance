using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    private bool isDead = false;
    private bool hasStarted = false;
    private bool tryJump = false;
    private Vector2 axis;
    public Vector2 Axis => axis;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GroundChecker groundChecker;
    [Header("Run")]
    [SerializeField] private float maxRunSpeed = 5;
    [Header("Jump")]
    [SerializeField] private float jumpSpeed = 5;

    public static Action started;

    void OnEnable()
    {
        DeathSystem.onReset += OnReset;
    }
    void OnDisable()
    {
        DeathSystem.onReset -= OnReset;
    }
    void Update()
    {
        if (isDead)
            return;

        // Changed axis
        axis.x = Input.GetAxisRaw("Horizontal");
        axis.y = Input.GetAxisRaw("Vertical");
        // Tried jumping
        if (Input.GetKeyDown(KeyCode.C))
            tryJump = true;

        if (!hasStarted && (axis != Vector2.zero || tryJump))
        {
            started?.Invoke();
            hasStarted = true;
        }
    }

    // Ensure it's constant framerate for good movement
    void FixedUpdate()
    {
        if (isDead)
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
    void OnReset(bool died, bool post)
    {
        if (post)
            isDead = false;
        else
        {
            hasStarted = false;
            isDead = true;
            rb.velocity = Vector2.zero;
        }
    }
    void GotOrb(int id)
    {
        hasStarted = false;
    }
}
