using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    private Rigidbody ballRb;

    private float epsilon = 0.001f;


    // Start is called before the first frame update
    void Start()
    {
        ballRb = GetComponent<Rigidbody>();
        // desable gravity at beginning
        ballRb.useGravity = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // checks if the ball is still moving or not
    public bool stopped()
    {
        if (Mathf.Abs(-ballRb.velocity.x) < epsilon
            && Mathf.Abs(-ballRb.velocity.y) < epsilon
            && Mathf.Abs(-ballRb.velocity.z) < epsilon)
        {
            return true;
        }
        return false;
    }
}