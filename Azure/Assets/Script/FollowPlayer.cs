using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject Player;
    public JellyJump jellyJump;
    public JellyMove jellyMove;
    public JellyWallJump jellyWallJump;


    [SerializeField] private Vector3 playerPos;

    //Rotation Player
    //[SerializeField] private Transform playerTransform;
    ////[SerializeField] private JellyMove jellyMove;
    //[SerializeField] private float flipYRotation;
    //[SerializeField] private bool isFacingRight;

    private Coroutine trunCoroutine;

    void Awake()
    {
        jellyJump = Player.GetComponent<JellyJump>();
        jellyMove = Player.GetComponent<JellyMove>();
        jellyWallJump = Player.GetComponent<JellyWallJump>();

        //jellyMove = playerTransform.gameObject.GetComponent<JellyMove>();

        //isFacingRight = jellyMove.isFacingRight;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = playerTransform.position;

        if (jellyMove.isFacingRight)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        else if(!jellyMove.isFacingRight)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    private void FixedUpdate()
    {
        //跟蹤Player
        Vector3 playerPos = transform.position;

        //跳躍與掉落追蹤
        if ((jellyJump.cameraDoubleJumpFollow && jellyJump.camY > 0) || jellyWallJump.isWallSliding)
        {
            //Debug.Log("1");
            transform.position = Player.transform.position;
        }
        else if (jellyJump.camY > 0)
        {
            //Debug.Log("2");
            playerPos.x = Player.transform.position.x;
            transform.position = playerPos;
        }
        else if (jellyJump.camY == 0)
        {
            //Debug.Log("3");
            transform.position = Player.transform.position;
        }

        //漂浮中追蹤
        if (jellyJump.isFloat)
        {
            transform.position = Player.transform.position;
        }
      
    }

    //public void CallTrun()
    //{
    //    trunCoroutine = StartCoroutine(FlipYLerp());
    //}

    //private IEnumerator FlipYLerp()
    //{
    //    float startRotation = transform.localEulerAngles.y;
    //    float endRotationAmount = DetermineEndRotation();
    //    float yRotation = 0f;

    //    float elapsedTime = 0f;
    //    while (elapsedTime < flipYRotation)
    //    {
    //        elapsedTime += Time.deltaTime;

    //        yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / flipYRotation));
    //        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

    //        yield return null;
    //    }

    //}

    //private float DetermineEndRotation()
    //{
    //    isFacingRight = !isFacingRight;

    //    if (isFacingRight)
    //    {
    //        return 180f;
    //    }
    //    else
    //    {
    //        return 0;
    //    }
    //}
}
