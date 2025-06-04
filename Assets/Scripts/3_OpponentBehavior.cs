using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OpponentBehavior : MonoBehaviour
{
    public GameObject opponentBallPrefab;
    public GameObject target;
    [SerializeField] TextMeshProUGUI messageBoxText;

    public int ballCounter = 3;
    public Vector3 opponentPos = new Vector3(1.0f, 1.5f, -8.0f);

    private GameObject currentBall;
    private BallBehavior ballScript;
    private GameObject closestBall = null;

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

        //Debug.Log("opponent spawn");
    }

    public void Shoot()
    {
        ballInHand = false;

        // Random behavior: shooting (only if closest ball belongs to player) or pointing (default behavior)
        int isPointing = 1;
        Vector3 shootingDir = target.transform.position - transform.position;
        float shootingForce = UnityEngine.Random.Range(minForce, maxForce * 0.5f);

        if (closestBall != null)
        {
            BallBehavior ballScript = closestBall.GetComponent<BallBehavior>();
            if (ballScript.belongsToPlayer())
                isPointing = UnityEngine.Random.Range(0, 2);

            if (isPointing == 0)
            {
                shootingDir = closestBall.transform.position - transform.position;
                shootingForce *= 2.0f;
                Debug.Log("opponent shooting");
            }
            else
                Debug.Log("opponent pointing");
        }
        else
            Debug.Log("opponent pointing");

        shootingDir += new Vector3( UnityEngine.Random.Range(-maxOffset, maxOffset), 
                                    UnityEngine.Random.Range(-maxOffset, maxOffset), 
                                    UnityEngine.Random.Range(-maxOffset, maxOffset) );
        shootingDir.Normalize();
        ballScript.Shoot(shootingDir, shootingForce);

        //Debug.Log("opponent shoot");
    }


    public int getBallCounter()
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

    public void activate(GameObject _closestBall)
    {
        activated = true;
        closestBall = _closestBall;
        if (ballCounter > 0)
            SpawnBall();

        messageBoxText.SetText("Opponent turn");
        messageBoxText.gameObject.SetActive(true);
    }

    public void deactivate()
    {
        activated = false;
        //Debug.Log("opponent deactivated");
    }

    public void newRound()
    {
        ballCounter = 3;
        closestBall = null;
        ballInHand = false;
        activated = false;
    }
}
