using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class StartDetect : MonoBehaviour
{
    private bool arrive =false;
    [SerializeField] private int fiction = 0;
    private void Start()
    {
        if(fiction == 0)
        {
            GameObject g = Instantiate(Resources.Load<GameObject>("Pre/BlackHead"), transform.position, Quaternion.identity) as GameObject;
            g.transform.localScale = transform.localScale;

        }
        else
        {
            GameObject g = Instantiate(Resources.Load<GameObject>("Pre/WhiteHead"), transform.position, Quaternion.identity) as GameObject;
            g.transform.localScale = transform.localScale;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if ((transform.position - other.transform.position).magnitude<=0.05&&!arrive)
        {
            CharacterMove o = other.GetComponent<CharacterMove>();
            if (o == null) { return; }
            if (o.BodyColor == fiction) { return; }
            arrive = true;
            o.Arrive();
        }
       
    }
}
