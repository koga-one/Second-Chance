using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SpawnerStates { Off, Recording, Replaying }

public class Spawner : MonoBehaviour
{
    /*
        Gets
            ID (int)
            nextSpawnerID (int, -1 if doesn't have next)

        Spawns the player
        Records their frames every FixedUpdate (if started)
            Deletes recording (if dies)
                Sends back to start
            Stops recording (if gets orb)
                Sends message to next spawner ID (if has next spawner ID)
                Sends message to win system (if doesn't have next spawner ID)
        Replays the movement (if started (on next spawner))
            Spawns clone (if isn't spawned)
    */

    public static Action<int> nextSpawner;
    public static Action wasLastSpawner;



    private int ID;
    private int nextID;
    private bool started;
    private bool recorded;
    private Transform player;
    private List<Vector2> frames = new List<Vector2>();
    private GameObject clone;
    private int index;



    public void SetupSpawner(int ID_, int nextID_, Transform player_)
    {
        ID = ID_;
        nextID = nextID_;
        player = player_;
    }
    void OnEnable()
    {
        PlayerMovement.started += Started;
        nextSpawner += CheckIfNextSpawner;
        DeathSystem.onDeath += DeleteRecording;
        OrbSystem.gotOrb += TryNextSpawner;
    }
    void OnDisable()
    {
        PlayerMovement.started -= Started;
        nextSpawner -= CheckIfNextSpawner;
        DeathSystem.onDeath -= DeleteRecording;
        OrbSystem.gotOrb -= TryNextSpawner;
    }
    void FixedUpdate()
    {
        if (started)
            Record();
    }



    void Started() => started = true;
    void CheckIfNextSpawner(int targetID) { if (ID == targetID) Spawn(); }
    void Spawn() { player.position = transform.position; }
    void Record() { frames.Add(player.position); }
    void DeleteRecording()
    {
        started = false;
        player.position = transform.position;
        frames.Clear();
    }
    void TryNextSpawner()
    {
        started = false;
        if (nextID != -1)
            nextSpawner?.Invoke(nextID);
        else
            wasLastSpawner?.Invoke();
    }
    void StartReplay()
    {
        clone.SetActive(true);
    }
    void Replay() => clone.transform.position = frames[index];
}
