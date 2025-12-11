using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class optionsciript : MonoBehaviour
{
    [Header("UI References")]
    // CHANGED: "Text" -> "TextMeshProUGUI"
    public TextMeshProUGUI statsDisplayText;
    public GameObject statsPanel;

    // --- RESTART BUTTON ---
    public void OnRestartPressed()
    {
        if (GameStatsManager.Instance != null)
        {
            GameStatsManager.Instance.InitializeStats();

            string sceneToLoad = GameStatsManager.Instance.gameSceneName;
            Debug.Log($"Restarting: Loading Scene '{sceneToLoad}'...");
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Error: GameStatsManager not found. Did you play from the Menu?");
        }
    }

    // --- CHECK STATS BUTTON ---
    public void OnCheckStatsPressed()
    {
        Debug.Log("Check Stats Button Pressed!"); // 1. Verify button click works

        if (GameStatsManager.Instance == null)
        {
            Debug.LogError("GameStatsManager is missing! Stats cannot be loaded."); // 2. Verify Manager exists
            return;
        }

        // 1. Build the Report String
        Dictionary<string, int> stats = GameStatsManager.Instance.gameStats;
        string report = "--- MISSION REPORT ---\n\n";

        if (stats.ContainsKey("EnemiesDefeated"))
            report += $"Enemies Defeated: {stats["EnemiesDefeated"]}\n";

        if (stats.ContainsKey("TurtlesRescued"))
            report += $"Turtles Rescued: {stats["TurtlesRescued"]}\n";

        if (stats.ContainsKey("TotalScore"))
            report += $"\nTOTAL SCORE: {stats["TotalScore"]}";

        // 2. Update the UI Text
        if (statsDisplayText != null)
        {
            statsDisplayText.text = report;

            if (statsPanel != null)
                statsPanel.SetActive(!statsPanel.activeSelf);
        }
        else
        {
            Debug.Log(report);
        }
    }
}
