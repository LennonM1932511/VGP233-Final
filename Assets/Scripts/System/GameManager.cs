using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool _isGameOver;

    private static readonly Dictionary<int, int> _EnemiesPerLevel = new Dictionary<int, int>()
    {
        { 1, 13 },
        { 2, 15 }
    };

    private static readonly Dictionary<int, int> _KeyPerLevel = new Dictionary<int, int>()
    {
        { 1, 3 },
        { 2, 3 }
    };

    private static readonly Dictionary<int, int> _DataShardsPerLevel = new Dictionary<int, int>()
    {
        { 1, 100 },
        { 2, 100 }
    };

    private static readonly Dictionary<int, string> _LevelMessage = new Dictionary<int, string>()
    {
        { 1, "PRESS MOUSE2\nTO THROW BOMBS" },
        { 2, "NEW WEAPON\nPRESS Q TO SWITCH" }
    };

    private int _numKilled = 0;
    public int TotalKills { get { return _numKilled; } }

    private int _totalDataShards = 0;
    public int TotalDataShards { get { return _totalDataShards; } }

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
        _totalDataShards = 0;
        AudioListener.pause = false;
        return this;
    }

    private void OnGameLoaderComplete()
    {
        _uiManager = ServiceLocator.Get<UIManager>();
    }

    public void SetLevel(int level)
    {
        _currentLevel = level;
    }

    public void LoadNextLevel()
    {
        int nextLevel = ++_currentLevel;

        // Save player score between levels for reinstating on death, as well as a penalty
        ServiceLocator.Get<SaveSystem>().SavePlayerPrefs(_currentScore, "tempscore");
        ServiceLocator.Get<SaveSystem>().SavePlayerPrefs((_currentScore * 0.25f), "penalty");
        ServiceLocator.Get<SaveSystem>().SavePlayerPrefs(_numKilled, "tempkills");
        ServiceLocator.Get<SaveSystem>().SavePlayerPrefs(_totalDataShards, "tempdatashards");


        if (_isGameOver)
        {
            _isGameOver = false;
            _currentHealth = 100.0f;
            _numKilled = ServiceLocator.Get<SaveSystem>().LoadInt("tempkills");
            _totalDataShards = ServiceLocator.Get<SaveSystem>().LoadInt("tempdatashards");
            _currentScore = ServiceLocator.Get<SaveSystem>().LoadInt("tempscore");            
            _currentScore -= (int)ServiceLocator.Get<SaveSystem>().LoadFloat("penalty");
            _currentBombs = 0;
            _numKilled = 0;
            _totalDataShards = 0;
        }

        SceneManager.LoadScene(nextLevel);
        SetLevel(nextLevel);
        Time.timeScale = 1;

        _totalDataShards += _dataShards;

        if (CurrentLevel < 4 && CurrentLevel > 1)
        {
            _currentKeys = 0;
            _dataShards = 0;
            UpdateHealth(0);
            UpdateBombs(0);
            UpdateKeys(0);
            UpdateDataShards(0);
            UpdateScore(0);
            _uiManager.DisplayMessage("");
            StartCoroutine(DisplayLevel());
        }

    }

    IEnumerator DisplayLevel()
    {
        _uiManager.DisplayMessage("LEVEL " + (_currentLevel - 1).ToString() + "\nSTART!");
        yield return new WaitForSecondsRealtime(2.0f);
        if (_currentLevel > 1)
        {
            _uiManager.DisplayMessage(_LevelMessage[_currentLevel - 1]);
            yield return new WaitForSecondsRealtime(2.0f);
        }
        _uiManager.DisplayMessage("");
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
        // Delay loading next level while showing messages
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
            _uiManager.DisplayMessage("SYSTEM FAILURE\nREBOOTING...");
            ServiceLocator.Get<SoundManager>().PlayAudio(SoundManager.Sound.End_GameOver);
            _isGameOver = true;
            Time.timeScale = 0;
            SetLevel(_currentLevel - 1);
            StartCoroutine(DelayLevelLoad(3.0f));
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
