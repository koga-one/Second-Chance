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

    public static Action<Pair> chosePair;
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
    void Update()
    {
        if (playing)
            return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            playing = true;

            // Do the playing code
            chosePair?.Invoke(pairs[currentPair]);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Keep searching for a pair that isn't done
            while (pairs[currentPair].pairDone)
            {
                currentPair--;
                if (currentPair < 0)
                    currentPair = pairs.Length - 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            while (pairs[currentPair].pairDone)
            {
                currentPair++;
                if (currentPair > pairs.Length - 1)
                    currentPair = 0;
            }
        }
    }

    void GotOrb()
    {
        pairs[currentPair].pairDone = true;
        amountDone++;

        // Won!
        if (amountDone == pairs.Length)
            won?.Invoke();
        else
            playing = false;
    }
}
