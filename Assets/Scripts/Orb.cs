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

    private void GotOrb(Orb orb)
    {

    }

    // MONOBEHAVIOUR ==========================================

    private void OnTriggerEnter2D(Collider2D other)
    {
        gotOrb?.Invoke(this);
        circleCollider2D.enabled = false;
    }

    // HELPER FUNCTIONS =======================================



    // (✿◡‿◡) ================================================
}
