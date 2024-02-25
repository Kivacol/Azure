using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PushThing : MonoBehaviour
{
    //public float distance = 1f;
    //public LayerMask boxMask;

    
    public GameObject boxT;
    // Start is called before the first frame update
    void Start()
    {
        //rb.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Physics.q = false;
        //RaycastHit hit = Physics.Raycast(transform.position, Vector2.right * transform.localScale.x, distance, boxMask);

        //Physics2D.queriesStartInColliders = false;
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance , boxMask);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag==("Ground") && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("碰到");
            boxT.GetComponent<FixedJoint>().connectedBody = this.GetComponent<Rigidbody>();
            //BoxThing.
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        
    }


    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;

    //    Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * distance);
    //}
}
