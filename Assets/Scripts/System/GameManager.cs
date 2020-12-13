using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool _isGameOver;

    private static readonly Dictionary<int, int> _EnemiesPerLevel = new Dictionary<int, int>()
    {
        { 1, 30 },
        { 2, 30 }
    };

    private static readonly Dictionary<int, int> _KeyPerLevel = new Dictionary<int, int>()
    {
        { 1, 3 },
        { 2, 3 }
    };

    private static readonly Dictionary<int, int> _DataShardsPerLevel = new Dictionary<int, int>()
    {
        { 1, 3 },
        { 2, 3 }
    };

    private int _numKilled = 0;

    private int _dataShards = 0;
    public int DataShards { get { return _dataShards; } }

    private int _currentKeys = 0;
    public int CurrentKeys { get { return _currentKeys; } }

    private int _currentBombs = 0;
    public int CurrentBombs { get { return _currentBombs; } }

    private float _currentHealth = 0.0f;
    private float _maxHealth = 100.0f;

    private int _currentScore = 0;
    public int CurrentScore { get { return _currentScore; } }

    private int _currentLevel = 0;
    public int CurrentLevel { get { return _currentLevel; } }

    private UIManager _uiManager = null;

    public GameManager Initialize(int startLevel)
    {
        GameLoader.CallOnComplete(OnGameLoaderComplete);
        SetLevel(startLevel);
        _currentHealth = _maxHealth;
        _isGameOver = false;
        AudioListener.pause = false;
        return this;
    }

    private void OnGameLoaderComplete()
    {
        _uiManager = ServiceLocator.Get<UIManager>();
    }

    private void SetLevel(int level)
    {
        _currentLevel = level;
    }

    public void LoadNextLevel()
    {
        int nextLevel = ++_currentLevel;

        SceneManager.LoadScene(nextLevel);
        SetLevel(nextLevel);
        _numKilled = 0;
        _currentKeys = 0;
        _dataShards = 0;
        UpdateKeys(0);
        UpdateDataShards(0);
        _uiManager.DisplayMessage("");
        Time.timeScale = 1;

        if (_isGameOver)
        {
            _isGameOver = false;
        }
    }

    public void UpdateScore(int score)
    {
        _currentScore += score;
        _uiManager.UpdateScoreDisplay(_currentScore);
    }

    public void UpdateKills()
    {
        ++_numKilled;
    }

    public void UpdateHealth(float health)
    {
        _currentHealth += health;
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
        else if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            CheckLoseCondition();
        }
        _uiManager.UpdateHealthDisplay(_currentHealth);
    }

    public void UpdateBombs(int bombs)
    {
        _currentBombs += bombs;
        _uiManager.UpdateBombsDisplay(_currentBombs);
    }

    public void UpdateKeys(int keys)
    {
        _currentKeys += keys;
        string keyText = _currentKeys + "/" + _KeyPerLevel[_currentLevel - 1];
        _uiManager.UpdateKeysDisplay(keyText);
        CheckWinCondition();
    }

    public void UpdateDataShards(int shards)
    {
        _dataShards += shards;
        float percentage = ((float)_dataShards / (float)_DataShardsPerLevel[_currentLevel - 1]) * 100.0f;
        string shardText = System.Math.Round(percentage, 0) + "%";
        _uiManager.UpdateDataShardDisplay(shardText);
    }

    IEnumerator DelayLevelLoad(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        LoadNextLevel();        
    }

    private void CheckWinCondition()
    {
        int numToWin = _KeyPerLevel[_currentLevel - 1];
        if (_currentKeys >= numToWin)
        {            
            _uiManager.DisplayMessage("LEVEL COMPLETED");
            ServiceLocator.Get<SoundManager>().PlayAudio(SoundManager.Sound.End_LevelComplete);
            Time.timeScale = 0;
            StartCoroutine(DelayLevelLoad(3.0f));
        }
    }

    private void CheckLoseCondition()
    {
        if (_currentHealth <= 0)
        {
            _uiManager.DisplayMessage("GAME OVER");
            ServiceLocator.Get<SoundManager>().PlayAudio(SoundManager.Sound.End_GameOver);
            _isGameOver = true;            
            Time.timeScale = 0;
        }
    }

    public void DisplayPauseMenu()
    {
        if (PauseControl.gameIsPaused)
        {
            _uiManager.DisplayMessage("PAUSED");
        }
        else
        {
            _uiManager.DisplayMessage("");
        }
    }
}
