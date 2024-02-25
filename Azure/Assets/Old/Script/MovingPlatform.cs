using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float speed = 0.2f; // 平台移動速度
    [SerializeField] private float distance = 11f; // 平台移動距離
    private Vector3 startPosition; // 平台起始位置
    private Vector3 targetPosition; // 平台目標位置

    private void Start()
    {
        startPosition = transform.position; // 記錄平台起始位置
        targetPosition = transform.position + new Vector3(0f, distance, 0f); // 計算平台目標位置
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(startPosition, targetPosition, Mathf.PingPong(Time.time * speed, 1f)); // 使用Lerp函數平滑移動平台
    }
}
