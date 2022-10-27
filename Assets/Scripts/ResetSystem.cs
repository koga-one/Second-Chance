using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public enum ResetType { Death, ManualReset, ChosePair }

public class ResetSystem : MonoBehaviour
{
    // DESCRIPTION ============================================

    // It's a central hub for calling resets

    // VARIABLES ==============================================

    private bool hasPair = false;

    // ACTIONS ================================================

    // bool is if the delay just started or if it'll end
    public static Action<ResetType, bool> onReset;

    // PUBLIC VARIABLES =======================================

    [Header("Settings")]
    [SerializeField] private int spawnerDelayFrames;

    // ACTION SUBSCRIPTIONS ===================================

    private void OnEnable()
    {
        ProgressionSystem.chosePair += ChosePair;
        Orb.gotOrb += GotOrb;
        BodyCollider.death += CallDeath;
    }
    private void OnDisable()
    {
        ProgressionSystem.chosePair -= ChosePair;
        Orb.gotOrb -= GotOrb;
        BodyCollider.death -= CallDeath;
    }

    // ACTION FUNCTIONS =======================================

    void ChosePair(Spawner spawner, Orb orb)
    {
        hasPair = true;
        StartCoroutine(Reset(ResetType.ChosePair));
    }
    void GotOrb()
    {
        hasPair = false;
    }
    void CallDeath() => StartCoroutine(Reset(ResetType.Death));
    private IEnumerator Reset(ResetType type)
    {
        onReset?.Invoke(type, false);

        yield return new WaitForSeconds(spawnerDelayFrames * Time.fixedDeltaTime);

        onReset?.Invoke(type, true);
    }

    // MONOBEHAVIOUR ==========================================

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (!hasPair)
            return;

        else if (Input.GetKeyDown(KeyCode.V))
            StartCoroutine(Reset(ResetType.ManualReset));
    }

    // HELPER FUNCTIONS =======================================



    // (✿◡‿◡) ================================================
}
