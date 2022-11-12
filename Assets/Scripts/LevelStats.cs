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
        DeathChecker.died += Died;
        LevelProgress.reset += Died;
    }
    private void OnDisable()
    {
        PlayerMovement.started -= Started;
        DeathChecker.died -= Died;
        LevelProgress.reset -= Died;
    }

    // ACTION FUNCTIONS =======================================

    void Started()
    {

    }
    void Died()
    {

    }

    // MONOBEHAVIOUR ==========================================



    // HELPER FUNCTIONS =======================================



    // (✿◡‿◡) ================================================
}
