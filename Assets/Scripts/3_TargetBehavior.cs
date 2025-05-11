using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    private Rigidbody targetRb;

    private float epsilon = 0.001f;


    // Start is called before the first frame update
    void Start()
    {
        targetRb = GetComponent<Rigidbody>();
        targetRb.useGravity = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // checks if the ball is still moving or not
    public bool stopped()
    {
        if (Mathf.Abs(-targetRb.velocity.x) < epsilon
            && Mathf.Abs(-targetRb.velocity.y) < epsilon
            && Mathf.Abs(-targetRb.velocity.z) < epsilon)
        {
            return true;
        }
        return false;
    }

}