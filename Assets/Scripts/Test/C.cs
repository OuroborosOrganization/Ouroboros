using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C : MonoBehaviour
{
    [SerializeField] private float Distance = 10.0f;
    [SerializeField] private float OriAngel = 60f;
    [SerializeField] private float Angel = 60;
    [SerializeField] private Vector2 Point = Vector2.zero;
    [SerializeField] private float speed = 1f;
    public bool ist = false;
    public bool back = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, Distance * Mathf.Sin(Angel / 180 * Mathf.PI), -Distance * Mathf.Cos(Angel / 180 * Mathf.PI));
        transform.localEulerAngles = new Vector3(Angel, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(ist)
        {
            Angel += Time.deltaTime*speed;
            transform.position = new Vector3(0, Distance * Mathf.Sin(Angel/180*Mathf.PI), -Distance * Mathf.Cos(Angel / 180 * Mathf.PI));
            transform.localEulerAngles = new Vector3(Angel, 0, 0);
            if(Angel >89)
            {
                ist = false;
            }
        }
        if (back)
        {
            Angel = OriAngel;
            transform.position = new Vector3(0, Distance * Mathf.Sin(Angel / 180 * Mathf.PI), -Distance * Mathf.Cos(Angel / 180 * Mathf.PI));
            transform.localEulerAngles = new Vector3(Angel, 0, 0);
            back = false;
        }
    }
}
