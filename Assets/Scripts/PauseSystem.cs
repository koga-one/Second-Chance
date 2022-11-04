using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    // DESCRIPTION ============================================

    // Takes care of the UI and mechanics related to pausing the game

    // VARIABLES ==============================================

    public static bool isPaused = false;

    // ACTIONS ================================================



    // PUBLIC VARIABLES =======================================

    [Header("References")]
    [SerializeField] private GameObject pauseMenu;

    // ACTION SUBSCRIPTIONS ===================================



    // ACTION FUNCTIONS =======================================



    // MONOBEHAVIOUR ==========================================



    // HELPER FUNCTIONS =======================================

    public void OnPause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0 : 1;
        pauseMenu.SetActive(isPaused);
    }

    // (✿◡‿◡) ================================================
}
