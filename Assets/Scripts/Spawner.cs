using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SpawnerMode { Waiting, Recording, Recorded }

public class Spawner : MonoBehaviour
{
    // DESCRIPTION ============================================

    // Records the player movements and plays it

    // VARIABLES ==============================================

    private GameObject playerRef;
    private bool started = false;
    private SpawnerMode mode = SpawnerMode.Waiting;
    private List<Vector2> pos = new List<Vector2>();

    // ACTIONS ================================================



    // PUBLIC VARIABLES =======================================

    [Header("References")]
    [SerializeField] private GameObject clone;

    // ACTION SUBSCRIPTIONS ===================================

    private void OnEnable()
    {
        PlayerMovement.started += Started;
    }
    private void OnDisable()
    {
        PlayerMovement.started -= Started;
    }

    // ACTION FUNCTIONS =======================================

    void Started()
    {
        started = true;

        switch (mode)
        {
            case SpawnerMode.Recorded:
                StartCoroutine(PlayBack());
                break;
            case SpawnerMode.Recording:
                StartCoroutine(Record());
                break;
            default:
                break;
        }
    }

    // MONOBEHAVIOUR ==========================================

    private void Start()
    {
        playerRef = FindObjectOfType<PlayerMovement>().gameObject;
    }

    // HELPER FUNCTIONS =======================================

    // Plays back the clone
    IEnumerator PlayBack()
    {
        int frame = 0;
        while (started)
        {
            clone.transform.position = pos[frame];

            frame++;
            if (frame >= pos.Count)
                frame = 0;

            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator Record()
    {
        while (started)
        {
            pos.Add(playerRef.transform.position);
            yield return new WaitForFixedUpdate();
        }
    }

    // (✿◡‿◡) ================================================
}
