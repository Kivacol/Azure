using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleTest2 : MonoBehaviour
{
    public SpringJoint joint;
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<SpringJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = cam.ScreenToWorldPoint(Input.mousePosition);

            Collider2D col =  Physics2D.OverlapPoint(pos);

            if (col!=null)
            {
                joint.connectedBody = col.GetComponent<Rigidbody>();
            }
        }
    }
}
