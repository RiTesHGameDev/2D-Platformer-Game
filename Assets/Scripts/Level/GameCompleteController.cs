using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCompleteController : MonoBehaviour
{
    public GameFinishController gameFinishController;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            Debug.Log("Game Complete");
            LevelManager.Instance.MarkCurrentLevelComplete();
            SoundManager.Instance.Play(Sounds.levelComplete);
            gameFinishController.GameFinished();
        }

    }
}
