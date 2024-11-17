using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region 键值
    public static KeyCode Up = KeyCode.W;
    public static KeyCode Down = KeyCode.S;
    public static KeyCode Left = KeyCode.A;
    public static KeyCode Right = KeyCode.D;
    public static KeyCode RestartKey = KeyCode.Space;
    #endregion
    #region 单例
    public static GameManager instance;

    public static GameManager Instance
    { 
        get 
        { 
            if (instance == null)
            {
                instance = FindAnyObjectByType<GameManager>();
                if(instance == null)
                {
                    isExist = true;
                    GameObject gameObject = new GameObject("GameManager");
                    instance = gameObject.AddComponent<GameManager>();
                    DontDestroyOnLoad(gameObject);
                    Debug.Log("GameManager Created!");
                }
            }
            return instance; 
        } 
    }
    #endregion
    public static bool isExist = false;
    [SerializeField] private static int AllLevels = 6;
    public int CurLevel = 1;
    [SerializeField] private int[] AllLevelsMoveTimes = new int[] { 4,6,8,9,12,13};
    [SerializeField] public Direction[] TeachSteps = new Direction[] {Direction.Up,Direction.Down,Direction.Left,Direction.Right};  
    public float Fluency;
    public float DeadTime;
    public List<GameObject> AllLevelsObjs = new List<GameObject> ();
    [SerializeField] private Material diedMaterial;
    [SerializeField] private float AnimationTime = 5f;
    [SerializeField] private float AnimationTimer = 0;
    [SerializeField] private float AnimationDeltTime = 0.02f;
    public StartDetect[] WhiteStarts = new StartDetect[AllLevels];
    public StartDetect[] BlackStarts = new StartDetect[AllLevels];
    List<GameObject> allBody;
    public Material DiedMaterial
    { get { return diedMaterial; } }
    public CharacterMove WhiteSnake;
    public CharacterMove BlackSnake;
    private CharacterMove curMovingSnake;
    private Direction curMovingDir;
    public CharacterMove CurMovingSnake
    {
        get
        {
            return curMovingSnake;
        }
        set
        {
            curMovingSnake = value;
            //可能为null
            if(CurLevel == 1)
            {
                Move?.Invoke(curMovingSnake);
            }
        }
    }
    [SerializeField] private bool whiteArrive =false;
    public bool WhiteArrive
    {
        set
        {
            whiteArrive = value;
            if (curMovingSnake == WhiteSnake) { CurMovingSnake = null; }
            if (whiteArrive & blackArrive)
            {
                Win(CurLevel);
            }
            else
            BlackSnake.Moveable = true;
        }
        get { return whiteArrive; }
    }
    [SerializeField] private bool blackArrive = false;
    public bool BlackArrive
    {
        set 
        { 
            blackArrive = value;
            if (curMovingSnake == BlackSnake) { CurMovingSnake = null; }
            if (whiteArrive & blackArrive)
            {
                Win(CurLevel);
            }
            else
            WhiteSnake.Moveable = true;
        }
        get { return blackArrive; }
    }
    [SerializeField]private int moveTimes;
    private bool over = false;
    public int MoveTimes
    {
        get { return moveTimes; }
        set
        {
            moveTimes = value;
            UIManager.Instance.ChangeStep();
            if (moveTimes == 0)
            {
                StartCoroutine("Wait1S");  
            }
        }
    }

    public event Action<int> LevelChange;
    public event Action<CharacterMove> Move;
    public event Action<int> ColorChange;
    public event Action<Direction> TeachFlash ;
    public event Action<int> EffectPlay;
    public void InvokeEffectPlay(int type)
    {
        EffectPlay?.Invoke(type);
    }
    public void GameOver()
    {
        if(!over)
        {
            EffectPlay?.Invoke(1);
            over = true;
            WhiteSnake?.Died();
            BlackSnake?.Died();
            CurMovingSnake = null;
            UIManager.Instance.Invoke("RestartUI", 3f);         
        }
    }
    void ChangeLevel(int Level)
    {
        ColorChange?.Invoke(1);
        if (Level > AllLevels)
        {
            instance = null;
            UIManager.Instance.WinUI();
            Destroy(gameObject);
            return;
        }
        if (CurLevel != Level) { EffectPlay?.Invoke(2); }
        CurLevel = Level;
        MoveTimes = AllLevelsMoveTimes[Level - 1];
        whiteArrive = false;
        blackArrive = false;
        AllLevelsObjs[Level-1].SetActive(true);
        WhiteStarts[Level - 1].Func();
        WhiteStarts[Level - 1].arrive = false;
        BlackStarts[Level - 1].Func();
        BlackStarts[Level - 1].arrive = false;
        if (Level == 1 && PlayerPrefs.GetInt("Teach", 0) == 0)
        {
            StartCoroutine(UIManager.Instance.Teach());
            PlayerPrefs.SetInt("Teach", 1);
            PlayerPrefs.Save();
        }
        PlayerPrefs.SetInt("AutoSave",Level);
        PlayerPrefs.SetInt("isSaved", 1);
        PlayerPrefs.Save();
    }
    void Win(int Level)
    {
        allBody = WhiteSnake.Body;
        for (int i = 0; i < BlackSnake.Body.Count; i++)
        {
            allBody.Add(BlackSnake.Body[i]);
        }
        Destroy(WhiteSnake.gameObject);
        Destroy(BlackSnake.gameObject);
        StartCoroutine("WinAnimation");
    }
    IEnumerator Wait1S()
    {
        yield return new WaitForSeconds(1);
        if (!(whiteArrive && blackArrive))
        {
            GameOver();
        }
    }
    IEnumerator WinAnimation()
    {
        int n = allBody.Count;
        AnimationTimer = 0;
        while(AnimationTimer <= AnimationTime)
        {
            for (int i = 0; i < n; i++)
            {
                if (i + 1 == n)
                {
                    allBody[i].transform.localScale = allBody[0].transform.localScale;
                    allBody[i].transform.position = allBody[0].transform.position;
                    break;
                }
                allBody[i].transform.localScale = allBody[i + 1].transform.localScale;
                allBody[i].transform.position = allBody[i + 1].transform.position;
            }
            float speed = 1 + Mathf.Sin(AnimationTimer / AnimationTime * Mathf.PI) * 4;
            AnimationTimer += AnimationDeltTime / speed;
            yield return new WaitForSecondsRealtime(AnimationDeltTime / speed);
        }
        float DeadTimer = 0;

        Shader.SetGlobalFloat("unityTime", DeadTimer);
        for (int i = 0; i < allBody.Count; i += (int)(transform.localScale.x / Fluency) / 3)
        {
            allBody[i].transform.localScale = Vector3.one * transform.localScale.x * UnityEngine. Random.Range(0.3f, 0.9f);
            allBody[i].GetComponent<MeshRenderer>().material = new Material(GameManager.Instance.DiedMaterial);
            allBody[i].AddComponent<BodyDestory>();
            allBody[i].GetComponent<MeshRenderer>().material.SetFloat("_ShaderSatrtTime", DeadTimer);
            allBody.Remove(allBody[i]);
        }
        for (int i = 0; i < allBody.Count; i++)
        {
            Destroy(allBody[i]);
        }
        allBody.Clear();
        while(DeadTimer<DeadTime)
        {
            Shader.SetGlobalFloat("unityTime", DeadTimer);
            DeadTimer += Time.fixedDeltaTime;
            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
        }
        LevelChange?.Invoke(CurLevel+1);
        yield break;
    }
    public void ChangeSide(int fiction)
    {
        if (whiteArrive) {
            ColorChange?.Invoke(0);
            WhiteSnake.Moveable = false;
            BlackSnake.Moveable = true;
        }
        else if (blackArrive)
        {
            ColorChange?.Invoke(1);
            WhiteSnake.Moveable = true;
            BlackSnake.Moveable = false;
        }
        else if (fiction == 0)
        {
            ColorChange?.Invoke(1);
            WhiteSnake.Moveable = true;
            BlackSnake.Moveable = false;
        }
        else
        {
            ColorChange?.Invoke(0);
            WhiteSnake.Moveable = false;
            BlackSnake.Moveable = true;
        }
        MoveTimes--;
        if(CurLevel == 1)
        {
            if (MoveTimes <= 0) {TeachFlash(Direction.NULL); return; }
            TeachFlash?.Invoke(TeachSteps[4-MoveTimes]);
        }
    }
    private void Start()
    {
        LevelChange += ChangeLevel;
        diedMaterial = Resources.Load<Material>("Material/Died");
        LevelChange?.Invoke(PlayerPrefs.GetInt("AutoSave", 1));
        UIManager.Instance.gameObject.SetActive(true);
        EffectPlay += MusicPlayer.Instance.PlayEffect;
    }
    public void StartNewGame()
    {
        LevelChange?.Invoke(1);
    }
    public void Restart()
    {
        over = false;
        LevelChange?.Invoke(CurLevel);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&(!(whiteArrive&&blackArrive)) && CurLevel!=1)
        {
            GameOver();
        }
    }
}
