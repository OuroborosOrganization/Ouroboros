using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource Mplayer;
    #region µ¥Àý
    public static MusicPlayer instance;

    public static MusicPlayer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<MusicPlayer>();
                if (instance == null)
                {
                    GameObject gameObject = new GameObject("MusicPlayer");
                    instance = gameObject.AddComponent<MusicPlayer>();
                    gameObject.AddComponent<AudioSource>();
                    DontDestroyOnLoad(gameObject);
                    Debug.Log("MusicPlayer Created!");
                }
            }
            return instance;
        }
    }
    #endregion
    AudioClip BackGround;
    // Start is called before the first frame update
    void Start()
    {
        BackGround = Resources.Load<AudioClip>("Music/BackGround");
        Mplayer = GetComponent<AudioSource>();
        Mplayer.clip = BackGround;
        Mplayer.loop = true;
        Mplayer.Play();
    }

    public void SoundChange(float f)
    {
        Mplayer.volume = f;
    }
}
