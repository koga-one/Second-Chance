using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Orb : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D collider;
    [Header("Settings")]
    [SerializeField] private int id;
    [SerializeField] private int targetID;

    // The bool is true if this is the last orb
    public static Action won;
    public static Action<int> gotOrb;

    void Start()
    {
        if (id == 0)
            spriteRenderer.color = Color.red;
    }
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

        if (targetID != -1)
            gotOrb?.Invoke(targetID);
        else
            won?.Invoke();

    }
    void GotOrb(int target)
    {
        // For now just sets color
        spriteRenderer.color = id == target ? Color.red : Color.white;
    }
    void DisableOrb()
    {
        // For now just disables collider
        collider.enabled = false;
    }
}
