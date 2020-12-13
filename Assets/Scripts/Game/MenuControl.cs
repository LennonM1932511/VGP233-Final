using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{    
    public void ButtonStart()
    {   
        ServiceLocator.Get<GameManager>().LoadNextLevel();
    }

    public void ButtonExit()
    {
        Application.Quit();
    }
}
