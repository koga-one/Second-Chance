using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BodyCollider : MonoBehaviour
{
    // DESCRIPTION ============================================

    // Checks if the player hit anything bad for now

    // VARIABLES ==============================================



    // ACTIONS ================================================

    public static Action death;

    // PUBLIC VARIABLES =======================================

    [Header("Settings")]
    [SerializeField] private LayerMask badLayers;

    // ACTION SUBSCRIPTIONS ===================================



    // ACTION FUNCTIONS =======================================



    // MONOBEHAVIOUR ==========================================

    // Checks if we collided with something bad like spikes or clones
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (badLayers == (badLayers | 1 << collision.gameObject.layer))
            death?.Invoke();
    }

    // HELPER FUNCTIONS =======================================



    // (✿◡‿◡) ================================================
}
