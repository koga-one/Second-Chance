using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProgressionSystem : MonoBehaviour
{
    [System.Serializable]
    public struct Pair
    {
        public bool pairDone;
        public Spawner spawner;
        public Orb orb;
    }

    [Header("References")]
    [SerializeField] private Pair[] pairs;
    [SerializeField] private GameObject player;

    public static Action<Spawner, Orb> chosePair;
    public static Action won;
    private int currentPair = 0;
    private int amountDone = 0;
    private bool playing = false;

    void OnEnable()
    {
        Orb.gotOrb += GotOrb;
    }
    void OnDisable()
    {
        Orb.gotOrb -= GotOrb;
    }
    void Start()
    {
        player.transform.position = pairs[currentPair].spawner.transform.position;
    }
    void Update()
    {
        if (playing)
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

    void GotOrb()
    {
        pairs[currentPair].pairDone = true;
        amountDone++;

        // Won!
        if (amountDone == pairs.Length)
        {
            won?.Invoke();
            Debug.Log("won!");
        }
        else
        {
            playing = false;
            NextSpawner(true);
        }
    }

    void NextSpawner(bool positive)
    {
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

        player.transform.position = pairs[currentPair].spawner.transform.position;
    }
}
