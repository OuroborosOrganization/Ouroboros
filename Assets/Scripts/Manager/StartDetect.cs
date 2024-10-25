using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class StartDetect : MonoBehaviour
{
    public bool arrive =false;
    [SerializeField] private int fiction = 0;
    [SerializeField] private int Level;
    public void Func()
    {
        if(fiction == 0)
        {
            GameObject g = Instantiate(Resources.Load<GameObject>("Pre/BlackHead"), transform.position, transform.rotation, transform.parent) as GameObject;
            g.transform.localScale = transform.localScale;
            GameManager.Instance.BlackSnake = g.GetComponent<CharacterMove>();
        }
        else
        {
            GameObject g = Instantiate(Resources.Load<GameObject>("Pre/WhiteHead"), transform.position, transform.rotation, transform.parent) as GameObject;
            g.transform.localScale = transform.localScale;
            GameManager.Instance.WhiteSnake = g.GetComponent<CharacterMove>();
        }
    }
    private void Awake()
    {
        if(fiction == 0)
        {
            GameManager.Instance.BlackStarts[Level - 1] = this;
        }
        else
        {
            GameManager.Instance.WhiteStarts[Level - 1] = this;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        CharacterMove o = other.GetComponent<CharacterMove>();
        if (o == null) { return; }
        if (o.BodyColor == fiction) { return; }
        BoxCollider t = other.gameObject.GetComponent<BoxCollider>();
        t.center = Vector3.zero;
        t.size = Vector3.one*0.99f;
    }
    private void OnTriggerStay(Collider other)
    {
        
        if ((transform.position - other.transform.position).magnitude<=0.02&&!arrive)
        {
            CharacterMove o = other.GetComponent<CharacterMove>();
            if (o == null) { return; }
            if (o.BodyColor == fiction) { return; }
            arrive = true;
            GameManager.Instance.ChangeSide(o.BodyColor);
            o.Arrive();
        }
       
    }
}
