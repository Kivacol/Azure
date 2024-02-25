using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colse : MonoBehaviour
{
    private Rigidbody rb;
    public MoveX movex;

    Hook hook;

    public float colseSpeed = 5f;

    public bool isHook = false;
    public bool isNear = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isNear)
        {
            return;
        }

        if (!movex.isGrapple)
        {
            isHook = false;
            movex.isClose = false;
        }

        if (movex.isClose && isHook)
        {
            rb.AddForce((movex.transform.position - transform.position).normalized * colseSpeed, ForceMode.VelocityChange);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isHook = false;
            isNear = true;
            movex.isGrapple = false;
            movex.canDash = true;
            movex.CanClose = false;
            //m_animator.SetBool("isNear", true)
            movex.gameObject.SendMessage("Near", true);
            if (movex.isClose) {
                movex.gameObject.SendMessage("CloseDownAnime");
                movex.isClose = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //m_animator.SetBool("isNear", false);
        movex.gameObject.SendMessage("Near",false);
        isNear = false;
    }

    void SetHook()
    {
        isHook = true;
    }
  
}
