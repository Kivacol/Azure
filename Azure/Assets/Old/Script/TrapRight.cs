using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRight : MonoBehaviour
{
    public float speed = 5f;   // 移動速度
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
        transform.position = new Vector3(startPosX - moveAmount, transform.position.y, transform.position.z);
    }
}
