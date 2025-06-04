using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject playerBallPrefab;
    public GameObject arrowDir;
    public GameObject scoreManager;

    private GameObject currentBall;
    private BallBehavior ballScript;
    private ScoringManager scoringScript;

    [SerializeField] TextMeshProUGUI ballCounterText;
    [SerializeField] TextMeshProUGUI forceCounterText;
    [SerializeField] TextMeshProUGUI messageBoxText;

    public Vector3 playerPos = new Vector3(0.0f, 1.5f, -8.0f);
    public float maxShootingForce = 80.0f;
    public float minShootingForce = 30.0f;
    public int ballCounter = 3;
    public float maxOffset = 0.1f; // maxOffset for random shooting direction

    private Vector3 ballInitPos = Vector3.zero;
    private Vector3 shootingDir = Vector3.zero;
    private float shootingForce = 0.0f;
    private bool ballInHand = false;
    private bool isKeyDown = false;
    private bool activated = false;



    // Start is called before the first frame update
    void Start()
    {
        shootingDir = Vector3.forward;
        shootingForce = minShootingForce;

        ballInitPos = playerPos;
        arrowDir.transform.position = ballInitPos + new Vector3(-0.12f, 0.0f, 1.0f);

        Debug.Log("player Start ");
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            // UI
            int nbBalls = ballCounter + 1;
            ballCounterText.SetText(nbBalls + " balls to play");
            forceCounterText.SetText("Force: " + shootingForce.ToString("#.00"));
            
            // Shooting
            if (ballInHand)
            {
                // press spacebar to start accumalte shooting force
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isKeyDown = true;
                }
                // release spacebar to shoot
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    if (isKeyDown)
                        Shoot();

                    isKeyDown = false;
                    // erase accumulated force
                    shootingForce = minShootingForce;
                }
            }
            
        }
    }

    // cf. https://docs.unity3d.com/460/Documentation/Manual/ExecutionOrder.html
    void FixedUpdate()
    {
        if (activated)
        {
            // increase shooting force if spacebar is pressed
            if (isKeyDown && shootingForce < maxShootingForce)
            {
                shootingForce += 0.2f;
            }
        }
    }

    void SpawnBall()
    {
        ballInHand = true;
        ballCounter--;
        
        currentBall = Instantiate(playerBallPrefab, ballInitPos, playerBallPrefab.transform.rotation);
        ballScript = currentBall.GetComponent<BallBehavior>();
        ballScript.assignToPlayer(true);
        arrowDir.SetActive(true);

        //Debug.Log("player spawn");
    }

    void Shoot()
    {
        ballInHand = false;

        // add randomness to the shooting dir
        shootingDir += new Vector3(Random.Range(-maxOffset, maxOffset), Random.Range(-maxOffset, maxOffset), Random.Range(-maxOffset, maxOffset));
        shootingDir.Normalize();

        ballScript.Shoot(shootingDir, shootingForce);
        arrowDir.SetActive(false);

        //Debug.Log("player shoot");
    }

    public void rotateArrow(float _rotHorizontal, float _rotVertical)
    {
        arrowDir.transform.RotateAround(ballInitPos, new Vector3(0.0f, 1.0f, 0.0f), _rotHorizontal);
        arrowDir.transform.RotateAround(ballInitPos, -arrowDir.transform.right, _rotVertical);
    }

    public void setShootingDir(Vector3 _direction)
    {
        shootingDir = _direction;
    }

    public void activate()
    {
        activated = true;
        Start();

        if (ballCounter>0)
            SpawnBall();

        messageBoxText.SetText("Your turn");
        messageBoxText.gameObject.SetActive(true);
    }

    public void deactivate()
    {
        activated = false;
        //Debug.Log("player deactivated");
    }

    public bool getBallInHand()
    {
        return ballInHand;
    }

    public int getBallCounter()
    {
        return ballCounter;
    }

    public void newRound()
    {
        ballCounter = 3;
        ballInHand = false;
        isKeyDown = false;
        activated = false;
    }
}
