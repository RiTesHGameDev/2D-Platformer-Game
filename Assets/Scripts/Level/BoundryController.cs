using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundryController : MonoBehaviour
{
    [SerializeField] GameOverController gameOverController;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            gameOverController.PlayerDied();
        }
    }
}
