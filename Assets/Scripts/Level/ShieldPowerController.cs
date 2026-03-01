using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerController : MonoBehaviour
{
    [SerializeField] private float duration = 5f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            SoundManager.Instance.Play(Sounds.Key);
            if (playerController != null)
            {
                playerController.ActiveShield(duration);
            }
            Destroy(gameObject);
        }

    }
}
