using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorTip : MonoBehaviour
{
    private Image image;
    private Sprite white;
    private Sprite black;
    void Start()
    {
        GameManager.Instance.ColorChange += ChangeColor;
        white = Resources.Load<Sprite>("Material/white");
        black = Resources.Load<Sprite>("Material/black");
        image = GetComponent<Image>();
    }

    void ChangeColor(int a)
    {
        if ( a== 0)
        {
            image.sprite = black;
        }
        if ( a== 1) 
        {
            image.sprite = white;
        }
    }
    private void OnDestroy()
    {
        GameManager.Instance.ColorChange -= ChangeColor;    
    }
}
