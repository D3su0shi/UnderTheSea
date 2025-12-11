using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameStatsManager : MonoBehaviour
{
    public static GameStatsManager Instance;

    // Data Storage
    public Dictionary<string, int> gameStats = new Dictionary<string, int>();

    [Header("Scene Settings")]
    public string gameSceneName = "undergroundCave";      // Name of your playable level
    public string gameOverSceneName = "gameLossCondition"; // Name of your new menu scene

    void Awake()
    {
        // 1. Singleton + Persistence Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // CRITICAL: This keeps the object alive across scenes
            InitializeStats();
        }
        else
        {
            // If we load back into the menu, a new Manager might try to spawn. 
            // We destroy the new one and keep the old one (which has the data).
            Destroy(gameObject);
        }
    }

    public void InitializeStats()
    {
        gameStats.Clear();
        gameStats.Add("TurtlesRescued", 0);
        gameStats.Add("EnemiesDefeated", 0);
        gameStats.Add("TotalScore", 0);
    }

    public void AddStat(string key, int value)
    {
        if (gameStats.ContainsKey(key))
        {
            gameStats[key] += value;
            // Auto-update total score
            if (key == "EnemiesDefeated") gameStats["TotalScore"] += (100 * value);
            if (key == "TurtlesRescued") gameStats["TotalScore"] += (500 * value);
        }
    }

    // Called by subScript when you die
    public void TriggerLoss()
    {
        Debug.Log("Loading Game Over Scene...");
        SceneManager.LoadScene(gameOverSceneName);
    }

    // Called by subScript when you win
    public void TriggerWin()
    {
        // You might want a separate "VictoryScene" or just send a flag
        SceneManager.LoadScene(gameOverSceneName);
    }
}