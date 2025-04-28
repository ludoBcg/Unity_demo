using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    private Rigidbody ballRb;
    private bool wasShot;

    private float epsilon = 0.001f;


    // Start is called before the first frame update
    void Start()
    {
        ballRb = GetComponent<Rigidbody>();
        wasShot = false;
        // desable gravity at beginning
        ballRb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // to be called by player when he shoots
    public void Shoot(float _shootingForce)
    {
        if (!wasShot)
        {
            // enable gravity
            ballRb.useGravity = true;
            // shoot
            ballRb.AddForce(Vector3.forward * _shootingForce, ForceMode.Impulse);
            // cannot be shot anymore
            wasShot = true;
        }
    }

    // checks if the ball is still moving or not
    public bool stopped()
    {
        if (wasShot)
        {
            if ( Mathf.Abs(-ballRb.velocity.x) < epsilon
              && Mathf.Abs(-ballRb.velocity.y) < epsilon
              && Mathf.Abs(-ballRb.velocity.z) < epsilon)
            {
                return true;
            }
        }
        return false;
    }
}