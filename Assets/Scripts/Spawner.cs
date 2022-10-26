using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
    Waiting => still hasn't been used
    Recording => is the current spawner and is recording the player moves
    Playing => has been used and now replays the movements

    Waiting -> Recording -> Playing
*/
public enum SpawnerStates { Waiting, Recording, Replaying }

public class Spawner : MonoBehaviour
{
    private SpawnerStates currentState = SpawnerStates.Waiting;
    private int frame = 0;
    private GameObject clone;
    private GameObject player;
    private List<Vector2> positions = new List<Vector2>();
    private bool canStart = false;

    [Header("References")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private int id;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;

        if (id == 0)
        {
            currentState = SpawnerStates.Recording;
            player.transform.position = transform.position;
        }
    }
    void OnEnable()
    {
        PlayerMovement.started += PlayerStarted;
        DeathSystem.onReset += Reset;
        Orb.gotOrb += GotOrb;
    }
    void OnDisable()
    {
        PlayerMovement.started -= PlayerStarted;
        DeathSystem.onReset -= Reset;
        Orb.gotOrb -= GotOrb;
    }
    void PlayerStarted()
    {
        canStart = true;

        if (currentState == SpawnerStates.Replaying)
        {
            clone.SetActive(true);
            clone.transform.position = transform.position;
        }
    }
    void Reset(bool died, bool post)
    {
        if (post)
            return;

        canStart = false;

        if (currentState == SpawnerStates.Recording && died)
        {
            positions.Clear();
            player.transform.position = transform.position;
        }
        else if (currentState == SpawnerStates.Replaying)
        {
            frame = 0;
            clone.SetActive(false);
        }
    }
    void GotOrb(int target)
    {
        if (id == target)
        {
            currentState = SpawnerStates.Recording;
            player.transform.position = transform.position;
        }
        // Disables this spawner bc we got the orb and also spawns the clone
        else if (currentState == SpawnerStates.Recording)
        {
            currentState = SpawnerStates.Replaying;

            clone = Instantiate(clonePrefab);
            clone.SetActive(false);
        }
    }
    void FixedUpdate()
    {
        if (!canStart)
            return;

        // If isCurrent this means we still need to record the player
        if (currentState == SpawnerStates.Recording)
            positions.Add(player.transform.localPosition);
        // Otherwise, play it
        else if (currentState == SpawnerStates.Replaying)
        {
            frame++;
            if (frame >= positions.Count)
                frame = 0;

            clone.transform.localPosition = positions[frame];
        }

    }
}