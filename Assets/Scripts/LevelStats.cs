using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelStats : MonoBehaviour
{
    // DESCRIPTION ============================================

    // Level stats such as time, resets, is paused, etc.

    // VARIABLES ==============================================

    private bool started = false;
    private float timer;

    // PUBLIC VARIABLES =======================================



    // ACTIONS ================================================



    // INSPECTOR VARIABLES ====================================



    // ACTION SUBSCRIPTIONS ===================================

    private void OnEnable()
    {
        PlayerMovement.started += Started;
        DeathChecker.died += Died;
        LevelProgress.reset += Died;
        LevelProgress.nextOrb += Died;
    }
    private void OnDisable()
    {
        PlayerMovement.started -= Started;
        DeathChecker.died -= Died;
        LevelProgress.reset -= Died;
        LevelProgress.nextOrb -= Died;
    }

    // ACTION FUNCTIONS =======================================

    void Started()
    {
        started = true;
    }
    void Died()
    {
        started = false;
    }

    // MONOBEHAVIOUR ==========================================

    void FixedUpdate()
    {
        if (!started)
            return;

        timer += Time.fixedDeltaTime;

    }

    // HELPER FUNCTIONS =======================================



    // (✿◡‿◡) ================================================
}
