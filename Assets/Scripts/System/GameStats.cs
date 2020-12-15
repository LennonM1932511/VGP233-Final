using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStats : MonoBehaviour
{
    [SerializeField] private Text dataText = null;
    [SerializeField] private Text killsText = null;
    [SerializeField] private Text scoreText = null;

    private void Awake()
    {
        // Calculate and display final data shards percentage
        int totalData = ServiceLocator.Get<GameManager>().TotalDataShards;
        float dataPercent = ((float)totalData / 200) * 100.0f; // hardcoded for now
        dataText.text = System.Math.Round(dataPercent, 0) + "%";
        Debug.Log("Player collected " + totalData + " Data Shards.");

        // Calculate and display final kills percentage
        int totalKills = ServiceLocator.Get<GameManager>().TotalKills;
        float killPercent = ((float)totalKills / 28) * 100.0f; // hardcoded for now
        killsText.text = System.Math.Round(killPercent, 0) + "%";
        Debug.Log("Player killed " + totalKills + " Enemies.");

        // Calculate and display final score
        int totalScore = ServiceLocator.Get<GameManager>().CurrentScore;
        scoreText.text = totalScore.ToString();
        Debug.Log("Player got a score of " + totalScore);

        // Create new high score and load previous best score
        HighScore finalScore = new HighScore { highScore = totalScore };
        HighScore bestScore = ServiceLocator.Get<SaveSystem>().LoadJSON<HighScore>("highscore.txt");

        // compare scores and store if new score is higher
        if (finalScore.highScore > bestScore.highScore)
        {
            ServiceLocator.Get<SaveSystem>().SaveJSON<HighScore>(finalScore, "highscore.txt");
            Debug.Log("New High Score!");
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ServiceLocator.Get<GameManager>().SetLevel(0);
            GameManager._isGameOver = true;
            ServiceLocator.Get<GameManager>().LoadNextLevel();
        }
    }
}

