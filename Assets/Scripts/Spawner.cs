using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SpawnerStates { Off, Recording, Replaying }

public class Spawner : MonoBehaviour
{
    /*
        Off:
            Does nothing
            if (has started and is current) => Recording
            if (has started and isn't current and has recorded) => Replaying
        Recording:
            Records frame every FixedUpdate
            if (gets orb) => Off and (has recorded = true) and (is current = false)
            if (dies) => Off and (has recorded = false)
        Replaying:
            Replays frames every FixedUpdate
            if (hasn't started) => Off
            if (reaches last frame) => (frameIndex = 0)
            if (hasn't reached last frame) => (frameIndex++)
    */

    bool hasStarted;
    bool isCurrent;
    bool hasRecorded;
}
