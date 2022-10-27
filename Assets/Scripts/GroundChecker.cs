using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GroundChecker : MonoBehaviour
{
    private int amountGrounded = 0;
    private bool isGrounded;
    public bool IsGrounded => isGrounded;

    void OnTriggerEnter2D(Collider2D col)
    {
        amountGrounded++;
        isGrounded = amountGrounded > 0;
    }
    void OnTriggerExit2D()
    {
        amountGrounded--;
        isGrounded = amountGrounded > 0;
    }
}
