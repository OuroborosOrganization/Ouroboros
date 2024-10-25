using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoad : MonoBehaviour
{
    void Awake()
    {
        for(int i = 0;i<transform.childCount;i++)
        {
            GameManager.Instance.AllLevelsObjs.Add(transform.GetChild(i).gameObject);
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
