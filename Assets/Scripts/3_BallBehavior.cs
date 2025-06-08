using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    private Rigidbody ballRb;
    private bool wasShot = false;

    private float epsilon = 0.001f;
    private Vector2 minFieldCoords = new Vector2(-13.5f, -21.0f);
    private Vector2 maxFieldCoords = new Vector2(13.5f, 13.5f);

    private bool isPlayer = true; // flag for player's or opponent's balls


    
    // Start is called before the first frame update
    void Start()
    {
        ballRb = GetComponent<Rigidbody>();
        // disable gravity at beginning
        ballRb.useGravity = false;
        wasShot = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // consider the ball has been shot when RB has force applied to it
        // do not change wasShot flag in Shoot() to avoid it been checked before animation loop
        if (ballRb.GetAccumulatedForce() != new Vector3(0.0f, 0.0f, 0.0f))
        {
            // cannot be shot anymore
            wasShot = true;
        }

        // Delete balls when thrown out of field
        if(outOfField())
        {
            pinToWall();
        }
    }

    bool outOfField()
    {
        if (transform.position.x <= minFieldCoords.x || transform.position.x >= maxFieldCoords.x ||
            transform.position.z <= minFieldCoords.y || transform.position.z >= maxFieldCoords.y)
            return true;

        return false;
    }

    void pinToWall()
    {
        ballRb.velocity = new Vector3(0.0f, 0.0f, 0.0f);

        transform.position = new Vector3( Mathf.Max(minFieldCoords.x + 0.25f, Mathf.Min(transform.position.x, maxFieldCoords.x - 0.25f)),
                                          0.5f,
                                          Mathf.Max(minFieldCoords.y + 0.25f, Mathf.Min(transform.position.z, maxFieldCoords.y - 0.25f)) );
    }

    // to be called by player when he shoots
    public void Shoot(Vector3 _direction, float _shootingForce)
    {
        Start();
        if (!wasShot)
        {
            // enable gravity
            ballRb.useGravity = true;
            // shoot
            ballRb.AddForce(_direction * _shootingForce, ForceMode.Impulse);
        }
    }

    // checks if the ball is still moving or not
    public bool stopped()
    {
        if (wasShot)
        {
            if (Mathf.Abs(-ballRb.velocity.x) < epsilon
              && Mathf.Abs(-ballRb.velocity.y) < epsilon
              && Mathf.Abs(-ballRb.velocity.z) < epsilon)
            {
                return true;
            }
        }
        return false;
    }

    public void assignToPlayer(bool _isPlayer)
    {
        isPlayer = _isPlayer;
    }

    public bool belongsToPlayer()
    {
        return isPlayer;
    }
}