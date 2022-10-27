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
    }
    private void OnDisable()
    {
        BodyCollider.death -= CallDeath;
    }
    void CallDeath() => StartCoroutine(Reset(true));
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
