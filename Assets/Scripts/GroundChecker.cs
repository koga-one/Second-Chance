using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GroundChecker : MonoBehaviour
{
    // DESCRIPTION ============================================

    // Checks if the player is grounded. Simple

    // VARIABLES ==============================================

    private int amountGrounded = 0;
    private bool isGrounded;
    public bool IsGrounded => isGrounded;

    // PUBLIC VARIABLES =======================================



    // ACTIONS ================================================



    // INSPECTOR VARIABLES ====================================



    // ACTION SUBSCRIPTIONS ===================================



    // ACTION FUNCTIONS =======================================



    // MONOBEHAVIOUR ==========================================

    // To detect if we are grounded. Simple!
    void OnTriggerEnter2D(Collider2D col)
    {
        amountGrounded++;
        isGrounded = amountGrounded > 0;
    }
    void OnTriggerExit2D()
    {
        amountGrounded--;
        isGrounded = amountGrounded > 0;
    }

    // HELPER FUNCTIONS =======================================



    // (✿◡‿◡) ================================================
}
