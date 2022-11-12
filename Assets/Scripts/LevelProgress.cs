using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class LevelProgress : MonoBehaviour
{
    // DESCRIPTION ============================================

    // Gets all of the spawners, coordinates them,
    // Checks the win condition (so grabbing orbs)

    // VARIABLES ==============================================

    private int currentReplay = 0;
    private bool hasWon = false;
    private int finished = 0;
    private bool playing = false;
    private int index;
    private Spawner[] allSpawners;

    // ACTIONS ================================================

    public static Action reset;
    public static Action<Spawner, int> spawned;
    public static Action nextOrb;
    public static Action won;
    public static Action<int> increaseReplay;

    // PUBLIC VARIABLES =======================================

    [Header("References")]
    [SerializeField] private Transform spawnersParent;
    [SerializeField] private GameObject player;

    // ACTION SUBSCRIPTIONS ===================================

    private void OnEnable()
    {
        DeathChecker.died += Died;
        Orb.gotOrb += GotOrb;
        Spawner.finishedReplay += FinishedReplay;
    }
    private void OnDisable()
    {
        DeathChecker.died -= Died;
        Orb.gotOrb -= GotOrb;
        Spawner.finishedReplay -= FinishedReplay;
    }

    // ACTION FUNCTIONS =======================================

    private void Died()
    {
        playing = false;

        SetPlayerPos();
    }
    private void GotOrb(Orb orb)
    {
        Died();

        finished++;

        if (finished == allSpawners.Length)
        {
            Debug.LogWarning("WON");
            hasWon = true;

            won?.Invoke();
        }
        else
        {
            nextOrb?.Invoke();
            ChangeIndex(true);
        }
    }
    private void FinishedReplay(int index)
    {
        // The latest replay finished => do it all again,
        // with one more
        if (index != currentReplay)
            return;

        currentReplay++;

        if (currentReplay == allSpawners.Length)
            currentReplay = 0;

        increaseReplay?.Invoke(currentReplay);
    }

    // MONOBEHAVIOUR ==========================================

    private void Start()
    {
        allSpawners = spawnersParent.GetComponentsInChildren<Spawner>(true);

        SetPlayerPos();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (hasWon)
            return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            reset?.Invoke();

            // Uses the same code as when they die
            Died();
        }

        if (playing)
            return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            ChangeIndex(false);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            ChangeIndex(true);

        if (Input.GetKeyDown(KeyCode.C))
        {
            playing = true;
            Invoke("InvokeSpawner", Time.fixedDeltaTime);
        }
    }

    // HELPER FUNCTIONS =======================================

    // Need to use this to ignore the first frame that it
    // tries to jump
    private void InvokeSpawner()
    {
        spawned?.Invoke(allSpawners[index], finished);
    }
    // Note: can only change index when the player has finished
    // the current orb
    private void ChangeIndex(bool right)
    {
        ChangeIndexHelper(right);

        while (allSpawners[index].Mode != SpawnerMode.Waiting)
            ChangeIndexHelper(right);

        SetPlayerPos();
    }
    private void ChangeIndexHelper(bool right)
    {
        index += right ? 1 : -1;

        if (index >= allSpawners.Length)
            index = 0;
        else if (index < 0)
            index = allSpawners.Length - 1;
    }
    private void SetPlayerPos()
    {
        player.transform.position = allSpawners[index].transform.position;
    }

    // (✿◡‿◡) ================================================
}
