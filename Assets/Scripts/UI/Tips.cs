using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tips : MonoBehaviour
{
    private Image W;
    private Image S;
    private Image A;
    private Image D;
    private Coroutine FCor = null;
    private void Start()
    {
        W = transform.GetChild(0).GetComponent<Image>();
        S = transform.GetChild(1).GetComponent<Image>();
        A = transform.GetChild(2).GetComponent<Image>();
        D = transform.GetChild(3).GetComponent<Image>();
        GameManager.Instance.Move += HighLighted;
        GameManager.Instance.TeachFlash += ObjectFlash;
        if(GameManager.Instance.CurLevel == 1)
        {
            ObjectFlash(GameManager.Instance.TeachSteps[0]);
        }
    }

    public void ObjectOpen(int index)
    {
        switch (index)
        {
            case 0:
                W.color = new Color(1,0,0,0.3f);
                break;
            case 1:
                S.color = new Color(1, 0, 0, 0.3f);
                break;
            case 2:
                A.color = new Color(1, 0, 0, 0.3f);
                break;
            case 3:
                D.color = new Color(1, 0, 0, 0.3f);
                break;
        }
    }
    public void ObjectClose(int index)
    {
        switch (index)
        {
            case 0:
                W.color = new Color(1, 0, 0, 0);
                break;
            case 1:
                S.color = new Color(1, 0, 0, 0);
                break;
            case 2:
                A.color = new Color(1, 0, 0, 0);
                break;
            case 3:
                D.color = new Color(1, 0, 0, 0);
                break;
        }
    }
    public void ObjectFlash(Direction index)
    {
        switch (index)
        {
            case Direction.Up:
                StopFlash();
                FCor = StartCoroutine(Flash(W));
                break;
            case Direction.Down:
                StopFlash();
                FCor = StartCoroutine(Flash(S));
                break;
            case Direction.Left:
                StopFlash();
                FCor = StartCoroutine(Flash(A));
                break;
            case Direction.Right:
                StopFlash();
                FCor = StartCoroutine(Flash(D));
                break;
            default:
                StopFlash();
                return;
        }
    }

    IEnumerator Flash(Image target)
    {
        float time = 10f;
        float timer = 0f;
        int flag = 1;
        target.color = new Color(1, 0, 0, 0);
        while(timer<time)
        {
            target.color += new Color(0, 0, 0, 0.02f)*flag*0.5f;
            if(target.color.a >0.3f||target.color.a <=0)
            {
                flag *= -1;
            }
            timer += 0.02f;
            yield return new WaitForSeconds(0.02f);
        }
        target.color = new Color(1, 0, 0, 0);
        StopFlash();
        yield break;
    }

    public void StopFlash()
    {
        if (FCor != null) { StopCoroutine(FCor); FCor = null; }
    }

    public void HighLighted(CharacterMove character)
    {
        if(character == null)
        {
            ObjectClose(0);
            ObjectClose(1);
            ObjectClose(2);
            ObjectClose(3);
            return;
        }
        Direction dir = character.CurDirection;
        switch(dir)
        {
            case Direction.Up:
                ObjectOpen(0);
                return;
            case Direction.Down:
                ObjectOpen(1);
                return;
            case Direction.Left:
                ObjectOpen(2);
                return;
            case Direction.Right:
                ObjectOpen(3);
                return;
            default:
                ObjectClose(0);
                ObjectClose(1);
                ObjectClose(2);
                ObjectClose(3);
                return;
        }
    }
    private void OnDestroy()
    {
        GameManager.Instance.Move -= HighLighted;
    }
}
