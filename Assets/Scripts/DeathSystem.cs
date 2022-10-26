using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class DeathSystem : MonoBehaviour
{
    // First bool is if they died or if it's just a reset
    // Second bool is if the death delay just started or if it'll end
    public static Action<bool, bool> onReset;

    [Header("Settings")]
    [SerializeField] private int spawnerDelayFrames;

    private void OnEnable()
    {
        BodyCollider.death += CallDeath;
        Orb.gotOrb += CallReset;
    }
    private void OnDisable()
    {
        BodyCollider.death -= CallDeath;
        Orb.gotOrb -= CallReset;
    }
    void CallDeath() => StartCoroutine(Reset(true));
    void CallReset(int id) => StartCoroutine(Reset(false));
    private IEnumerator Reset(bool died)
    {
        onReset?.Invoke(died, false);

        yield return new WaitForSeconds(spawnerDelayFrames * Time.fixedDeltaTime);

        onReset?.Invoke(died, true);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
