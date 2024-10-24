using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public float rotationSpeed = 90f;
    int count = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(RotateMap());
        }
    }

    private IEnumerator RotateMap()
    {
        Quaternion originalRotation = transform.rotation;

        
        float targetAngle = 90f; // 目标旋转角度
        float totalRotation = 0f; // 当前旋转角度
        float duration = targetAngle / rotationSpeed; // 计算持续时间
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float angleThisFrame = rotationSpeed * Time.deltaTime;
                transform.Rotate(-angleThisFrame, 0,0 );

            totalRotation += angleThisFrame; // 更新总旋转角度
            elapsedTime += Time.deltaTime; // 更新经过时间
            if (totalRotation >= targetAngle)
            {
                count++;
                Debug.Log("count:" + (count%6+1));//关卡数
                break;
                
            }
            yield return null;
        }
            transform.rotation= originalRotation*Quaternion.Euler(-90, 0,0);
    }
}
