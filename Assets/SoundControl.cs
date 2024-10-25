using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    private Slider m_Slider;
    void Start()
    {
        m_Slider = GetComponent<Slider>();
        m_Slider.onValueChanged.AddListener(MusicPlayer.Instance.SoundChange);
        m_Slider.value = 1;
    }
}
