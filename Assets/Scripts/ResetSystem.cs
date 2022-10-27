using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public enum ResetType { Death, ManualReset, ChosePair }

public class ResetSystem : MonoBehaviour
{
    // bool is if the delay just started or if it'll end
    public static Action<ResetType, bool> onReset;

    [Header("Settings")]
    [SerializeField] private int spawnerDelayFrames;

    private bool hasPair = false;

    private void OnEnable()
    {
        BodyCollider.death += CallDeath;
        ProgressionSystem.chosePair += ChosePair;
        Orb.gotOrb += GotOrb;
    }
    private void OnDisable()
    {
        BodyCollider.death -= CallDeath;
        ProgressionSystem.chosePair -= ChosePair;
        Orb.gotOrb -= GotOrb;
    }
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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (!hasPair)
            return;

        else if (Input.GetKeyDown(KeyCode.V))
            StartCoroutine(Reset(ResetType.ManualReset));
    }
}
