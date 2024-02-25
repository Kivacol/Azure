using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyDash : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    JellyMove jellyMove;

    public bool canDash;
    public bool isDashing;
    public float dashForce;
    public float dashForce2;
    public float dashForce3;
    public float dashTime;
    public float dashCoolDownTime;

    public float preGrravity;


    // Start is called before the first frame update
    void Start()
    {
        jellyMove = GetComponent<JellyMove>();
        canDash = true;
    }
    public void UseDash()
    {
        StartCoroutine(Dash());
    }

    public IEnumerator Dash()
    {
        if (canDash)
        {
            canDash = false;
            isDashing = true;
            preGrravity = rb.gravityScale;
            rb.gravityScale = 0;

            if (jellyMove.isFacingRight)
            {
                rb.velocity = new Vector2(transform.localScale.x * dashForce, 0f);
                yield return new WaitForSeconds(0.05f);
                rb.velocity = new Vector2(transform.localScale.x * dashForce2, 0f);
                yield return new WaitForSeconds(0.025f);
                rb.velocity = new Vector2(transform.localScale.x * dashForce3, 0f);
            }

            if (!jellyMove.isFacingRight)
            {
                rb.velocity = new Vector2(transform.localScale.x * -dashForce, 0f);
                yield return new WaitForSeconds(0.05f);
                rb.velocity = new Vector2(transform.localScale.x * -dashForce2, 0f);
                yield return new WaitForSeconds(0.025f);
                rb.velocity = new Vector2(transform.localScale.x * -dashForce3, 0f);
            }

            yield return new WaitForSeconds(dashTime);

            isDashing = false;
            rb.gravityScale = preGrravity;

            yield return new WaitForSeconds(dashCoolDownTime);
            canDash = true;
        }
    }
}
