using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                    Debug.Log("UIManager Created!");
                }
            }
            return instance;
        }
    }
    #endregion
    [SerializeField] private GameObject Menu;
    // Start is called before the first frame update
    void Start()
    {
        Menu = Instantiate(Resources.Load<GameObject>("Pre/Menu"));
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
}
