using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookRange : MonoBehaviour
{
    public MoveX movex;

    public Vector3 postion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            movex.isPull = true;
        }

        if (other.gameObject.name == "Close")
        {
            movex.CanClose = true;
        }

        if (other.gameObject.name == "Pull")
        {
            movex.isPull = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        
    }
}
