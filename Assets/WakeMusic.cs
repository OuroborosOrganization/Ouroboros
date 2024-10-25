using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeMusic : MonoBehaviour
{
    private void Start()
    {
        MusicPlayer.Instance.enabled = true;
    }
}
