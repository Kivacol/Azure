using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class add01 : MonoBehaviour
{
    public float rotationSpeed = 50f; // ±ÛÂà³t«×
                                     
void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
