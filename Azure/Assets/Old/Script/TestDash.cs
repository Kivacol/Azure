using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDash : MonoBehaviour
{
   private float horizontal;
    private float speed = 8f;
    //private float jumpingPower = 16f;
    private bool isFacingRight = true;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 2400f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    [SerializeField] private Rigidbody rb;
    
    [SerializeField] private TrailRenderer tr;

    public float gravityScale = 5f;

    public static float globalGravity = -9.81f;

    private void Update()
    {
        if (isDashing)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

      

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());
        }

        Flip();
    }

    private void FixedUpdate()
    {
        Debug.Log(rb.velocity);
        if (isDashing)
        {
            return;
        }

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        Vector2 gravity = globalGravity * gravityScale * Vector2.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash()
    {
        if (canDash) {
            Debug.Log("dash");
            canDash = false;
            isDashing = true;
            float originalGravity = gravityScale;
            gravityScale = 0f;
            rb.velocity = new Vector3(transform.localScale.x * dashingPower, 0f);
            tr.emitting = true;
            yield return new WaitForSeconds(dashingTime);
            tr.emitting = false;
            gravityScale = originalGravity;
            isDashing = false;
            yield return new WaitForSeconds(dashingCooldown);
            canDash = true; }
    }
}
