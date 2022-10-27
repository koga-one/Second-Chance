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
        ResetSystem.onReset += OnReset;
        PlayerMovement.started += Started;
        Orb.gotOrb += GotOrb;
    }
    void OnDisable()
    {
        ResetSystem.onReset -= OnReset;
        PlayerMovement.started -= Started;
        Orb.gotOrb -= GotOrb;
    }
    void Started() => started = true;
    void GotOrb() => started = false;
    void OnReset(ResetType type, bool post)
    {
        if (post)
            return;

        started = false;

        if (type == ResetType.Death || type == ResetType.ManualReset)
            resetCount++;
        deathText.text = "Resets: " + resetCount.ToString();
    }
    void FixedUpdate()
    {
        if (started)
        {
            timer += Time.fixedDeltaTime;
            timerText.text = "Time: " + timer.ToString("00.00");
        }
    }
}
