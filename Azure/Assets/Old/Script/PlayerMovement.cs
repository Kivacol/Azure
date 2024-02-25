using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    [Header("Grevity")]
    //重力
    public float gravityScale = 5f;

    public static float globalGravity = -9.81f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 gravity = globalGravity * gravityScale * Vector2.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }
}
