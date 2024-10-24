using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class StartDetect : MonoBehaviour
{
    [SerializeField] private int fiction = 0;

    private void OnTriggerEnter(Collider other)
    {
        CharacterMove o = other.GetComponent<CharacterMove>();
        if (o == null) { return; }
        if(o.BodyColor == fiction) {  return; }
        o.Arrive();
    }
}
