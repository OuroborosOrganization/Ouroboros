using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region 键值
    public static KeyCode ManuKey = KeyCode.Escape;
    #endregion
    #region 单例
    public static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<UIManager>();
                if (instance == null)
                {
                    GameObject gameObject = new GameObject("UIManager");
                    instance = gameObject.AddComponent<UIManager>();
                    DontDestroyOnLoad(gameObject);
                    instance.Load();
                    Debug.Log("UIManager Created!");
                }
            }
            return instance;
        }
    }
    #endregion
    private Coroutine FCor = null;
    private GameObject Restart;
    private TextMeshProUGUI TeachText;
    private GameObject canvas;
    private GameObject Menu;
    private GameObject Tip;
    private GameObject colorTip;
    private TextMeshProUGUI RemainStep;
    private GameObject Settings;
    private GameObject WinImage;
    private List<Image> UITeach = new List<Image>();
    private string[] TeachTexts = new string[] {"黑白双蛇回合制移动，在限制回合数内达成衔尾连接即完成关卡\n" +
        "Black and white double snake turn-based movement, reach the tail connection within the limited number of rounds to complete the level."
        ,"灰色方块为障碍物，触碰到障碍物后停止运动\n"+"The gray square is an obstacle. Stop moving after touching the obstacle."};
    public event Action<CharacterMove> DirHighLighted;
    // Start is called before the first frame update
    private void Load()
    {
        canvas = Instantiate(Resources.Load<GameObject>("Pre/Menu"));
        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            switch(canvas.transform.GetChild(i).name)
            {
                case "Menu":
                    Menu = canvas.transform.GetChild(i).gameObject;
                    break;
                case "RemainStep":
                    RemainStep = canvas.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
                    break;
                case "TeachText":
                    TeachText = canvas.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
                    break;
                case "Restart":
                    Restart = canvas.transform.GetChild(i).gameObject;
                    break;
                case "Tip":
                    Tip = canvas.transform.GetChild(i).gameObject;
                    break;
                case "ColorTip":
                    colorTip = canvas.transform.GetChild(i).gameObject;
                    break;
                case "Arrors":
                    Transform p = canvas.transform.GetChild(i);
                    for (int j = 0; j < p.childCount; j++)
                    {
                        UITeach.Add(p.GetChild(j).GetComponent<Image>());
                    }
                    break;
                case "Settings":
                    Settings = canvas.transform.GetChild(i).gameObject;
                    break;
                case "WinImage":
                    WinImage = canvas.transform.GetChild(i).gameObject;
                    break;
                default:
                    break;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        OpenManu();
    }
    private void OpenManu()
    {
        if (Input.GetKeyUp(ManuKey))
        {
            if (Settings.activeInHierarchy) { return; }
            Time.timeScale = (Menu.activeInHierarchy) ? 1 : 0;
            Menu.SetActive(!Menu.activeInHierarchy);
        }
    }
    public void ChangeStep()
    {
        RemainStep?.SetText("RemainStep: " + GameManager.Instance.MoveTimes);
    }
    public IEnumerator Teach()
    {
        GameManager.Instance.WhiteSnake.Moveable = false;
        TeachText.gameObject.SetActive(true);
        TeachText.SetText("");
        Time.timeScale = 0;
        for (int i = 0; i < TeachTexts.Length; i++)
        {
            TeachText.SetText(TeachTexts[i]);
            StartCoroutine(FlashOnce(TeachText));
            yield return new WaitForSecondsRealtime(1.5f);
            yield return new WaitUntil(() => { return Input.anyKeyDown; });
        }
        TeachText.gameObject.SetActive(false);
        TeachText.SetText("");
        for (int i = 0; i < UITeach.Count; i++)
        {
            StartFlash(i);
            yield return new WaitForSecondsRealtime(1.5f);
            yield return new WaitUntil(() => { return Input.anyKeyDown; });
        }
        StopFlash();
        Time.timeScale = 1;
        GameManager.Instance.WhiteSnake.Moveable = true;
    }
    public void WinUI()
    {
        StartCoroutine(Win());
    }
    private IEnumerator Win()
    {
        Time.timeScale = 0;
        WinImage.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        yield return new WaitUntil(() => { return Input.anyKeyDown; });
        Time.timeScale = 1;
        TeachText.autoSizeTextContainer = false;
        TeachText.gameObject.SetActive(false);
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
    public void RestartUI()
    {
        colorTip.SetActive(false);
        Tip.SetActive(false);
        Restart.SetActive(true);
        Restart.GetComponent<Image>().color = Color.black;
        Time.timeScale = 0;
        StartCoroutine(Black());
    }
    IEnumerator Black()
    {
        Image image = Restart.GetComponent<Image>();
        float timer = 0;
        while(timer < 3)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Sin(Mathf.PI*timer/3f));
            timer += 0.01f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        Time.timeScale = 1;
        Tip.SetActive(true);
        colorTip.SetActive(true);
        GameManager.Instance.Restart();
        Restart.SetActive(false );
    }
    public void StartFlash(int index)
    {
        StopFlash();
        if (index >= UITeach.Count)
        {
            return;
        }
        FCor = StartCoroutine(Flash(UITeach[index]));
    }
    IEnumerator Flash(Image target)
    {
        TextMeshProUGUI E = target.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI C = target.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        float time = 5f;
        float timer = 0f;
        int flag = 1;
        target.gameObject.SetActive(true);
        target.color = new Color(1, 1, 1, 0);
        E.color = new Color(1, 1, 1, 0);
        C.color = new Color(1, 1, 1, 0);
        while (true)
        {
            target.color += new Color(0, 0, 0, 0.02f) * flag * (1 + Mathf.Sin(Mathf.PI * timer)) * 0.5f;
            if(C.color.a<0.9f)
            {
                E.color += new Color(0, 0, 0, 0.02f) * flag * (1 + Mathf.Sin(Mathf.PI * timer)) * 0.5f;
                C.color += new Color(0, 0, 0, 0.02f) * flag * (1 + Mathf.Sin(Mathf.PI * timer)) * 0.5f;
            }
            if (target.color.a > 0.9f || target.color.a <= 0)
            {
                timer = 0;
                flag *= -1;
            }
            timer += 0.02f;
            yield return new WaitForSecondsRealtime(0.02f);
        }
    }
    IEnumerator FlashOnce(Image target)
    {
        target.gameObject.SetActive(true);
        target.color = new Color(1, 1, 1, 0);
        float timer = 0;
        while(target.color.a<0.95f)
        {
            target.color += new Color(0, 0, 0, 0.02f) * (1 + Mathf.Sin(Mathf.PI * timer)) * 0.5f;
            yield return new WaitForSecondsRealtime(0.02f);
            timer += 0.02f;
        }
        yield break;
    }
    IEnumerator FlashOnce(TextMeshProUGUI target)
    {
        target.gameObject.SetActive(true);
        target.color = new Color(1, 1, 1, 0);
        float timer = 0;
        while (target.color.a < 0.95f)
        {
            target.color += new Color(0, 0, 0, 0.02f) * (1 + Mathf.Sin(Mathf.PI * timer)) * 0.5f;
            yield return new WaitForSecondsRealtime(0.02f);
            timer += 0.02f;
        }
        yield break;
    }

    public void StopFlash()
    {
        for (int i = 0; i < UITeach.Count; i++)
        {
            UITeach[i].gameObject.SetActive(false);
        }
        if (FCor != null) { StopCoroutine(FCor); FCor = null; }
    }
}
