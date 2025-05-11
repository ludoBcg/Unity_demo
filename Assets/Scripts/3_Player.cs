using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject ballPrefab;
    public GameObject target;
    public GameObject arrowDir;

    private GameObject currentBall;
    private BallBehavior ballScript;
    private TargetBehavior targetScript;

    // display speed
    [SerializeField] TextMeshProUGUI BallCounterText;
    [SerializeField] TextMeshProUGUI ForceCounterText;

    public float maxShootingForce = 80.0f;
    public float minShootingForce = 30.0f;
    public int ballCounter = 3;

    private Vector3 ballInitPos = Vector3.zero;
    private Vector3 shootingDir = Vector3.zero;
    private float shootingForce = 0.0f;
    private bool ballInHand = false;
    private bool isKeyDown = false;



    // Start is called before the first frame update
    void Start()
    {
        shootingDir = Vector3.forward;
        shootingForce = minShootingForce;

        targetScript = target.GetComponent<TargetBehavior>();

        ballInitPos = new Vector3(0.0f, 1.5f, -8.0f);
        arrowDir.transform.position = ballInitPos + new Vector3(0.0f, 0.0f, 1.0f);

        SpawnBall();
    }

    // Update is called once per frame
    void Update()
    {
        // UI
        int nbBalls = ballCounter + 1;
        BallCounterText.SetText("Balls: " + nbBalls);
        ForceCounterText.SetText("Force: " + shootingForce.ToString("#.00"));

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

    void FixedUpdate()
    {
        // increase shooting force if spacebar is pressed
        if (isKeyDown && shootingForce < maxShootingForce)
        {
            shootingForce += 0.1f;
        }

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
        currentBall = Instantiate(ballPrefab, ballInitPos, ballPrefab.transform.rotation);
        ballScript = currentBall.GetComponent<BallBehavior>();
        arrowDir.SetActive(true);
        Debug.Log("spawn");
    }

    void Shoot()
    {
        ballInHand = false;
        ballScript.Shoot(shootingDir, shootingForce);
        arrowDir.SetActive(false);
        Debug.Log("Shoot(" + shootingDir + " , " + shootingForce.ToString() + ")");
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
}
