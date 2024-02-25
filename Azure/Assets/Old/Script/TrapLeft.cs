using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapLeft : MonoBehaviour
{

    /*public float speed = 5f;   // 移動速度
    public float moveRange = 5f;  // 移動範圍

    private float startPosX;

    void Start()
    {
        startPosX = transform.position.x;   // 記錄起始位置
    }

    void Update()
    {
        // 計算移動量
        float moveAmount = Mathf.Sin(Time.time * speed) * moveRange;

        // 更新物件位置
        transform.position = new Vector3(startPosX + moveAmount, transform.position.y, transform.position.z);
    }*/
    public float extendDuration = 2f; // 伸出去的時間
    public float retractDuration = 1f; // 縮回來的時間
    public float interval = 1f; // 伸縮的時間間隔

    private Vector3 originalPosition;
    private Vector3 extendedPosition;
    private bool isExtended = false;
    private float timeElapsed = 0f;

    void Start()
    {
        originalPosition = transform.position;
        extendedPosition = originalPosition + transform.forward * 5f; // 假設要往前伸15個單位
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= interval)
        {
            timeElapsed = 0f;
            if (!isExtended)
            {
                StartCoroutine(Extend());
            }
            else
            {
                StartCoroutine(Retract());
            }
        }
    }

    IEnumerator Extend()
    {
        float timeElapsed = 0f;
        while (timeElapsed < extendDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, extendedPosition, timeElapsed / extendDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = extendedPosition;
        isExtended = true;
    }

    IEnumerator Retract()
    {
        float timeElapsed = 0f;
        while (timeElapsed < retractDuration)
        {
            transform.position = Vector3.Lerp(extendedPosition, originalPosition, timeElapsed / retractDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;
        isExtended = false;
    }
}