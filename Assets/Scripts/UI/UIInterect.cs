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
        PlayerPrefs.DeleteKey("AutoSave");
        PlayerPrefs.DeleteKey("isSaved");
        SceneManager.LoadScene(1);
    }
    public void ContinueGame()
    {
        if(PlayerPrefs.GetInt("isSaved",0)==1)
        {
            SceneManager.LoadScene(1);
        }
    }
    public void Restart()
    {
        Time.timeScale = 1.0f;
        GameManager.Instance.Restart();
        gameObject.SetActive(false);
    }
    public void BackToMenu()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        Destroy(GameManager.Instance.gameObject);
        Destroy(UIManager.Instance.gameObject);
        SceneManager.LoadScene(0);
    }
}
