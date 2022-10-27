using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Orb : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Collider2D col;

    // The bool is true if this is the last orb
    public static Action gotOrb;

    void Start()
    {
        col.enabled = false;
    }
    void OnEnable()
    {
        ProgressionSystem.chosePair += ChosePair;
    }
    void OnDisable()
    {
        ProgressionSystem.chosePair -= ChosePair;
    }
    void OnTriggerEnter2D()
    {
        gotOrb?.Invoke();
        col.enabled = false;
    }
    void ChosePair(Spawner spawner, Orb orb)
    {
        if (orb == this)
            Switcher(true);
    }
    void Switcher(bool on)
    {
        // For now set the collider to on
        col.enabled = true;
    }
}
