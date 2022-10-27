using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stats : MonoBehaviour
{
    // DESCRIPTION ============================================

    // Keeps track of the player stats for display

    // VARIABLES ==============================================

    private bool started = false;
    private int resetCount = 0;
    private float timer;

    // ACTIONS ================================================



    // PUBLIC VARIABLES =======================================

    [Header("References")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI deathText;

    // ACTION SUBSCRIPTIONS ===================================

    void OnEnable()
    {
        ResetSystem.onReset += OnReset;
        PlayerMovement.started += Started;
        Orb.gotOrb += GotOrb;
    }
    void OnDisable()
    {
        ResetSystem.onReset -= OnReset;
        PlayerMovement.started -= Started;
        Orb.gotOrb -= GotOrb;
    }

    // ACTION FUNCTIONS =======================================

    void OnReset(ResetType type, bool post)
    {
        if (post)
            return;

        started = false;

        if (type == ResetType.Death || type == ResetType.ManualReset)
            resetCount++;
        deathText.text = "Resets: " + resetCount.ToString();
    }
    void Started() => started = true;
    void GotOrb() => started = false;

    // MONOBEHAVIOUR ==========================================

    void FixedUpdate()
    {
        if (started)
        {
            timer += Time.fixedDeltaTime;
            timerText.text = "Time: " + timer.ToString("00.00");
        }
    }

    // HELPER FUNCTIONS =======================================



    // (✿◡‿◡) ================================================
}
