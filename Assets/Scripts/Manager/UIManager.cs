using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    #region ¼üÖµ
    public static KeyCode ManuKey = KeyCode.Escape;
    #endregion
    #region µ¥Àý
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
    [SerializeField] private GameObject Restart;
    [SerializeField] private TextMeshProUGUI TeachText;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject Menu;
    [SerializeField] private TextMeshProUGUI RemainStep;
    [SerializeField] private string[] TeachTexts = new string[] { "The white snake goes first, and presses WSAD to control the movement forward, backward, left, right, and left.",
        "When the arrow keys are pressed, the snake will travel in that direction until it hits an obstacle and consumes a number of progressions",
        "Obstacles are divided into crashable obstacles and non-collision obstacles, where the wall and one's body are non-collision obstacles, and touching them will cause the snake to die. ",
        "A snake body of a different color than oneself is not an obstacle and will not stop the snake from moving forward."
        ,"When both snakes are attached to each other's tails within the allotted number of steps, the level wins." ,
    "And Now, Start!"};
    // Start is called before the first frame update
    private void Load()
    {
        canvas = Instantiate(Resources.Load<GameObject>("Pre/Menu"));
        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            if (canvas.transform.GetChild(i).name == "Menu")
            {
                Menu = canvas.transform.GetChild(i).gameObject;
            }
            if (canvas.transform.GetChild(i).name == "RemainStep")
            {
                RemainStep = canvas.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            }
            if (canvas.transform.GetChild(i).name == "TeachText")
            {
                TeachText = canvas.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            }
            if (canvas.transform.GetChild(i).name == "Restart")
            {
                Restart = canvas.transform.GetChild(i).gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(ManuKey))
        {
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
        Time.timeScale = 0;
        for (int i = 0; i < TeachTexts.Length; i++)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            Debug.Log(i);
            TeachText.SetText(TeachTexts[i]);
            yield return new WaitUntil(() => { return Input.anyKeyDown; });
        }
        TeachText.gameObject.SetActive(false);
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
        TeachText.gameObject.SetActive(true);
        TeachText.SetText("All Levels are passed");
        TeachText.autoSizeTextContainer = true;
        yield return new WaitUntil(() => { return Input.anyKeyDown; });
        Time.timeScale = 1;
        TeachText.autoSizeTextContainer = false;
        TeachText.gameObject.SetActive(false);
        PlayerPrefs.DeleteKey("AutoSave");
        PlayerPrefs.DeleteKey("isSaved");
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
    public void RestartUI()
    {
        Restart.SetActive(true);
        Time.timeScale = 0;
    }
}
