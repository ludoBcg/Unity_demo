using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject ballPrefab;
    public GameObject target;

    private GameObject currentBall;
    private BallBehavior ballScript;
    private TargetBehavior targetScript;
    

    public float shootingForce = 100.0f; 
    public int ballCounter = 3;
    private bool ballInHand = false;



    // Start is called before the first frame update
    void Start()
    {
        targetScript = target.GetComponent<TargetBehavior>();
        SpawnBall();
    }

    // Update is called once per frame
    void Update()
    {
        // press spacebar to shoot the current ball
        if (Input.GetKeyDown(KeyCode.Space) && ballInHand)
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        // spawns a new ball after the previous one was shot,
        // if there are still balls available
        if (!ballInHand && ballCounter > 0)
        {
            // wait for previous balls and target to be still
            if (ballScript.stopped() && targetScript.stopped())
            {
                SpawnBall();
            }
        }

    }

    void SpawnBall()
    {
        ballInHand = true;
        ballCounter--;
        currentBall = Instantiate(ballPrefab, new Vector3(0.0f, 1.5f, -8.0f), ballPrefab.transform.rotation);
        ballScript = currentBall.GetComponent<BallBehavior>();
        Debug.Log("spawn");
    }

    void Shoot()
    {
        ballInHand = false;
        ballScript.Shoot(shootingForce);
        Debug.Log("shoot");
    }
}
