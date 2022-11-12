using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HueChanger : MonoBehaviour
{
    // DESCRIPTION ============================================

    // Changes the hue of the sprite it is attached to

    // VARIABLES ==============================================

    // Always between 0 and 1
    private float current;
    private Color initialColor;

    // PUBLIC VARIABLES =======================================



    // ACTIONS ================================================



    // INSPECTOR VARIABLES ====================================

    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float speed;

    // ACTION SUBSCRIPTIONS ===================================



    // ACTION FUNCTIONS =======================================



    // MONOBEHAVIOUR ==========================================

    private void OnEnable()
    {
        initialColor = spriteRenderer.color;
    }
    private void OnDisable()
    {
        // Ensure it doesn't stay colored
        spriteRenderer.color = initialColor;
    }
    private void FixedUpdate()
    {
        current += Time.fixedDeltaTime * speed;
        while (current >= 1)
            current -= 1;

        spriteRenderer.color = Color.HSVToRGB(current, 1, 1);
    }

    // HELPER FUNCTIONS =======================================



    // (✿◡‿◡) ================================================
}
