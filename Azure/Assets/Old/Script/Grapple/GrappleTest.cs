using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleTest : MonoBehaviour
{
    private Vector3 targetops;
    private LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mouse = Input.mousePosition;
            Vector3 obj = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 direction = mouse - obj;
            direction.z = 0;
            direction = direction.normalized;
            transform.up = direction;
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.1f, 0), direction);
            if(hit.collider.tag=="Ground" && hit.collider != null)
            {
                targetops = hit.point;

            }
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, targetops);
        }

        else if(Input.GetMouseButtonUp(0))
        {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position);
        }
    }
}
