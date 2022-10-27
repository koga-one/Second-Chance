using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BodyCollider : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask badLayers;

    public static Action death;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (badLayers == (badLayers | 1 << collision.gameObject.layer))
            death?.Invoke();
    }
}
