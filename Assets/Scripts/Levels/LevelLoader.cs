using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelLoader : MonoBehaviour
{
    private Button button;
    [SerializeField] private string LevelName;

    public void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        LevelStatus levelStatus = LevelManager.Instance.GetLevelStatus(LevelName);
        switch(levelStatus)
        {
            case LevelStatus.Locked:
                SoundManager.Instance.Play(Sounds.UnlockedButtonClick);
                Debug.Log("Cant Play this level till you unlocked it!");
                break;
            case LevelStatus.Unlocked:
                SoundManager.Instance.Play(Sounds.ButtonClick);
                SceneManager.LoadScene(LevelName);
                break;
            case LevelStatus.Completed:
                SoundManager.Instance.Play(Sounds.ButtonClick);
                SceneManager.LoadScene(LevelName);
                break;
        }
        
    }
}
