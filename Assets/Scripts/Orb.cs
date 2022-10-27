using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Orb : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D col;

    // The bool is true if this is the last orb
    public static Action gotOrb;

    void OnEnable()
    {
        gotOrb += GotOrb;
    }
    void OnDisable()
    {
        gotOrb -= GotOrb;
    }
    void OnTriggerEnter2D()
    {
        // Got orb! This means next pair or winning
        DisableOrb();

        gotOrb?.Invoke();
    }
    void GotOrb()
    {
    }
    void DisableOrb()
    {
        // For now just disables collider
        col.enabled = false;
    }
}
