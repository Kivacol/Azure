using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Move : MonoBehaviour
{
    CapsuleCollider capsuleCollider;

    [Header("Move")]
    private Vector3 playerVel;
    private float horizontal;
    public float speed;

    private float perSpeed;
    [SerializeField] private bool isRun = false;
    public bool stopRuning = false;

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

    //internal void Initialize(Grapple grapple, Transform shootTransform)
    //{
    //    throw new NotImplementedException();
    //}

    [Header("Dash")]
    private bool canDash = true;
    private bool isDashing;
    [SerializeField]public float dashSpeed = 800f;
    private float dashingTime = 0.8f;
    float dashTimer;
    private float dashingCooldown = 1f;
    //public bool getDash = false;

    [SerializeField] private Animator m_animation;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Rigidbody rb;


    [Header("Grevity")]
    //重力
    public float gravityScale = 5f;

    public static float globalGravity = -9.81f;

    [Header("InputAct")]
    //控制器
    private Vector2 moveInputValue;
    private PlayerInputActions controls;


    //慣性
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
    [SerializeField] float pullSpeed = 0.5f;
    [SerializeField] float stopDistance = 4f;
    [SerializeField] GameObject hookPrefab;
    public Transform shootTransform;

    Hook hook;
    bool pulling = false;
    //Rigidbody rigid;
    bool isGrapple;

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

    void Awake()
    {
        controls = new PlayerInputActions();

        controls.GamePlay.Move.performed += ctx => moveInputValue = ctx.ReadValue<Vector2>();
        controls.GamePlay.Move.canceled += ctx => moveInputValue = Vector2.zero;

        controls.GamePlay.Run.started += ctx => isRun = true;
        controls.GamePlay.Run.canceled += ctx => isRun = false;

        controls.GamePlay.Jump.started += ctx => Jump();
        controls.GamePlay.Jump.canceled += ctx => isJump = false;

        controls.GamePlay.Attack.started += ctx => AttackCombo();

        controls.GamePlay.Dash.started += ctx => StartCoroutine(Dash());

        controls.GamePlay.Grapple.started += ctx => isGrapple = true;
        controls.GamePlay.Grapple.canceled += ctx => isGrapple = false;
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
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<TrailRenderer>();
        m_animation = GetComponent<Animator>();
        perSpeed = speed;
        lastPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {

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

        if (isAttack)
        {
            rb.velocity = new Vector2(transform.localScale.x * attackSpeed, rb.velocity.y);
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        //Run();
        if (stopRuning) {
            return;
        }
        else if (!stopRuning)
        {
            Run();
        }
        RunFast();

        JumpCut();

        

        if (isGround && !isDashing && !isRun && !isAttack && !isClimb)
        {
            RecoverMove();
            RecoverG();
        }

        if (isGround == true)//&& !Input.GetButton("Jump"))
        {
            slowJump = false;
            fastJump = false;
        }

        //攻擊時間
        timeSinceAttack += Time.deltaTime;

        if (timeSinceAttack > 1f)
            attckCount = 0;

        Flip();


        //Hook

        if (hook == null && isGrapple == true)
        {
            StopAllCoroutines();
            pulling = false;
            hook = Instantiate(hookPrefab, shootTransform.position, Quaternion.identity).GetComponent<Hook>();
            //hook.Initialize(this, shootTransform, isFacingRight);
            StartCoroutine(DestroyHookAfterLifetime());
        }
        else if (hook != null && isGrapple == false)
        {
            DestroyHook();
        }

        if (!pulling || hook == null) return;

        if (Vector3.Distance(transform.position, hook.transform.position) <= stopDistance)
        {
            DestroyHook();
        }
        else
        {
            rb.AddForce((hook.transform.position - transform.position).normalized * pullSpeed, ForceMode.VelocityChange);
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
            
            if (!isClimb)
            {
                speed = 10;
            }
            
            m_animation.SetBool("isFast", true);
            tr.emitting = true;
            
        }
        else
        {
            if (!isClimb)
            {
                m_animation.SetBool("isFast", false);
                tr.emitting = false;
                speed = perSpeed;
            }
            else
            {
                m_animation.SetBool("isFast", false);
                tr.emitting = false;
            }

        }
    }

    void Flip()
    {
        

        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;

            //localScale.z *= -1f;
            //transform.localScale = localScale;

            //if (isGround && speed == 6f)
            //{
            //    stopRuning = true;
            //    //m_animation.SetTrigger("T_isTurn");
            //    //StartCoroutine(FlipDrag());
            //}
            if (isGround && speed == 10f)
            {
                stopRuning = true;
                m_animation.SetTrigger("T_isTurn");
                StartCoroutine(FlipDragFast());
            }
            else
            {
                //Vector3 localScale = transform.localScale;
                m_animation.SetTrigger("T_isTurn");
                localScale.z *= -1f;
                transform.localScale = localScale;
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
        m_animation.SetTrigger("T_isTurn");
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
        speed = 10;
        stopRuning = false;
    }




    void Jump()
    {
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
            if (isGround == true)
            {
                if (speed == 6f)
                {
                    m_animation.SetBool("isJumping", true);
                    //rb.velocity = Vector2.up * jumpingSpeed;
                    speed = 4.5f;
                    isGround = false;
                    isDoubleJump = true;
                    jumpTimeCounter = jumpTime;
                    isJump = true;
                    slowJump = true;
                    gravityScale = 4.5f;
                    //m_animation.SetTrigger("T_isDown");
                    //Invoke("RecoverMove", 1f);
                    //Debug.Log("Jump");
                    return;
                }

                else if (speed == 10f)
                {
                    m_animation.SetBool("isJumping", true);
                    //rb.velocity = Vector2.up * jumpingSpeed;
                    speed = 6f;
                    isGround = false;
                    isDoubleJump = true;
                    jumpTimeCounter = jumpFastTime;
                    isJump = true;
                    fastJump = true;
                    gravityScale = 5.5f;
                    //m_animation.SetTrigger("T_isDown");
                    //Invoke("RecoverMove", 1f);
                    //Debug.Log("Jump");
                    return;
                }
            }
        }

        //if (Input.GetButtonDown("Jump") && isDoubleJump == true)
        if (isDoubleJump)
        {
            if (slowJump) {
                m_animation.SetTrigger("T_isDuobleJump");
                //Debug.Log("DoubleJump");
                speed = 3.5f;
                gravityScale = 6f;
                //rb.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
                rb.velocity = Vector2.up * jumpingSpeed * 1.9f;
                //m_animation.SetTrigger("T_isDown");
                isDoubleJump = false;
                //Invoke("RecoverMove", 0.5f);
                //isDoubleJump = false;
            }

            if (fastJump)
            {
                m_animation.SetTrigger("T_isDuobleJump");

                speed = 2.5f;
                gravityScale = 7f;

                rb.velocity = Vector2.up * jumpingSpeed * 2.3f;

                isDoubleJump = false;

            }
        }



    }

    void JumpCut()
    {
        if (isJump && slowJump)
        {

            if (jumpTimeCounter > 0)
            {
                //gravityScale = 4f;
                if (jumpTimeCounter > 0.05f)
                {
                    Debug.Log("J1");
                    rb.velocity = Vector2.up * jumpingSpeed * 0.9f;
                    jumpTimeCounter -= Time.deltaTime;
                }

                else if (jumpTimeCounter > 0.1f)
                {
                    Debug.Log("J2");
                    rb.velocity = Vector2.up * jumpingSpeed;
                    jumpTimeCounter -= Time.deltaTime;
                }

                else if (jumpTimeCounter > 0.2f)
                {
                    Debug.Log("J3");
                    rb.velocity = Vector2.up * jumpingSpeed * 1.1f;
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
        gravityScale = 5f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            m_animation.SetBool("isJumping", false);
            isGround = true;
            isDoubleJump = false;
        }

        if (other.gameObject.tag == "open")
        {

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
            rb.velocity = new Vector3(transform.localScale.z * 2500f, 0f);
            tr.emitting = true;
            yield return new WaitForSeconds(0.1f);
            rb.velocity = new Vector3(transform.localScale.z * 1000f, 0f);
            //gravityScale = 6f;
            yield return new WaitForSeconds(0.2f);
            rb.velocity = new Vector3(transform.localScale.z * 500f, 0f);
            yield return new WaitForSeconds(0.2f);
            //rb.velocity = transform.right * dashSpeed;
            //rb.velocity = new Vector2(transform.position.x + 10f, 0f);
            Debug.Log("衝刺");
            
            //yield return new WaitForSeconds(dashingTime);
            tr.emitting = false;
            gravityScale = originalGravity;
            //rb.useGravity = true;
            //rb.mass = 1;
            isDashing = false;
            yield return new WaitForSeconds(dashingCooldown);
            canDash = true;
        }

    }

    //Grapple

    public void StartPull()
    {
        pulling = true;
    }

    private void DestroyHook()
    {
        if (hook == null) return;

        pulling = false;
        Destroy(hook.gameObject);
        hook = null;
    }

    private IEnumerator DestroyHookAfterLifetime()
    {
        yield return new WaitForSeconds(8f);

        DestroyHook();
    }

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
