using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayHUD : MonoBehaviour
{
    [SerializeField] private Text MessageText = null;
    [SerializeField] private Text ScoreText = null;
    [SerializeField] private Text HealthText = null;
    [SerializeField] private Text BombText = null;
    [SerializeField] private Text KeyText = null;
    [SerializeField] private Text DataShardText = null;

    public void Initialize()
    {
        MessageText.text = string.Empty;
        ScoreText.text = "0";
    }

    public void SetGameplayHUDActive(bool shouldBeActive)
    {
        gameObject.SetActive(shouldBeActive);
    }

    public void UpdateScore(int currentScore)
    {
        ScoreText.text = currentScore.ToString();
    }

    public void UpdateHealth(float health)
    {
        int roundHealth = Mathf.RoundToInt(health);
        HealthText.text = roundHealth.ToString();
    }

    public void UpdateBombs(int bombs)
    {
        BombText.text = bombs.ToString();
    }

    public void UpdateKeys(string keyMessage)
    {
        KeyText.text = keyMessage;
    }
    public void UpdateDataShard(string shardMessage)
    {
        DataShardText.text = shardMessage;
    }
    
    public void UpdateMessageText(string message)
    {
        MessageText.text = message;
    }
}
