using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIInterect : MonoBehaviour
{
    public void OpenSetting()
    {

    }
    public void CloseSetting() { }
    public void BackGame() { 
        this.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void StartNewGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(1);
    }
    public void ContinueGame()
    {
        if(PlayerPrefs.GetInt("isSaved",0)==1)
        {
            SceneManager.LoadScene(1);
        }
    }
}
