using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    // DESCRIPTION ============================================

    // The movement script
    // It listens to inputs
    // It also stores data from the environment
    // It runs the inputs through the active modules
    // The modules might also use the data from the environment to make decisions
    // They all input Vector2s that will directly affect the velocity of the player

    // VARIABLES ==============================================

    private int runFrames = 0;
    private bool isTryingJump = false;
    private int jumpFrames = 0;
    private int fallFrames = 0;
    private int coyoteFrames = 0;
    private int echoFrames = 0;
    private Vector2 axis;

    // ACTIONS ================================================



    // PUBLIC VARIABLES =======================================

    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CircleCollider2D circleCollider2D;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GroundChecker groundChecker;

    [Header("Run")]
    [SerializeField] private float maxRunSpeed = 5;
    [SerializeField] private int framesToMaxSpeed = 3;
    [SerializeField] private AnimationCurve runCurve;

    [Header("Jump")]
    [SerializeField] private float maxJumpSpeed = 5;
    [SerializeField] private int framesToMaxJump = 10;
    [SerializeField] private int framesToMinJump = 2;
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private int maxCoyote = 5;
    [SerializeField] private int maxEcho = 5;
    [Header("Fall")]
    [SerializeField] private float maxFallSpeed = 20;
    [SerializeField] private int framesToMaxFall = 8;
    [SerializeField] private AnimationCurve fallCurve;


    // ACTION SUBSCRIPTIONS ===================================

    void OnEnable()
    {

    }
    void OnDisable()
    {

    }

    // ACTION FUNCTIONS =======================================



    // MONOBEHAVIOUR ==========================================

    void Update()
    {
        // Changed axis
        axis.x = Input.GetAxisRaw("Horizontal");
        axis.y = Input.GetAxisRaw("Vertical");

        // Set the coyote frames
        if (groundChecker.IsGrounded)
            coyoteFrames = maxCoyote;

        // Tried jumping
        if (Input.GetKeyDown(KeyCode.C))
            echoFrames = maxEcho;
        if (echoFrames > 0 && coyoteFrames > 0)
        {
            isTryingJump = true;
            coyoteFrames = 0;
            echoFrames = 0;
        }
        else if (!Input.GetKey(KeyCode.C) && jumpFrames >= framesToMinJump)
            isTryingJump = false;
    }
    // Ensure it's constant framerate for good movement
    void FixedUpdate()
    {
        // Decrease the coyote frames
        if (coyoteFrames > 0)
            coyoteFrames--;

        // Decrease the echo frames
        if (echoFrames > 0)
            echoFrames--;

        // Reset the jump frames immediately
        if (!isTryingJump)
            jumpFrames = 0;
        if (groundChecker.IsGrounded)
            fallFrames = 0;

        // Do the movements normally
        rb.velocity = new Vector2(MoveFloat(), JumpFloat() + FallFloat());
    }

    // HELPER FUNCTIONS =======================================
    float MoveFloat()
    {
        // Decrease
        if (axis.x == 0 && runFrames != 0)
            runFrames -= runFrames / Mathf.Abs(runFrames);
        // Run frames isn't 0 and they're different directions
        else if (axis.x != 0 && runFrames * axis.x < 0)
            runFrames = 0;
        // Maybe run frames is 0 or they're same diretion => logic is the same
        else if (axis.x != 0 && Mathf.Abs(runFrames) < framesToMaxSpeed)
            runFrames += (int)(axis.x / Mathf.Abs(axis.x));

        return runCurve.Evaluate(Mathf.Abs(runFrames) / (float)framesToMaxSpeed) * maxRunSpeed * (runFrames < 0 ? -1 : 1);
    }
    // Vector2 KickVector()
    // {
    //     Vector2 result = new Vector2();

    //     // Going up and down
    //     result.y = axis.y * maxClimbSpeed;

    //     return result;
    // }
    float JumpFloat()
    {
        // Isn't jumping
        if (!isTryingJump || jumpFrames > framesToMaxJump)
            return 0;

        jumpFrames++;

        return jumpCurve.Evaluate(jumpFrames / framesToMaxJump) * maxJumpSpeed;
    }
    float FallFloat()
    {
        // Is jumping
        if (isTryingJump && jumpFrames <= framesToMaxJump)
            return 0;

        if (fallFrames < framesToMaxFall)
            fallFrames++;

        return fallCurve.Evaluate(fallFrames / (float)framesToMaxFall) * maxFallSpeed * -1;
    }

    // (✿◡‿◡) ================================================
}
