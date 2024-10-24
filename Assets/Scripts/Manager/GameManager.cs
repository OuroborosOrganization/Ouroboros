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
    public static KeyCode ManuKey = KeyCode.Escape;
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

    public void GameOver()
    {
        WhiteSnake?.Died();
        BlackSnake?.Died();
    }
    void ChangeLevel(int Level)
    {

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
        diedMaterial = Resources.Load<Material>("Material/Died");
    }
    private void Update()
    {
        
    }
}
