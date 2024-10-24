using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
