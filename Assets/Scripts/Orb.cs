using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Orb : MonoBehaviour
{
    // DESCRIPTION ============================================

    // The orb script. It detects the collision

    // VARIABLES ==============================================

    private bool alreadyUsed = false;
    private int index = 0;

    // PUBLIC VARIABLES =======================================



    // ACTIONS ================================================

    public static Action<Orb> gotOrb;

    // INSPECTOR VARIABLES ====================================

    [Header("References")]
    [SerializeField] private CircleCollider2D circleCollider2D;
    [SerializeField] private HueChanger hueChanger;

    // ACTION SUBSCRIPTIONS ===================================

    private void OnEnable()
    {
        gotOrb += GotOrb;
    }
    private void OnDisable()
    {
        gotOrb -= GotOrb;
    }

    // ACTION FUNCTIONS =======================================

    // This keeps increasing until the orb is captured. Therefore
    // each orb has a different index that represents
    // the capture order
    private void GotOrb(Orb orb)
    {
        if (alreadyUsed)
            return;

        index++;
    }

    // MONOBEHAVIOUR ==========================================

    private void OnTriggerEnter2D(Collider2D other)
    {
        alreadyUsed = true;

        gotOrb?.Invoke(this);
        circleCollider2D.enabled = false;

        hueChanger.enabled = false;
    }

    // HELPER FUNCTIONS =======================================



    // (✿◡‿◡) ================================================
}
