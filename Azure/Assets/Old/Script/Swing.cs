using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    [Header("Swinging")]

    //public Rigidbody box;
    //public GameObject box1;

    private float maxSwingDistance = 25f;
    public Vector3 swingPoint;
    private SpringJoint joint;

    // Start is called before the first frame update
    void Start()
    {
        StartSwing();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartSwing()
    {
        //swingPoint = box.transform.position;
        joint = gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = swingPoint;
        //joint.connectedBody = box;

        float distanceFromPoint = Vector3.Distance(transform.position, swingPoint);

        // the distance grapple will try to keep from grapple point. 
        joint.maxDistance = distanceFromPoint * 0.2f;
        joint.minDistance = distanceFromPoint * 0.1f;

        // customize values as you like
        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;


    }

    public void StopSwing()
    {

        Destroy(joint);
    }
}
