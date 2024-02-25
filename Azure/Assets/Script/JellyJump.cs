using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyJump : MonoBehaviour
{
    private Rigidbody2D rb;
    private TrailRenderer tr;
    Animator animator;
    JellyDash jellyDash;
    JellyWallJump jellyWallJump;
    JellyAnimator jellyAnimator;

    public float jumpForce;
    public float jumpForce2;
    public float jumpForce3;
    public float jumpTimeCount;
    public float jumpTime;

    public bool isGround;
    public bool isJumpping;
    public bool isJumpingMove;
    public Transform feetpos;
    public float checkRadius;
    public LayerMask Ground;

    //Coyote
    public float coyoteTime;
    public float coyoteTimeCounter;

    //緩衝
    public float jumpBufferTime;
    public float jumpBufferCounter;

    //判斷y軸
    public float nowY;
    public float preY;
    public float camY;

    public float downGravity;
    public float doubleDownGravity;
    private float preGravity;

    public bool isDown;

    //二段跳
    public bool canDoubleJump;
    public bool isDoubleJumpping;
    public bool cameraDoubleJumpFollow;
    public float doubleJumpForce;

    //漂浮
    public float floatGravity;
    public bool isFloat;
    public bool isFloated;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jellyDash = GetComponent<JellyDash>();
        jellyWallJump = GetComponent<JellyWallJump>();
        jellyAnimator = GetComponent<JellyAnimator>();
        animator = GetComponent<Animator>();

        preGravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (jellyDash.isDashing || jellyWallJump.isWallJumping || jellyWallJump.isWallSliding)
        {
            isFloated = false;
            return;
        }

        isGround = Physics2D.OverlapCircle(feetpos.position, checkRadius, Ground);

        if (isJumpping || isDoubleJumpping)
        {
            if (jumpTimeCount > 0f)
            {
                //rb.velocity += Vector2.up * jumpForce * Time.deltaTime;
                jumpTimeCount -= Time.deltaTime;

                camY = transform.position.y;

                if (isJumpping)
                {
                    canDoubleJump = true;
                }
            }
            //else
            //{
            //    isJumpping = false;
            //    //isDoubleJumpping = false;
            //}

            if(rb.velocity.y < 0)
            {
                isJumpping = false;
            }
        }

        //恢復coyoteTime
        if (isGround)
        {
            coyoteTimeCounter = coyoteTime;
            camY = 0f;
            rb.gravityScale = preGravity;
            canDoubleJump = false;
            cameraDoubleJumpFollow = false;
            isDown = false;
            isJumpingMove = false;
            isFloated = false;

            RecoverFloat();

            animator.SetBool("isGround", isGround);
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            jumpBufferCounter -= Time.deltaTime;
            animator.SetBool("isGround", isGround);
        }

        //減少jumpBufferCount

        //jumpBufferCounter -= Time.deltaTime;

        //掉落
        preY = nowY;
        nowY = transform.position.y;

        animator.SetFloat("AirSpeedY", rb.velocity.y);

        if (!isFloat)
        {
            JumpDown();
        }

        JumpCut();

        JumpingAni();

        //因為Dash Gravity要歸0
        //if (jellyDash.isDashing)
        //{
        //    rb.gravityScale = 0;
        //}
        //else if (!jellyDash.isDashing)
        //{
        //    rb.gravityScale = preGravity;
        //}
    }

    public void Jump()
    {
        jumpBufferCounter = jumpBufferTime;

        if (canDoubleJump)
        {
            rb.velocity = Vector2.up * doubleJumpForce;
            //jumpBufferCounter = 0f;
            canDoubleJump = false;
            isDoubleJumpping = true;
            cameraDoubleJumpFollow = true;
            
        }
        else if ((isGround && jumpBufferCounter > 0f) || coyoteTimeCounter > 0f)
        {
            isJumpping = true;
            jumpTimeCount = jumpTime;
            jumpBufferCounter = 0f;
            
        }

    }

    void JumpCut()
    {
        if (isJumpping)
        {
            if(jumpTimeCount > 0)
            {
                isJumpingMove = true;
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                

                if (jumpTimeCount >= 0.1f)
                {
                    Debug.Log("J3");
                    rb.AddForce(Vector2.up * jumpForce3, ForceMode2D.Impulse);
                    //rb.velocity += Vector2.up * jumpForce3 * Time.deltaTime;
                    //jumpTimeCount -= Time.deltaTime;
                }

                else if (jumpTimeCount >= 0.05f && jumpTimeCount < 0.1f)
                {
                    Debug.Log("J2");
                    rb.AddForce(Vector2.up * jumpForce2, ForceMode2D.Impulse);
                    //rb.velocity += Vector2.up * jumpForce2 * Time.deltaTime;
                    //jumpTimeCount -= Time.deltaTime;
                }

                else if (jumpTimeCount >= 0f && jumpTimeCount < 0.05f)
                {
                    Debug.Log("J1");
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    //rb.velocity += Vector2.up * jumpForce * Time.deltaTime;
                    //jumpTimeCount -= Time.deltaTime;
                }




            }
        }
    }

    public void isNotJump()
    {
        isJumpping = false;
        isDoubleJumpping = false;
    }

    //掉落判定
    void JumpDown()
    {
        if(nowY <= preY && !isFloat && !isGround && !isJumpping)
        {
            isDown = true;

            float maxGravity;
            rb.gravityScale = downGravity + (90f * Time.deltaTime);
            maxGravity = rb.gravityScale;

            if (rb.gravityScale >= 5f)
            {
                rb.gravityScale = maxGravity;
            }

            if (isDoubleJumpping)
            {
                rb.gravityScale = doubleDownGravity;
            }
        }

    }

    void JumpingAni()
    {
        if (isJumpingMove)
        {
            animator.SetBool("isJumping", isJumpping);

            //if (isJumpping)
            //{
            //    Debug.Log("Up");
            //    jellyAnimator.SendMessage("IsJump");
            //}

            //else if (!isJumpping && !isDown)
            //{
            //    Debug.Log("Mid");
            //    jellyAnimator.SendMessage("IsJumpMid");
            //}

            //else if (!isJumpping && isDown)
            //{
            //    Debug.Log("Down");
            //    jellyAnimator.SendMessage("IsJumpDown");
            //}


        }
    }

    public void UseFloat()
    {
        if (isDown && !isGround && !isFloated)
        {
            rb.velocity = new Vector2(0,2);
            rb.angularVelocity = 0;
            rb.gravityScale = floatGravity;
            isFloat = true;
            isFloated = true;
            canDoubleJump = false;
        }
    }

    public void RecoverFloat()
    {
        rb.gravityScale = preGravity;
        isFloat = false;
    }
}
