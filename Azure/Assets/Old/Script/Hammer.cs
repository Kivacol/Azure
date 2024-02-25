using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public float downTime;
    public float waitTime;
    public float upTime;

    public float orgDownTime;
    public float orgWaitTime;
    public float orgUpTime;

    public float speed = 5f;   // 移動速度
    public float moveRange = 2f;  // 移動範圍

    private float startPosY;

    public bool isdown;
    // Start is called before the first frame update
    void Start()
    {
        orgDownTime = downTime;
        orgWaitTime = waitTime;

        startPosY = transform.position.y;   // 記錄起始位置
    }

    // Update is called once per frame
    void Update()
    {
        if (downTime > 0)
        {
            waitTime = orgWaitTime;
            downTime -= Time.deltaTime;
        }

        else if (downTime <= 0)
        {
            //// 計算移動量
            float moveAmount = Mathf.Sin(Time.time * speed) * moveRange;

            //// 更新物件位置
            transform.position = new Vector3(transform.position.x, startPosY - moveAmount, transform.position.z);
            isdown = true;

        }

        if (isdown == true)
        {
            waitTime -= Time.deltaTime;
            downTime = orgDownTime;
        }

        if (waitTime <= 0 || downTime > 0)
        {
            //transform.position = new Vector3(transform.position.x, startPosY + 5f, transform.position.z);
            float moveAmount = Mathf.Sin(Time.time * speed) * moveRange;
            transform.position = new Vector3(transform.position.x, startPosY + moveAmount, transform.position.z);
            isdown = false;
            //waitTime = orgWaitTime;
        }
    }

    //public float crushingDuration = 2f; // 壓擠持續時間
    //public float trapResetDelay = 5f; // 陷阱重置延遲時間

    //private bool isTrapActive = false; // 陷阱是否處於激活狀態

    //private Vector3 initialPosition; // 陷阱的初始位置

    //private void Start()
    //{
    //    initialPosition = transform.position;
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player")) // 根據需求，設定碰撞物體的標籤
    //    {
    //        if (!isTrapActive)
    //        {
    //            isTrapActive = true;
    //            StartCoroutine(ActivateTrap());
    //        }
    //    }
    //}

    //private IEnumerator ActivateTrap()
    //{
    //    // 陷阱壓擠的動作
    //    // 假設使用 Transform.Translate 來實現陷阱壓擠的效果
    //    while (transform.position.y > 0)
    //    {
    //        transform.Translate(Vector3.down * Time.deltaTime);
    //        yield return null;
    //    }

    //    yield return new WaitForSeconds(crushingDuration);

    //    // 陷阱重置
    //    ResetTrap();
    //}

    //private void ResetTrap()
    //{
    //    transform.position = initialPosition;
    //    isTrapActive = false;

    //    // 延遲重置陷阱，以便在一段時間後重新激活陷阱
    //    StartCoroutine(DelayedTrapReset());
    //}

    //private IEnumerator DelayedTrapReset()
    //{
    //    yield return new WaitForSeconds(trapResetDelay);
    //    isTrapActive = true;
    //}
}
