using System;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    [SerializeField]private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField]private GameObject LevelSelection;

    private void Awake()
    {
        playButton.onClick.AddListener(PlayGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void QuitGame()
    {
        Debug.Log("Application Quit !");
        Application.Quit();
    }

    private void PlayGame()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        LevelSelection.SetActive(true);
    }
}
