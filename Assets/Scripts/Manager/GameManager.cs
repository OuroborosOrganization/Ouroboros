using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region ¼üÖµ
    public static KeyCode Up = KeyCode.W;
    public static KeyCode Down = KeyCode.S;
    public static KeyCode Left = KeyCode.A;
    public static KeyCode Right = KeyCode.D;
    #endregion
    #region µ¥Àý
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
    [SerializeField] private static int AllLevels = 6;
    [SerializeField] private int CurLevel = 1;
    [SerializeField] private int[] AllLevelsMoveTimes = new int[] { 4,6,8,9,12,13};
    private float Fluency;
    public float DeadTime;
    public List<GameObject> AllLevelsObjs = new List<GameObject> ();
    [SerializeField] private Material diedMaterial;
    [SerializeField] private float AnimationTime = 5f;
    [SerializeField] private float AnimationTimer = 0;
    [SerializeField] private float AnimationDeltTime = 0.02f;
    List<GameObject> allBody;
    public Material DiedMaterial
    { get { return diedMaterial; } }
    public CharacterMove WhiteSnake;
    public CharacterMove BlackSnake;
    private bool whiteArrive =false;
    public bool WhiteArrive
    {
        set
        {
            whiteArrive = value;
            if (whiteArrive & blackArrive)
            {
                Win(CurLevel);
            }
            else
                BlackSnake.Moveable = true;
        }
        get { return whiteArrive; }
    }
    private bool blackArrive = false;
    public bool BlackArrive
    {
        set 
        { 
            blackArrive = value;
            if (whiteArrive & blackArrive)
            {
                Win(CurLevel);
            }
            else
            WhiteSnake.Moveable = true;
        }
        get { return blackArrive; }
    }
    private int moveTimes;
    public int MoveTimes
    {
        get { return moveTimes; }
        set
        {
            moveTimes = value;
            if (moveTimes == 0)
            {
                GameOver();
            }
        }
    }

    public event Action<int> LevelChange;

    public void GameOver()
    {
        WhiteSnake?.Died();
        BlackSnake?.Died();
    }
    void ChangeLevel(int Level)
    {
        if(Level == AllLevels)
        {
            return;
        }
        AllLevelsObjs[Level - 1].SetActive(false);
        AllLevelsObjs[Level].SetActive(true);
    }
    void Win(int Level)
    {
        Fluency = WhiteSnake.Fluency;
        DeadTime = WhiteSnake.DeadTime;
        allBody = WhiteSnake.Body;
        for (int i = 0; i < BlackSnake.Body.Count; i++)
        {
            allBody.Add(BlackSnake.Body[i]);
        }
        Destroy(WhiteSnake.gameObject);
        Destroy(BlackSnake.gameObject);
        StartCoroutine("WinAnimation");
    }
    IEnumerator WinAnimation()
    {
        int n = allBody.Count;
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
        LevelChange(CurLevel);
        yield break;
    }
    public void ChangeSide(int fiction)
    {
        if (whiteArrive) {
            WhiteSnake.Moveable = false;
            BlackSnake.Moveable = true;
        }
        if (blackArrive)
        {
            WhiteSnake.Moveable = true;
            BlackSnake.Moveable = false;
        }
            if (fiction == 0)
        {
            WhiteSnake.Moveable = true;
            BlackSnake.Moveable = false;
        }
        else
        {
            WhiteSnake.Moveable = false;
            BlackSnake.Moveable = true;
        }
    }
    private void Start()
    {
        LevelChange += ChangeLevel;
        diedMaterial = Resources.Load<Material>("Material/Died");
        UIManager.Instance.gameObject.SetActive(true);
    }
    private void Update()
    {
        
    }
}
