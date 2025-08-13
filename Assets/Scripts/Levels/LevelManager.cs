using System;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;

    [SerializeField] private string[] Levels;
    public static LevelManager Instance { get { return instance; } }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        if (GetLevelStatus(Levels[0]) == LevelStatus.Locked)
        {
            SetLevelStatus(Levels[0], LevelStatus.Unlocked);
        }
    }
    public void MarkCurrentLevelComplete()
    {
        //Setting the current Level Status to Complete
        Scene currentLevel = SceneManager.GetActiveScene(); 
        SetLevelStatus(currentLevel.name ,LevelStatus.Completed);

        //Setting the next Level Status to Unlocked
        int currentLevelIndex = Array.FindIndex(Levels, level => level == currentLevel.name);
        int nextLevelIndex = currentLevelIndex + 1;
        if (nextLevelIndex < Levels.Length)
        {
            SetLevelStatus(Levels[nextLevelIndex], LevelStatus.Unlocked);
        }
    }
    public LevelStatus GetLevelStatus(string level)
    {
       LevelStatus levelStatus = (LevelStatus) PlayerPrefs.GetInt(level, 0);
        return levelStatus;
    }
    public void SetLevelStatus(string level,LevelStatus levelStatus)
    {
        PlayerPrefs.SetInt(level,(int)levelStatus);
        Debug.Log("Setting Level : " + level + " Status" +  levelStatus);
    }
}
