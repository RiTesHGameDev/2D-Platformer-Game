using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameFinishController : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    private Animator animator;
    private void Awake()
    {
        mainMenuButton.onClick.AddListener(LoadLobby);
        animator = GetComponent<Animator>();
    }
    public void GameFinished()
    {
        gameObject.SetActive(true);
    }
    private void LoadLobby()
    {
        SceneManager.LoadScene(0);
    }
}
