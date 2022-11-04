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
    // DESCRIPTION ============================================

    // The spawner for the clone. It stores frames
    // and replays them when necessary

    // VARIABLES ==============================================

    private SpawnerStates currentState = SpawnerStates.Waiting;
    private int frame = 0;
    private GameObject player;
    private List<Vector2> positions = new List<Vector2>();
    private bool wokeUp = false;

    // ACTIONS ================================================



    // PUBLIC VARIABLES =======================================

    [Header("References")]
    [SerializeField] private GameObject clone;
    [SerializeField] private HueChanger hueChanger;

    // ACTION SUBSCRIPTIONS ===================================

    void OnEnable()
    {
        ProgressionSystem.chosePair += ChosePair;
        PlayerMovement.started += PlayerStarted;
        ResetSystem.onReset += Reset;
        Orb.gotOrb += GotOrb;
    }
    void OnDisable()
    {
        ProgressionSystem.chosePair -= ChosePair;
        PlayerMovement.started -= PlayerStarted;
        ResetSystem.onReset -= Reset;
        Orb.gotOrb -= GotOrb;
    }

    // ACTION FUNCTIONS =======================================

    void ChosePair(Spawner spawner, Orb orb)
    {
        if (spawner == this)
            currentState = SpawnerStates.Recording;
    }
    void PlayerStarted()
    {
        wokeUp = true;

        if (currentState == SpawnerStates.Replaying)
        {
            frame = 0;
            clone.transform.position = transform.position;
        }
    }
    void Reset(ResetType type, bool post)
    {
        if (post)
            return;

        wokeUp = false;

        if (currentState == SpawnerStates.Recording && (type == ResetType.Death || type == ResetType.ManualReset))
            positions.Clear();
        else if (currentState == SpawnerStates.Replaying)
            clone.transform.position = transform.position;
    }
    void GotOrb()
    {
        // Sets replaying mode
        if (currentState == SpawnerStates.Recording)
        {
            currentState = SpawnerStates.Replaying;
            clone.SetActive(true);
        }
    }

    // MONOBEHAVIOUR ==========================================

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
        clone.SetActive(false);
    }
    void FixedUpdate()
    {
        if (!wokeUp)
            return;

        // If isCurrent this means we still need to record the player
        if (currentState == SpawnerStates.Recording)
            positions.Add(player.transform.position);
        // Otherwise, play it
        else if (currentState == SpawnerStates.Replaying)
        {
            clone.transform.position = positions[frame];

            frame++;
            if (frame >= positions.Count)
                frame = 0;
        }

    }

    // HELPER FUNCTIONS =======================================

    public void HighlightThis(bool value)
    {
        hueChanger.enabled = value;
    }

    // (✿◡‿◡) ================================================
}