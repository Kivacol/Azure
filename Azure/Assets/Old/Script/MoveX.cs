using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MoveX : MonoBehaviour
{
    //CapsuleCollider capsuleCollider;

    //public GameObject Player;
    //[Header("Son")]
    //public

    [Header("Move")]
    private Vector3 playerVel;
    private float horizontal;
    public float speed;

    private float perSpeed;
    public bool isWalk;
    [SerializeField] private bool isRun = false;
    public bool stopRuning = false;

    //public bool isFlip;

    Vector3 moveAmount;
    [Header("Jump")]
    //Jump Cut
    [SerializeField] private float jumpingSpeed = 10f;
    [SerializeField] private float jumpTimeCounter;
    //[SerializeField] private float jumpFastTimeCounter;
    public float jumpTime;
    public float jumpFastTime;

    public bool isFacingRight = true;
    public bool isGround = true;
    public bool isDoubleJump = false;
    public bool isJump = false;
    public bool slowJump = false;
    public bool fastJump = false;

    public float startY;
    public float downY;

    //滯空
    public float coyoteTime = 0.2f;
    public float coyoteTimeCounter;

    //緩衝
    public float jumpBufferTime = 0.2f;
    public float jumpBufferCounter;

    //internal void Initialize(Grapple grapple, Transform shootTransform)
    //{
    //    throw new NotImplementedException();
    //}

    [Header("Dash")]
    public bool canDash = true;
    public bool isDashing;
    [SerializeField]public float dashSpeed = 800f;
    public float dashingTime = 0.8f;
    float dashTimer;
    public float dashingCooldown = 0.5f;
    //public bool getDash = false;

    [SerializeField] private Animator m_animation;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Rigidbody rb;


    [Header("Grevity")]
    //重力
    public float gravityScale = 3f;

    public static float globalGravity = -9.81f;

    [Header("InputAct")]
    //控制器
    [SerializeField] private Vector2 moveInputValue;
    private PlayerInputActions controls;

    //private Vector2 arrrowInputValue;

    //慣性
    //[Header("StopMove")]
    //public float StopTimer;
    //public bool isRuning = false;

    //public float stopTime = 0.3f;

    [Header("Attack")]
    //攻擊
    public float attckCount = 0;
    public float timeSinceAttack = 0.0f;
    public bool isAttack = false;
    public float attackSpeed;

    //繩索
    [Header("Grappling")]
    [SerializeField] float pullSpeed = 2f;
    [SerializeField] float stopDistance = 4f;
    [SerializeField] GameObject hookPrefab;
    public Transform shootTransform;

    Hook hook;
    HookRange hookRange;
    bool pulling = false;
    //Rigidbody rigid;
    public bool isGrapple;

    public GameObject arrow;

    public bool isTarget;

    //public float hookForce = 25f;

    //Grapple grapple;

    //LineRenderer lr;

    //SpringJoint joint;
    ////public SpringJoint Sj;

    //public Transform shotTip, camera, player;
    //private float maxDistance = 20f;


    ////public float maxGrappleDistance;
    ////public float grappleDelayTime;

    //private Vector3 grapplePoint;
    //public LayerMask whatIsGrappleable;

    //Vector2 pointMove;

    //public float grapplingCd;
    //public float grapplingCdTimer;

    //public bool isGrappling;

    //爬坡
    [Header("Climb")]
    public Transform groundCheck;
    public float rayLength = 1f;
    public float maxSlopeAngle = 30f;
    public float slopeAngle;

    public bool isClimb;

    Vector3 hitNormal;

    [Header("Down")]
    //private float lastY;
    [SerializeField] private Vector3 lastPos;
    public bool isUp;
    public bool isDown;

    //[Header("Arrow")]
    //private Camera mainCam;
    //private Vector3 arrowPos;

    //public Transform target;    // 玩家物件
    //public float Rspeed = 100f; // 旋轉速度
    //public Vector3 axis = Vector3.down; // 旋轉軸向

    [Header("Pull")]
    public bool isPull = false;
    public bool isClose = false;
    public bool CanClose = false;
    public bool releasePull = false;
    public Vector3 dis;

    [Header("Swinging")]

    //public Rigidbody box;
    //public GameObject box1;

    private float maxSwingDistance = 25f;
    private Vector3 swingPoint;
    //private HingeJoint joint;
    public SpringJoint joint;
    public bool isSwing;

    [Header("Timer")]
    public TimeManager timeManager;
    public bool timeFlag;

    void Awake()
    {
        controls = new PlayerInputActions();

        controls.GamePlay.Move.performed += ctx => { moveInputValue = ctx.ReadValue<Vector2>(); isWalk = true; };
        controls.GamePlay.Move.canceled += ctx => {moveInputValue = Vector2.zero; isWalk = false; };
        //controls.GamePlay.Move.canceled += ctx => StopTimer = 10f; ;

        controls.GamePlay.Run.started += ctx => isRun = true;
        controls.GamePlay.Run.canceled += ctx => isRun = false;

        controls.GamePlay.Jump.started += ctx => Jump();
        controls.GamePlay.Jump.canceled += ctx => isJump = false;

        controls.GamePlay.Attack.started += ctx => AttackCombo();

        controls.GamePlay.Dash.started += ctx => StartCoroutine(Dash());

        controls.GamePlay.Grapple.started += ctx => isGrapple = false;
        controls.GamePlay.Grapple.canceled += ctx => isGrapple = true;
        controls.GamePlay.Grapple.started += ctx => { arrow.SetActive(true); isTarget = true; };
        controls.GamePlay.Grapple.canceled += ctx => { arrow.SetActive(false); isTarget = false; };

        //controls.GamePlay.Pull.started += ctx => isPull = true;
        //controls.GamePlay.Pull.canceled += ctx => { isPull = false; releasePull = true; };

        controls.GamePlay.Colse.started += ctx => { isClose = true; m_animation.SetTrigger("T_isClose");
        };

        //controls.GamePlay.Arrow.performed += ctx => arrrowInputValue = ctx.ReadValue<Vector2>();
        //controls.GamePlay.Arrow.canceled += ctx => arrrowInputValue = Vector2.zero;
    }

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;


        controls.GamePlay.Enable();
    }

    void OnDisable()
    {
        controls.GamePlay.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        //capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<TrailRenderer>();
        //m_animation = GetComponent<Animator>();
        perSpeed = speed;
        lastPos = transform.position;

        m_animation = transform.GetComponentInChildren<Animator>();
        tr = transform.GetComponentInChildren<TrailRenderer>();
        //tr.emitting = true;

        //mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

       

    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = Quaternion.identity;

        //if (!isGrapple)
        //{
        //    isPull = false;
        //    isSwing = false;
        //    releasePull = true;
        //    isGrapple = false;
        //}
        if (isJump)
        {
            if (isPull || isSwing || isGrapple)
            {
                Debug.Log("解除");
                isPull = false;
                isSwing = false;
                releasePull = true;
                isGrapple = false;
            }
        }

        if(isPull || isGrapple || isSwing)
        {
            isGround = true;
            isJump = false;
        }

        if (!canDash)
        {
            Invoke("recoverDash", 1f);
        }

        if (isTarget && !isJump)
        {
            m_animation.SetBool("isTarget", true);
            speed = 0;
        }

        else if(isTarget && isJump)
        {
            m_animation.SetBool("isTarget", true);
        }

        else
        {
            m_animation.SetBool("isTarget", false);
            speed = perSpeed;
        }

        if (hook == null)
        {
            m_animation.SetBool("isGrapple", false);
        }


        //判斷現在Y軸座標
        if (isGround)
        {
            //Debug.Log("地上");
            m_animation.SetBool("isJumping", false);
            isDoubleJump = false;
            startY = 0;
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        jumpBufferCounter -= Time.deltaTime;

        downY = transform.position.y;

        JumpDown();

        //SlowMotion
        if(!isGround && isTarget)
        {
            //Debug.Log("慢動作");
            timeFlag = true;
            timeManager.DoSlowMotion(); 
        }

        if (!isTarget || isGround || isGrapple)
        {
            //Debug.Log("恢復");
            timeFlag = false;
            timeManager.ReCoverNormol();
        }


        //擺盪速度
        if (isSwing)
        {
            //isGrapple = true;
            m_animation.SetBool("isDon", true);
            if (isWalk)
            {
                speed = 5;
                if (isRun)
                {
                    speed = 8;
                }
            }
            
        }
        else
        {
            //isGrapple = false;
            m_animation.SetBool("isDon", false);
            m_animation.SetTrigger("T_isDonLow");
            //speed = perSpeed;
        }

        //tr.emitting = true;
        //if (OnSlope())
        //{
        //    playerVel = Vector3.ProjectOnPlane(playerVel, hitNormal).normalized;

        //    //m_animation.SetBool("isJumping", false);
        //    //isGround = true;
        //    //isDoubleJump = false;

        //    if (slopeAngle > 0)
        //    {
        //        //rb.drag = 20;

        //        //rb.useGravity = true;
        //        //rb.velocity = new Vector3(0, rb.velocity.y, 0);

        //        rb.AddForce(Vector3.down * 200f, ForceMode.Force);
        //    }
        //}

        //trun gravity off while on slope
        //rb.useGravity = !OnSlope();

        //Vector3 currentPos = transform.position;
        //float heightDiff = currentPos.y - lastPos.y;
        //print(heightDiff);

        //檢查上下坡
        //if (slopeAngle > 0)
        //{
        //    isClimb = true;
        //}
        //else
        //{
        //    isClimb = false;
        //}

        //if (slopeAngle> 0 && !isJump && !isDashing && !isAttack && !isGrapple)
        //{

        //    if (heightDiff > 0 && isClimb)
        //    {
        //        isUp = true;
        //        isDown = false;
        //        if (isRun)
        //            speed = 8;
        //        else
        //            speed = 6f;

        //        Debug.Log("角色正在上坡");
        //    }
        //    else if (heightDiff < 0 && isClimb)
        //    {
        //        isDown = true;
        //        isUp = false;
        //        if (isRun)
        //            speed = 15;
        //        else
        //            speed = 9;
        //        Debug.Log("角色正在下坡");
        //    }
        //    else
        //    {
        //        isDown = false;
        //        isUp = false;
        //        isClimb = false;
        //        if (isRun)
        //            speed = 12;
        //        else
        //            speed = 6;
        //        Debug.Log("平地");
        //    }
        //}

        //// 更新lastPos
        //lastPos = currentPos;

        //if (!isRun && !isJump && !isDashing && !isAttack && !isGrapple && !isDown)
        //{

        //    if (slopeAngle > 7f && isClimb)
        //    {
        //        //Debug.Log("上坡");
        //        speed = 4f;
        //    }
        //    else
        //    {
        //        isClimb = false;
        //        speed = 6;
        //    }
        //}

        if (isDashing)
        {
            return;
        }

        //慣性
        //if(isRun)
        //    StopTimer = 0;
        //else
        //StopTimer--;

        //StopMove();

        if (isAttack)
        {
            rb.velocity = new Vector2(transform.localScale.x * attackSpeed, rb.velocity.y);
        }

        if (stopRuning)
        {
            return;
        }
        else if (!stopRuning)
        {
            Run();
        }

        RunFast();

        JumpCut();

        if (isGround && !isDashing && !isRun && !isAttack && !isClimb && !isSwing && !isTarget)
        {
            RecoverMove();
            RecoverG();
        }

        if (isGround == true)//&& !Input.GetButton("Jump"))
        {
            slowJump = false;
            fastJump = false;
            //canDash = true;
        }

        //攻擊時間
        timeSinceAttack += Time.deltaTime;

        if (timeSinceAttack > 1f)
            attckCount = 0;

        Flip();


        //Hook
        if (CanClose)
        {
            m_animation.SetBool("isHold", true);
        }
        else
        {
            m_animation.SetBool("isHold", false);
        }

        if (hook != null)
        {
            dis = hook.transform.position - this.transform.position;
            //m_animation.SetBool("isHold", true);

            if (dis.magnitude > 13f)
            {
                DestroyHook();
                CanClose = false;
                isGrapple = false;
                isPull = false;
                isClose = false;
                releasePull = false;
            }
        }

        if (hook == null && isGrapple)
        {
            StopAllCoroutines();
            pulling = false;
            m_animation.SetBool("isHold", false);
            m_animation.SetTrigger("T_isShoot");
            m_animation.SetBool("isGrapple", true);
            hook = Instantiate(hookPrefab, shootTransform.position, Quaternion.identity).GetComponent<Hook>();
            hook.Initialize(this, shootTransform, isFacingRight);
            StartCoroutine(DestroyHookAfterLifetime());
        }
        else if (hook != null && !isGrapple && !isPull)
        {
            m_animation.SetBool("isGrapple", false);
            m_animation.SetBool("isHold", false);
            DestroyHook();
            releasePull = false;
            isClose = false;
            CanClose = false;
        }

        if (!pulling || hook == null) return;

        //if (Vector3.Distance(transform.position, hook.transform.position) <= stopDistance)
        //{
        //    DestroyHook();
        //}

        if (isPull)
        {
            m_animation.SetTrigger("T_isPull");
            rb.AddForce((hook.transform.position - transform.position).normalized * pullSpeed, ForceMode.VelocityChange);
        }

        if (releasePull && hook!=null)
        {

            DestroyHook();
            m_animation.SetBool("isGrapple", false);
            m_animation.SetBool("isHold", false);
            releasePull = false;
            isPull = false;
            CanClose = false;
        }

     
        

        

    }
 
    private void FixedUpdate()
    {


        //RunStop(movement);
        //rb.AddForce(playerVel * inertiaSpeed * Time.deltaTime , ForceMode.Acceleration);
        //rb.MovePosition(rb.position + moveAmount);

        if (isDashing)
        {
            return;
        }

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        Vector2 gravity = globalGravity * gravityScale * Vector2.up;
        rb.AddForce(gravity, ForceMode.Acceleration);

    }



    void Run()
    {
     

        horizontal = Input.GetAxisRaw("Horizontal");

        playerVel = new Vector3(horizontal * speed, rb.velocity.y);
        rb.velocity = playerVel;
        bool haveSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        
        
        //currentInputVector = Vector2.SmoothDamp(currentInputVector, target, ref smoothInputVelocity, smoothInputSpeed);

        m_animation.SetFloat("speed", Mathf.Abs(horizontal));
        m_animation.SetBool("isRun", haveSpeed);

        //慣性
        //movement = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));


        //Vector3 playerVel = new Vector3(horizontal * speed, rb.velocity.y);
        //rb.velocity = playerVel;
        //bool haveSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        //m_animation.SetBool("isRun", haveSpeed);
    }



    void RunFast()
    {
        //加速
        if (isRun == true)
        {
            
            if (!isClimb && !isSwing)
            {
                speed = 6f;
            }
            
            m_animation.SetBool("isFast", true);
            //tr.emitting = true;
            
        }
        else
        {
            if (!isClimb)
            {
                m_animation.SetBool("isFast", false);
                //tr.emitting = false;
                //speed = perSpeed;
            }
            else
            {
                m_animation.SetBool("isFast", false);
                //tr.emitting = false;
            }

        }
    }

    void Flip()
    {
        

        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;

            //m_animation.SetBool("isFlip", true);
            //isFlip = true;

            //localScale.z *= -1f;
            //transform.localScale = localScale;

            //if (isGround && speed == 6f)
            //{
            //    stopRuning = true;
            //    //m_animation.SetTrigger("T_isTurn");
            //    //StartCoroutine(FlipDrag());
            //}
            if (isGround && speed == 6f)
            {
                stopRuning = true;
                m_animation.SetTrigger("T_isTurn");
                StartCoroutine(FlipDragFast());
            }
            else
            {
                //Vector3 localScale = transform.localScale;
                //m_animation.SetTrigger("T_isTurn");
                localScale.z *= -1f;
                transform.localScale = localScale;
                //Invoke("RecoverFlip",0.3f);
            }
            //else if( isGround|| !isGround || isDashing　|| speed < 12)
            //{
            //    localScale.z *= -1f;
            //    transform.localScale = localScale;
            //}
            //rb.drag = 20;
            //Invoke("RecoverDrag", 0.5f);
            //yield return new WaitForSeconds(0.2f);

        }

    }

    //void RecoverFlip()
    //{
    //    isFlip = false;
    //    //m_animation.SetBool("isFlip", false);
    //}

    //IEnumerator FlipDrag()
    //{
    //    //m_animation.SetTrigger("T_isTurn");
    //    rb.drag = 25;
    //    //speed = 0;
    //    yield return new WaitForSeconds(0.1f);
    //    rb.drag = 20;
    //    yield return new WaitForSeconds(0.08f);
    //    rb.drag = 10;
    //    yield return new WaitForSeconds(0.05f);
    //    Vector3 localScale = transform.localScale;

    //    localScale.z *= -1f;
    //    transform.localScale = localScale;
    //    rb.drag = 0;
    //    speed = perSpeed;
    //    stopRuning = false;
    //}

    IEnumerator FlipDragFast()
    {
        //m_animation.SetTrigger("T_isTurn");
        rb.drag = 35;
        speed = 0;
        yield return new WaitForSeconds(0.2f);
        rb.drag = 30;
        yield return new WaitForSeconds(0.15f);
        rb.drag = 20;
        yield return new WaitForSeconds(0.1f);
        Vector3 localScale = transform.localScale;

        localScale.z *= -1f;
        transform.localScale = localScale;
        rb.drag = 0;
        speed = 6f;
        stopRuning = false;
        //isFlip = false;
        //m_animation.SetBool("isFlip", false);
    }

    //慣性
    //void StopMove()
    //{
    //    Debug.Log("慣性");
    //    //m_animation.SetTrigger("T_isFollow");
    //    if (StopTimer == 1 && !isRun)
    //        m_animation.SetTrigger("T_isFollow");
    //}


    void Jump()
    {
        Debug.Log("jump");
        jumpBufferCounter = jumpBufferTime;
        //isPull = false;
        //isSwing = false;
        //releasePull = true;
        StopSwing();
        //isGrapple = false;

        if (isGround == true && !isJump)//&& !Input.GetButton("Jump"))
        {
            slowJump = false;
            fastJump = false;
            isDoubleJump = false;
            //if(!isClimb)
            //speed = perSpeed;
            //m_animation.SetBool("notOnGround", false);
        }

        //if (Input.GetButtonDown("Jump") )
        {
            if (isGround == true && jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
            {
                if (speed == 3f || speed == 5f)
                {
                    m_animation.SetBool("isJumping", true);
                    jumpBufferCounter = 0f;
                    //tr.emitting = true;
                    //rb.velocity = Vector2.up * jumpingSpeed;
                    speed = 4f;
                    isGround = false;
                    isDoubleJump = true;
                    jumpTimeCounter = jumpTime;
                    isJump = true;
                    slowJump = true;
                    gravityScale = 2.3f;
                    //m_animation.SetTrigger("T_isDown");
                    //Invoke("RecoverMove", 1f);
                    //Debug.Log("Jump");
                    return;
                }

                else if (speed == 6f || speed == 8f)
                {
                    m_animation.SetBool("isJumping", true);
                    jumpBufferCounter = 0f;
                    //tr.emitting = true;
                    //rb.velocity = Vector2.up * jumpingSpeed;
                    speed = 7f;
                    isGround = false;
                    isDoubleJump = true;
                    jumpTimeCounter = jumpFastTime;
                    isJump = true;
                    fastJump = true;
                    gravityScale = 3.7f;
                    //m_animation.SetTrigger("T_isDown");
                    //Invoke("RecoverMove", 1f);
                    //Debug.Log("Jump");
                    return;
                }
            }
        }

        //if (Input.GetButtonDown("Jump") && isDoubleJump == true)
        //二段跳

        //if (isDoubleJump)
        //{
        //    if (slowJump) {
        //        m_animation.SetTrigger("T_isDuobleJump");
        //        //Debug.Log("DoubleJump");
        //        speed = 3.5f;
        //        gravityScale = 6f;
        //        //rb.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
        //        rb.velocity = Vector2.up * jumpingSpeed * 1.9f;
        //        //m_animation.SetTrigger("T_isDown");
        //        isDoubleJump = false;
        //        //Invoke("RecoverMove", 0.5f);
        //        //isDoubleJump = false;
        //    }

        //    if (fastJump)
        //    {
        //        m_animation.SetTrigger("T_isDuobleJump");

        //        speed = 2.5f;
        //        gravityScale = 7f;

        //        rb.velocity = Vector2.up * jumpingSpeed * 2.3f;

        //        isDoubleJump = false;

        //    }
        //}



    }

    void JumpCut()
    {
        if (isJump && slowJump)
        {

            if (jumpTimeCounter > 0)
            {
                //gravityScale = 4f;
                if (jumpTimeCounter >= 0f && jumpTimeCounter < 0.2f)
                {
                    Debug.Log("J1");
                    //m_animation.SetTrigger("T_isJump");
                    rb.velocity = Vector2.up * jumpingSpeed * 1.1f;
                    startY = transform.position.y;
                    jumpTimeCounter -= Time.deltaTime;
                }

                else if (jumpTimeCounter >= 0.2f && jumpTimeCounter < 0.3f)
                {
                    Debug.Log("J2");
                    //m_animation.SetTrigger("T_isJump");
                    rb.velocity = Vector2.up * jumpingSpeed;
                    startY = transform.position.y;
                    jumpTimeCounter -= Time.deltaTime;
                }

                else if (jumpTimeCounter >= 0.3f)
                {
                    Debug.Log("J3");
                    //m_animation.SetTrigger("T_isJump");
                    rb.velocity = Vector2.up * jumpingSpeed * 0.9f;
                    startY = transform.position.y;
                    jumpTimeCounter -= Time.deltaTime;
                }
                //Debug.Log("Jumping");
            }
        }
        if (isJump && fastJump)
        {

            if (jumpTimeCounter > 0)
            {
                //gravityScale = 3f;
                if (jumpTimeCounter > 0.1f)
                {
                    Debug.Log("J1");
                    rb.velocity = Vector2.up * jumpingSpeed;
                    jumpTimeCounter -= Time.deltaTime;
                }

                else if (jumpTimeCounter > 0.2f)
                {
                    Debug.Log("J2");
                    rb.velocity = Vector2.up * jumpingSpeed * 1.2f;
                    jumpTimeCounter -= Time.deltaTime;
                }

                else if (jumpTimeCounter > 0.3f)
                {
                    Debug.Log("J3");
                    rb.velocity = Vector2.up * jumpingSpeed * 1.4f;
                    jumpTimeCounter -= Time.deltaTime;
                }
                //Debug.Log("Jumping");
            }

       

            else
            {
                isJump = false;
            }
        }
    }

    void JumpDown()
    {
        if (downY <= startY)
        {
            //Debug.Log("下墜");
            m_animation.SetTrigger("T_isDown");
            //coyoteTimeCounter = 0f;
            gravityScale = 3.5f;
        }
    }

    public void OnLanding()
    {
        m_animation.SetBool("isJumping", false);
    }

    //private void Attack()
    //{
    //    //if (Input.GetKeyDown(KeyCode.K))
    //    if(attckCount == 0)
    //    {
    //        //m_animation.
    //        attckCount = 1;
    //        speed = 0f;
    //        m_animation.SetTrigger("T_isAttack");
    //        Invoke("RecoverMove", 0.5f);
    //        //return;
    //        //yield return new WaitForSeconds(0.5f);
    //        //speed = perSpeed;
    //    }

    //    else if (attckCount == 1)
    //    {
    //        //m_animation.
    //        Debug.Log("combo");
    //        speed = 0f;
    //        m_animation.SetTrigger("T_isAttack2");
    //        Invoke("RecoverMove", 0.5f);
    //        attckCount = 0;
    //        //return;
    //        //yield return new WaitForSeconds(0.5f);
    //        //speed = perSpeed;
    //    }
    //}
    //private IEnumerator Attack()
    //{
    //    //if (Input.GetKeyDown(KeyCode.K))
    //    if (attckCount == 0)
    //    {
    //        //m_animation.
    //        //attckCount = 1;
    //        isAttack = true;
    //        speed = 0f;
    //        m_animation.SetTrigger("T_isAttack");
    //        //Invoke("RecoverMove", 0.5f);
    //        //return;
    //        yield return new WaitForSeconds(0.5f);
    //        speed = perSpeed;
    //    }
    //}

    private void AttackCombo()
    {
        if (timeSinceAttack > 0.01f && !isDashing && !isDoubleJump && !isRun)
        {
            isAttack = true;
            attckCount++;

            if (attckCount > 3)
                attckCount = 0;

            if (timeSinceAttack > 1.5f)
                attckCount = 0;


            if (attckCount == 1)
            {
                Debug.Log("combo1");
                speed = 0f;

                m_animation.SetTrigger("T_isAttack1");
            }
            else if (attckCount == 2)
            {
                Debug.Log("combo2");
                speed = 0f;
                m_animation.SetTrigger("T_isAttack2");
            }
            else if (attckCount == 3)
            {
                Debug.Log("combo3");
                speed = 0f;
                m_animation.SetTrigger("T_isAttack3");
            }
            timeSinceAttack = 0f;
            Invoke("RecoverMove", 1.5f);
        }
    }


    private void RecoverMove()
    {
        speed = perSpeed;
    }

    private void RecoverG()
    {
        gravityScale = 3f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGround = true;
        }


        //if ((LayerMask.GetMask("Ground") & 1 << other.gameObject.layer) > 0)
        //{
        //    rb.useGravity = false;
        //    rb.isKinematic = true;

        //    grapple.StartPull();
        //}
    }

    //void Dashing()
    //{
    //    if (!isDashing)
    //    {
    //        // 設置衝刺速度和時間
    //        rb.velocity = new Vector3(dashSpeed, 0f, 0f);
    //        isDashing = true;
    //        dashTimer = dashingTime;
    //    }

    //    // 檢查是否正在衝刺中
    //    if (isDashing)
    //    {
    //        // 減少衝刺時間
    //        dashTimer -= Time.deltaTime;

    //        // 如果衝刺時間用盡，則停止衝刺
    //        if (dashTimer <= 0f)
    //        {
    //            isDashing = false;
    //            rb.velocity = new Vector3(horizontal * speed, 0f, 0f);
    //        }
    //    }
    //}

    private IEnumerator Dash()
    {
        isGrapple = false;
        isPull = false;
        isSwing = false;
        //releasePull = true;
        m_animation.SetBool("isGrapple", false);

        //canDash = false;
        //isDashing = true;
        //float originalGravity = gravityScale;
        //gravityScale = 0f;
        //m_animation.SetTrigger("T_isDash");
        //tr.emitting = true;
        ////Debug.Log("衝刺");
        ////rb.velocity = new Vector3(transform.localScale.x * dashSpeed, rb.velocity.y, rb.velocity.z);
        //if (speed == 6f)
        //{
        //    //rb.AddForce(playerVel * speed);
        //    speed = 24f;
        //    yield return new WaitForSeconds(0.15f);
        //    speed = 16f;
        //    yield return new WaitForSeconds(0.1f);
        //    speed = 0f;
        //    yield return new WaitForSeconds(0.05f);
        //    speed = perSpeed;
        //}

        //else
        //{
        //    speed = 30;
        //    yield return new WaitForSeconds(0.05f);
        //    speed = 24f;
        //    yield return new WaitForSeconds(0.03f);
        //    speed = 0;
        //    yield return new WaitForSeconds(0.02f);
        //    speed = perSpeed;
        //}
        ////Debug.Log("衝刺");

        ////if (speed == 6f)
        ////    yield return new WaitForSeconds(dashingTime);

        ////else
        ////    yield return new WaitForSeconds(0.1f);
        //tr.emitting = false;
        //gravityScale = originalGravity;
        //isDashing = false;
        ////speed = perSpeed;
        //yield return new WaitForSeconds(dashingCooldown);
        //canDash = true;


        if (canDash)
        {

            canDash = false;
            isDashing = true;
            float originalGravity = gravityScale;
            gravityScale = 0f;
            //rb.useGravity = false;
            //rb.mass = 0;
            m_animation.SetTrigger("T_isDash");
            if (isFacingRight) {
                rb.velocity = new Vector3(transform.localScale.x * 20f, 0f);
                //rb.velocity = Vector2.up * 10;
                //tr.emitting = true;
                yield return new WaitForSeconds(0.15f);
                rb.velocity = new Vector3(transform.localScale.x * 10f, 0f);
                //gravityScale = 6f;
                yield return new WaitForSeconds(0.1f);
                rb.velocity = new Vector3(transform.localScale.x * 5f, 0f);
            }
            else if(!isFacingRight){
                rb.velocity = new Vector3(transform.localScale.x * -20f, 0f);
                yield return new WaitForSeconds(0.15f);
                rb.velocity = new Vector3(transform.localScale.x * -10f, 0f);
                yield return new WaitForSeconds(0.1f);
                rb.velocity = new Vector3(transform.localScale.x * -5f, 0f);
            }
            yield return new WaitForSeconds(0.05f);
            //rb.velocity = transform.right * dashSpeed;
            //rb.velocity = new Vector2(transform.position.x + 10f, 0f);
            //Debug.Log("衝刺");
            
            //yield return new WaitForSeconds(dashingTime);
            //tr.emitting = false;
            gravityScale = originalGravity;
            //rb.useGravity = true;
            //rb.mass = 1;
            isDashing = false;
            yield return new WaitForSeconds(dashingCooldown);
            canDash = true;
        }

    }

    void recoverDash()
    {
        canDash = true;
    }

    //Grapple

    public void CloseDownAnime()
    {
        Debug.Log("摔倒");
        m_animation.SetTrigger("T_isCloseDown");
    }

    public void StartPull()
    {
        pulling = true;
    }

    private void DestroyHook()
    {
        if (hook == null) return;

        isGrapple = false;
        pulling = false;
        Destroy(hook.gameObject);
        hook = null;
    }

    private IEnumerator DestroyHookAfterLifetime()
    {
        yield return new WaitForSeconds(15f);

        DestroyHook();
    }

    public void StartSwing()
    {
        if (joint != null)
            return;

        //Debug.Log("Swing");

        //m_animation.SetBool("isDon", true);
        isSwing = true;
        isGrapple = true;
        //Debug.Log("鉤子");

        swingPoint = hook.transform.position;
        float distanceFromPoint = Vector3.Distance(transform.position, swingPoint);

        //joint = gameObject.AddComponent<HingeJoint>();
        
        //joint.autoConfigureConnectedAnchor = false;
        //joint.connectedAnchor = swingPoint;
        //joint.anchor = new Vector3(0, distanceFromPoint, 0);
        //JointSpring spring = joint.spring;
        //spring.spring = 100;
        //spring.damper = 3;
        //spring.targetPosition = 70;
        //joint.spring = spring;
        //joint.useLimits = true;

        joint = gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = swingPoint;
        

        //the distance grapple will try to keep from grapple point.
        joint.maxDistance = distanceFromPoint * 0.3f;
        joint.minDistance = distanceFromPoint * 0.3f;

        //customize values as you like
        joint.spring = 12f;
        joint.damper = 4f;
        joint.massScale = 1f;


        //rb.freezeRotation = false;

    }

    public void StopSwing()
    {
        if (joint == null)
            return;

        //m_animation.SetBool("isDon", false);
        //m_animation.SetTrigger("T_isDonLow");
        
        rb.freezeRotation = true;
        transform.localEulerAngles = new Vector3(0, 90, 0);
        Destroy(joint);
        //transform.rotation = new Quaternion(0, 90, 0, 0);
    }

    public void Near(bool NewNear)
    {
        m_animation.SetBool("isNear", NewNear);
    }

    //void RotateArrow()
    //{
    //    float angle = Rspeed * Time.deltaTime;
    //    // 計算旋轉中心點，這裡以玩家物件的位置為中心點
    //    Vector3 center = target.position;
    //    // 使用 Transform.RotateAround 方法實現旋轉
    //    transform.RotateAround(center, axis, angle);
    //}

    //void ContorlArrow()
    //{
    //    arrowPos = mainCam.ScreenToViewportPoint(Input.mousePosition);

    //    Vector3 rotation = arrowPos - transform.position;

    //    float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Deg2Rad;

    //    transform.rotation = Quaternion.Euler(0, 0, rotZ);
    //}

    //bool OnSlope()
    //{
    //    Ray ray = new Ray(capsuleCollider.bounds.center, Vector3.down);
    //    RaycastHit hit;
    //    if (Physics.Raycast(ray, out hit, rayLength))
    //    {
    //        //Debug.Log("射線");
    //        //isClimb = true;
    //        hitNormal = hit.normal;
    //        slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
    //        //print(slopeAngle);
    //        if (slopeAngle < maxSlopeAngle)
    //        {
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    //private void Climb()
    //{
    //    Vector3 newup = transform.position;

    //    RaycastHit hit;
    //    if (Physics.Raycast(transform.position, Vector3.down, out hit))
    //    {
    //        newup.y = (hit.point + Vector3.up * 2.0f).y;
    //        destUp = hit.normal;
    //    }
    //    transform.position = newup;
    //    transform.up = Vector3.Slerp(transform.up, destUp, 5f * Time.deltaTime);
    //}

    //void Climb (){
    //    if (OnSlope())
    //    {
    //        Debug.Log("climb");
    //        speed = 4;
    //        gravityScale = 6;
    //    }
    //    }


}
