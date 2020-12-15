using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    [SerializeField] private Text highscoreText = null;
    private HighScore highscore;

    private void Awake()
    {
        highscore = ServiceLocator.Get<SaveSystem>().LoadJSON<HighScore>("highscore.txt");

        if (highscore.highScore > 0)
        {
            highscoreText.GetComponent<Text>().text = highscore.highScore.ToString();
        }
        Debug.Log(highscore.highScore.ToString());
    }

    public void ButtonStart()
    {
        ServiceLocator.Get<GameManager>().LoadNextLevel();
    }

    public void ButtonExit()
    {
        Application.Quit();
    }
}
