using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    private float horizontal;
    [SerializeField] private float speed = 6f;
    public Rigidbody rb;

    //public Transform target;
    private Animator animator;
    public Vector3 velocity = Vector3.zero;
    [SerializeField] private float smoothTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        Vector2 playerVel = new Vector2(horizontal * speed, rb.velocity.y);
        rb.velocity = playerVel;
        bool haveSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        animator.SetFloat("speed", Mathf.Abs(horizontal));

        //transform.position = Vector3.SmoothDamp(transform.position, target.position,ref velocity , speed * Time.deltaTime , smoothTime);
    }
}
