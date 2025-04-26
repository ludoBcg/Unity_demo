using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject ballPrefab;

    private GameObject currentBall;
    private BallBehavior ballScript;

    public int ballCounter = 3;
    private bool readyToShoot = false;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // spawn a new ball after shooting (if some balls are still available)
        if (ballCounter > 0 && !readyToShoot)
        {
            SpawnBall();
            Debug.Log("spawn ball");
        }

        // press shapcebar to shoot the current ball
        if (Input.GetKeyDown(KeyCode.Space) && readyToShoot)
        {
            Debug.Log("shoot ball");
            ballScript.Shoot();
            readyToShoot = false;
        }
    }

    void SpawnBall()
    {
        currentBall = Instantiate(ballPrefab, new Vector3(0.0f, 1.5f, -8.0f), ballPrefab.transform.rotation);
        ballScript = currentBall.GetComponent<BallBehavior>();
        ballCounter--;
        readyToShoot = true;
    }
}
