using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    JellyJump jellyJump;
    JellyDash jellyDash;
    JellyWallJump jellyWallJump;
    JellyAnimator jellyAnimator;

    [Header("Run")]
    private float preSpeed;
    public float speed;
    public float floatSpeed;
    public float jumpSpeed;

    [Header("FlipPlayer")]
    public float horizontal;
    public bool isFacingRight;

    //public FollowPlayer followPlayer;
    //[SerializeField] GameObject playerPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        jellyJump = GetComponent<JellyJump>();
        jellyDash = GetComponent<JellyDash>();
        jellyWallJump = GetComponent<JellyWallJump>();
        jellyAnimator = GetComponent<JellyAnimator>();

        //followPlayer = playerPos.GetComponent<FollowPlayer>();

        preSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (jellyDash.isDashing || jellyWallJump.isWallJumping)
        {
            return;
        }

        Run();

        if (jellyJump.isFloat)
        {
            speed = floatSpeed;
        }
        else if (jellyJump.isJumpingMove)
        {
            speed = jumpSpeed;
        }
        else
        {
            speed = preSpeed;
        }

        //rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        //角色翻轉

        if (!jellyWallJump.isWallSliding)
        {
            if (horizontal > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                isFacingRight = true;

                //followPlayer.CallTrun();
            }
            else if (horizontal < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                isFacingRight = false;

                //followPlayer.CallTrun();
            }
        }
    }

    void Run()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        bool haveSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        if (haveSpeed)
        {
            jellyAnimator.SendMessage("IsRun");
        }

        else
        {
            jellyAnimator.SendMessage("StopRun");
        }
    }
}
