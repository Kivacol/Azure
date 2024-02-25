using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testOnSlope : MonoBehaviour
{
    public float speed = 5f;
    Rigidbody rb;

    Vector3 moveAmount;

    public Transform groundCheck;
    public float rayLength = 0.5f;
    public float maxSlopeAngle = 30f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"),0,0).normalized;
        moveAmount = moveDir * speed * Time.deltaTime;
        rb.MovePosition(rb.position + moveAmount);

        OnSlope();
    }

    private void FixedUpdate()
    {
        //rb.MovePosition(rb.position + moveAmount);
    }

    bool OnSlope()
    {
        Ray ray = new Ray(groundCheck.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            Debug.Log("射線");
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            print(slopeAngle);
            if (slopeAngle < maxSlopeAngle)
            {
                return true;
            }
        }

        return false;
    }

}
