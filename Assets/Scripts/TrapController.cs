using System;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Character"))
        {
            GameManager.Instance.EndGame();
        }
    }
}
