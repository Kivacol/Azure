using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class JellyAnimator : MonoBehaviour
{

    public Animator animator;
    Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void IsRun()
    {
        animator.SetBool("isRuning", true);
    }

    public void StopRun()
    {
        animator.SetBool("isRuning", false);
    }

    public void IsJump()
    {
        animator.SetBool("isJumping", true);
    }

    public void IsJumpMid()
    {
        animator.SetBool("isJumping", false);
    }

    public void IsJumpDown()
    {
        animator.SetFloat("AirSpeedY", rb.velocity.y);
    }

}
