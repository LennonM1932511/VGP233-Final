using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControl : MonoBehaviour, IGameModule
{
    public static bool gameIsPaused;

    public IEnumerator LoadModule()
    {
        ServiceLocator.Register<PauseControl>(this);
        yield return null;
    }

    void Update()
    {
        if (!GameManager._isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameIsPaused = !gameIsPaused;
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
        }
        else
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
        }
        ServiceLocator.Get<GameManager>().DisplayPauseMenu();
    }
}
