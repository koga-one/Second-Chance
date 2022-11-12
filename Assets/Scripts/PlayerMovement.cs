using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum MovementType { Normal, Sticky }

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

    private bool hasStarted = false;
    private MovementType currentMovement = MovementType.Normal;
    private bool isJumping = false;
    // The current stage of the run
    private int runFrames = 0;
    // The current stage of the jump
    private int jumpFrames = 0;
    // The current stage of the fall
    private int fallFrames = 0;
    // Allows players to jump even after leaving the ground
    private int coyoteFrames = 0;
    // Allows players to jump even if they press space too early
    private int echoFrames = 0;
    // The arrow keys current value
    private Vector2 axis;

    // ACTIONS ================================================

    public static Action started;

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
            isJumping = true;
            coyoteFrames = 0;
            echoFrames = 0;
        }
        // Deactivates the jump only when the player stops holding and they got the min jump height
        else if (!Input.GetKey(KeyCode.C) && jumpFrames >= framesToMinJump)
            isJumping = false;

        if (!hasStarted && (axis != Vector2.zero || Input.GetKeyDown(KeyCode.C)))
        {
            hasStarted = true;
            started?.Invoke();
        }
    }
    // Ensure it's constant framerate for good movement
    void FixedUpdate()
    {
        FramesUpdate();

        // Do the movements normally
        switch (currentMovement)
        {
            case MovementType.Normal:
            default:
                rb.velocity = new Vector2(MoveFloat(), JumpFloat() + FallFloat());
                break;
        }
    }

    // HELPER FUNCTIONS =======================================

    void FramesUpdate()
    {
        // Decrease the coyote frames
        if (coyoteFrames > 0)
            coyoteFrames--;

        // Decrease the echo frames
        if (echoFrames > 0)
            echoFrames--;

        // Reset the jump frames immediately
        if (!isJumping)
            jumpFrames = 0;

        // Rset the fall frames immediately
        if (groundChecker.IsGrounded)
            fallFrames = 0;
    }
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
    float JumpFloat()
    {
        // Isn't jumping
        if (!isJumping || jumpFrames > framesToMaxJump)
            return 0;

        jumpFrames++;

        return jumpCurve.Evaluate(jumpFrames / framesToMaxJump) * maxJumpSpeed;
    }
    float FallFloat()
    {
        // Is jumping
        if (isJumping && jumpFrames <= framesToMaxJump)
            return 0;

        if (fallFrames < framesToMaxFall)
            fallFrames++;

        return fallCurve.Evaluate(fallFrames / (float)framesToMaxFall) * maxFallSpeed * -1;
    }

    // (✿◡‿◡) ================================================
}
