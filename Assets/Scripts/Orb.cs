using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Orb : MonoBehaviour
{
    // DESCRIPTION ============================================

    // The orb that the player has to get to progress

    // VARIABLES ==============================================



    // ACTIONS ================================================

    public static Action gotOrb;

    // PUBLIC VARIABLES =======================================

    [Header("References")]
    [SerializeField] private Collider2D col;
    [SerializeField] private HueChanger hueChanger;

    // ACTION SUBSCRIPTIONS ===================================

    void OnEnable()
    {
        ProgressionSystem.chosePair += ChosePair;
    }
    void OnDisable()
    {
        ProgressionSystem.chosePair -= ChosePair;
    }

    // ACTION FUNCTIONS =======================================

    // If this is the pair that was chosen, enable the collider
    // This means for every orb we have this call, but we won't have many pairs so it's fine
    // Maybe send a direct message to the orb later
    void ChosePair(Spawner spawner, Orb orb)
    {
        if (orb == this)
        {
            col.enabled = true;
            hueChanger.enabled = true;
        }
    }

    // MONOBEHAVIOUR ==========================================

    void Start()
    {
        // We disable the collider as a standard procedure
        // Add visual cues to show it is disabled later
        col.enabled = false;
        hueChanger.enabled = false;
    }
    // If trigger enter then this MUST mean the player got the orb
    // We disable the collider again
    void OnTriggerEnter2D()
    {
        gotOrb?.Invoke();
        col.enabled = false;
        hueChanger.enabled = false;
    }

    // HELPER FUNCTIONS =======================================

    public void HighlightThis(bool value)
    {
        hueChanger.enabled = value;
    }

    // (✿◡‿◡) ================================================
}
