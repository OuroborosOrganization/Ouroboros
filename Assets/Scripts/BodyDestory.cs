using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyDestory : MonoBehaviour
{
    [SerializeField] private float time = 1.5f;
    private float timer = 0f;
    BodyDestory(float time)
    {
        this.time = time;
    }
    BodyDestory()
    {

    }
    private void Update()
    {
        timer += Time.deltaTime;
        Vector4 temp = GetComponent<MeshRenderer>().material.GetVector("_FinalColor");
        Debug.Log(temp);
        GetComponent<MeshRenderer>().material.SetVector("_FinalColor", new Vector4(temp.x, temp.y, temp.z, (1 - timer * timer / time / time) / 2));
        if (timer>time)
        {
            Destroy(gameObject);
        }
    }
}
