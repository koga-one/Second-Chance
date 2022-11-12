using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeathChecker : MonoBehaviour
{
    // DESCRIPTION ============================================

    // The player death checker. Mostly OnCollision stuff

    // VARIABLES ==============================================



    // ACTIONS ================================================

    public static Action died;

    // PUBLIC VARIABLES =======================================

    [Header("Settings")]
    [SerializeReference] private LayerMask badLayers;

    // ACTION SUBSCRIPTIONS ===================================



    // ACTION FUNCTIONS =======================================



    // MONOBEHAVIOUR ==========================================

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (badLayers == (badLayers | 1 << other.gameObject.layer))
            died?.Invoke();
    }

    // HELPER FUNCTIONS =======================================



    // (✿◡‿◡) ================================================
}
