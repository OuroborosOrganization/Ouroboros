using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvevt : MonoBehaviour
{

    public void startGame()
    {
        SceneManager.LoadScene("test3");
    }
    public void exitGame()
    {
        Application.Quit();
    }
}
