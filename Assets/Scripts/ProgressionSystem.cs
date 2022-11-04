using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProgressionSystem : MonoBehaviour
{
    // DESCRIPTION ============================================

    // Keeps track of the orbs that still need to be caught
    // Checks if the player won

    // VARIABLES ==============================================

    [System.Serializable]
    public struct Pair
    {
        public bool pairDone;
        public Spawner spawner;
        public Orb orb;
    }

    private int currentPair = 0;
    private int amountDone = 0;
    private bool playing = false;

    // ACTIONS ================================================

    public static Action<Spawner, Orb> chosePair;
    public static Action won;

    // PUBLIC VARIABLES =======================================

    [Header("References")]
    [SerializeField] private Pair[] pairs;
    [SerializeField] private GameObject player;

    // ACTION SUBSCRIPTIONS ===================================

    void OnEnable()
    {
        Orb.gotOrb += GotOrb;
    }
    void OnDisable()
    {
        Orb.gotOrb -= GotOrb;
    }

    // ACTION FUNCTIONS =======================================

    void GotOrb()
    {
        pairs[currentPair].pairDone = true;
        amountDone++;

        // Won!
        if (amountDone == pairs.Length)
        {
            won?.Invoke();
        }
        else
        {
            playing = false;
            NextSpawner(true);
        }
    }

    // MONOBEHAVIOUR ==========================================

    void Start()
    {
        player.transform.position = pairs[currentPair].spawner.transform.position;

        HighlightPairs(currentPair, true);
    }
    void Update()
    {
        if (playing || PauseSystem.isPaused)
            return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            playing = true;

            // Do the playing code
            chosePair?.Invoke(pairs[currentPair].spawner, pairs[currentPair].orb);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            NextSpawner(false);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            NextSpawner(true);
    }

    // HELPER FUNCTIONS =======================================

    void NextSpawner(bool positive)
    {
        int previousPair = currentPair;

        if (positive)
        {
            currentPair++;
            if (currentPair > pairs.Length - 1)
                currentPair = 0;
            while (pairs[currentPair].pairDone)
            {
                currentPair++;
                if (currentPair > pairs.Length - 1)
                    currentPair = 0;
            }
        }
        else
        {
            currentPair--;
            if (currentPair < 0)
                currentPair = pairs.Length - 1;
            while (pairs[currentPair].pairDone)
            {
                currentPair--;
                if (currentPair < 0)
                    currentPair = pairs.Length - 1;
            }
        }

        // This means there's only one pair left
        if (previousPair == currentPair)
            return;

        HighlightPairs(previousPair, false);
        HighlightPairs(currentPair, true);

        player.transform.position = pairs[currentPair].spawner.transform.position;
    }
    void HighlightPairs(int index, bool state)
    {
        pairs[index].spawner.HighlightThis(state);
        pairs[index].orb.HighlightThis(state);
    }

    // (✿◡‿◡) ================================================
}
