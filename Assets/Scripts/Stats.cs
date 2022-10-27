using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stats : MonoBehaviour
{
    private bool started = false;
    private int resetCount = 0;
    private float timer;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI deathText;

    void OnEnable()
    {
        DeathSystem.onReset += OnReset;
        PlayerMovement.started += Started;
    }
    void OnDisable()
    {
        DeathSystem.onReset -= OnReset;
        PlayerMovement.started -= Started;
    }
    void Started() => started = true;
    void OnReset(bool died, bool post)
    {
        if (post)
            return;

        started = false;

        if (died)
            resetCount++;
        deathText.text = "RIP: " + resetCount.ToString();
    }
    void FixedUpdate()
    {
        if (started)
        {
            timer += Time.fixedDeltaTime;
            timerText.text = "T: " + timer.ToString("00.00");
        }
    }
}
