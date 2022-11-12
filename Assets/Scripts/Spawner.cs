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

    // When was this spawner played?
    private int index = 0;
    private int frame = 0;
    private Coroutine playback;
    private Coroutine record;
    private GameObject playerRef;
    private bool won = false;
    private bool started = false;
    private List<Vector2> pos = new List<Vector2>();
    private SpawnerMode mode = SpawnerMode.Waiting;

    // PUBLIC VARIABLES =======================================

    public SpawnerMode Mode => mode;

    // ACTIONS ================================================

    public static Action<int> finishedReplay;

    // INSPECTOR VARIABLES ====================================

    [Header("References")]
    [SerializeField] private GameObject clone;

    // ACTION SUBSCRIPTIONS ===================================

    private void OnEnable()
    {
        LevelProgress.spawned += Spawned;
        PlayerMovement.started += Started;
        DeathChecker.died += Died;
        LevelProgress.reset += Died;
        LevelProgress.nextOrb += Died;
        LevelProgress.won += Won;
        LevelProgress.increaseReplay += IncreaseReplay;
    }
    private void OnDisable()
    {
        LevelProgress.spawned -= Spawned;
        PlayerMovement.started -= Started;
        DeathChecker.died -= Died;
        LevelProgress.reset -= Died;
        LevelProgress.nextOrb -= Died;
        LevelProgress.won -= Won;
        LevelProgress.increaseReplay -= IncreaseReplay;
    }

    // ACTION FUNCTIONS =======================================

    void Spawned(Spawner spawner, int newIndex)
    {
        if (spawner != this)
            return;

        mode = SpawnerMode.Recording;
        index = newIndex;
    }
    void Started()
    {
        started = true;

        switch (mode)
        {
            case SpawnerMode.Recorded:
                playback = StartCoroutine(PlayBack());
                break;
            case SpawnerMode.Recording:
                record = StartCoroutine(Record());
                break;
            default:
                break;
        }
    }
    void Died()
    {
        started = false;

        OnModeStop();
    }
    // Shows the recordings one by one just like the player did it
    void Won()
    {
        won = true;

        OnModeStop();

        if (index == 0)
            playback = StartCoroutine(PlayBack());
    }
    void IncreaseReplay(int otherIndex)
    {
        if (otherIndex == 0)
        {
            StopCoroutine(playback);
            frame = 0;

            clone.SetActive(false);
            clone.transform.position = transform.position;
        }

        if (index < otherIndex)
            frame = 0;
        else if (index == otherIndex)
            playback = StartCoroutine(PlayBack());
    }

    // MONOBEHAVIOUR ==========================================

    private void Start()
    {
        playerRef = FindObjectOfType<PlayerMovement>().gameObject;
    }

    // HELPER FUNCTIONS =======================================

    // Executed when we should stop a mode
    // (and possibly switch to another)
    void OnModeStop()
    {
        switch (mode)
        {
            case SpawnerMode.Recorded:
                StopCoroutine(playback);
                frame = 0;

                clone.SetActive(false);
                clone.transform.position = transform.position;
                break;
            case SpawnerMode.Recording:
                mode = SpawnerMode.Recorded;

                StopCoroutine(record);
                break;
            default:
                break;
        }
    }
    // Plays back the clone
    IEnumerator PlayBack()
    {
        clone.SetActive(true);

        while (started || won)
        {
            clone.transform.position = pos[frame];

            frame++;
            if (frame >= pos.Count)
            {
                if (won)
                    finishedReplay?.Invoke(index);

                frame = 0;
            }

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
