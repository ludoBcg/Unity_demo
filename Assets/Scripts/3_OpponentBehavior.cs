using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentBehavior : MonoBehaviour
{
    public GameObject opponentBallPrefab;
    public GameObject target;

    public int ballCounter = 3;
    public Vector3 opponentPos = new Vector3(1.0f, 1.5f, -8.0f);

    private GameObject currentBall;
    private BallBehavior ballScript;

    private bool ballInHand = false;
    private bool activated = false;

    private float minForce = 30.0f;
    private float maxForce = 50.0f;
    private float maxOffset = 0.2f; // maxOffset for random shooting direction


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (activated && ballInHand)
        {
            Shoot();
        }
    }

    public void SpawnBall()
    {
        ballInHand = true;
        ballCounter--;
        currentBall = Instantiate(opponentBallPrefab, opponentPos, opponentBallPrefab.transform.rotation);
        ballScript = currentBall.GetComponent<BallBehavior>();
        ballScript.assignToPlayer(false);

        Debug.Log("opponent spawn");
    }

    public void Shoot()
    {
        ballInHand = false;

        Vector3 shootingDir = target.transform.position - transform.position;
        shootingDir += new Vector3( UnityEngine.Random.Range(-maxOffset, maxOffset), 
                                    UnityEngine.Random.Range(-maxOffset, maxOffset), 
                                    UnityEngine.Random.Range(-maxOffset, maxOffset) );
        shootingDir.Normalize();
        ballScript.Shoot(shootingDir/*Vector3.forward*/, UnityEngine.Random.Range(minForce, maxForce));

        Debug.Log("opponent shoot");
    }


    public int GetBallCounter()
    {
        return ballCounter;
    }

    public bool hasBallInHand()
    {
        return ballInHand;
    }

    public bool ballStopped()
    {
        return ballScript.stopped();
    }

    public void activate()
    {
        activated = true;
        if (ballCounter > 0)
            SpawnBall();
        Debug.Log("opponent activated");
    }

    public void deactivate()
    {
        activated = false;
        Debug.Log("opponent deactivated");
    }
}
