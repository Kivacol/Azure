using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = new Color(1, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
