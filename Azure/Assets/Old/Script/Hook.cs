using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] float hookForce = 80f;

    MoveX grapple;
    Rigidbody rigid;
    LineRenderer lineRenderer;


    //private bool isFacingRight = true;
    //private float horizontal;

    public void Initialize(MoveX grapple, Transform shootTransform , bool isFacingRight)
    {
        //transform.forward = shootTransform.forward;
        if (isFacingRight)
        {
            transform.forward = shootTransform.forward;
        }
        else
        {
            transform.forward = shootTransform.forward * -1;
        }
        this.grapple = grapple;

        rigid = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();

        rigid.AddForce(transform.forward * hookForce, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        //horizontal = Input.GetAxisRaw("Horizontal");

        //if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        //{
        //    isFacingRight = !isFacingRight;
        //    Vector3 localScale = transform.localScale;

        //    localScale.z *= -1f;
        //    transform.localScale = localScale;
        //}
        Vector3[] positions = new Vector3[]
            {
                transform.position,
                grapple.transform.position
            };

        lineRenderer.SetPositions(positions);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((LayerMask.GetMask("Grapple") & 1 << other.gameObject.layer) > 0)
        {
            rigid.useGravity = false;
            rigid.isKinematic = true;
            
            //grapple.gameObject.SendMessage("StartSwing");

            grapple.StartPull();
        }

        if (other.gameObject.name == "Swing")
        {
            grapple.gameObject.SendMessage("StartSwing");
        }

        if (other.gameObject.name == "Close")
        {
            grapple.CanClose = true;
            //grapple.isClose = true;
            other.gameObject.SendMessage("SetHook");
        }

        if (other.gameObject.name == "Pull")
        {
            grapple.isPull = true;
            //grapple.gameObject.SendMessage("isPull",false);
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    other.gameObject.SendMessage("SetHook",false);
    //}

    //internal void Initialize(MoveX moveX, Transform shootTransform, bool isFacingRight)
    //{
    //    throw new NotImplementedException();
    //}

    //internal void Initialize(MoveX moveX, Transform shootTransform, bool isFacingRight)
    //{
    //    throw new NotImplementedException();
    //}
}