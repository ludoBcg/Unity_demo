using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    public float shootForce;

    private Rigidbody ballRb;
    private bool wasShot;


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
    public void Shoot()
    {
        if (!wasShot)
        {
            // jump on spacebar
            //if (Input.GetKeyDown(KeyCode.Space))
            {
                // enable gravity
                ballRb.useGravity = true;
                // shoot
                ballRb.AddForce(Vector3.forward * shootForce, ForceMode.Impulse);
                // cannot be shot anymore
                wasShot = true;
            }
        }
    }
}
