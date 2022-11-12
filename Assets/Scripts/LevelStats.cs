using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStats : MonoBehaviour
{
    // DESCRIPTION ============================================

    // Level stats such as time, resets, is paused, etc.

    // VARIABLES ==============================================



    // ACTIONS ================================================



    // PUBLIC VARIABLES =======================================



    // ACTION SUBSCRIPTIONS ===================================

    private void OnEnable()
    {
        PlayerMovement.started += Started;
    }
    private void OnDisable()
    {
        PlayerMovement.started -= Started;
    }

    // ACTION FUNCTIONS =======================================

    void Started()
    {

    }

    // MONOBEHAVIOUR ==========================================



    // HELPER FUNCTIONS =======================================



    // (✿◡‿◡) ================================================
}
