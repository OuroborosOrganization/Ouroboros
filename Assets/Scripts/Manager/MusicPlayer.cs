using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static AudioSource background;
    public static AudioSource Effect;
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
                    background = gameObject.AddComponent<AudioSource>();
                    Effect = gameObject.AddComponent<AudioSource>();
                    DontDestroyOnLoad(gameObject);
                    Debug.Log("MusicPlayer Created!");
                }
            }
            return instance;
        }
    }
    #endregion
    AudioClip BackGround;
    AudioClip Fail;
    AudioClip NextLevel;
    AudioClip Arrive;
    AudioClip PressKey;
    // Start is called before the first frame update
    void Start()
    {
        BackGround = Resources.Load<AudioClip>("Music/BackGround");
        Fail = Resources.Load<AudioClip>("Music/Fail");
        NextLevel = Resources.Load<AudioClip>("Music/NextLevel");
        Arrive = Resources.Load<AudioClip>("Music/Arrive");
        PressKey = Resources.Load<AudioClip>("Music/PressKey");
        background.clip = BackGround;
        background.loop = true;
        background.Play();
        Effect.loop = false;
    }

    public void SoundChange(float f)
    {
        background.volume = f;
        Effect.volume = f;
    }
    public void PlayEffect(int type)
    {
        Debug.Log("PlayEffect  " + type);
        switch (type)
        {
            case 0:
                Effect.clip = Arrive;
                break;
            case 1:
                Effect.clip = Fail;
                break;
            case 2:
                Effect.clip = NextLevel;
                break;
            case 3:
                Effect.clip = PressKey;
                break;
        }
        Effect.Play();
    }
}
