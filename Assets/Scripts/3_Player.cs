/*********************************************************************************************************************
 *
 * 3_Player.cs
 * 
 * Unity_demo
 * Scene 3_playground
 * 
 * Ludovic Blache
 *
 *********************************************************************************************************************/

using TMPro;
using UnityEngine;

// Player controls
public class Player : MonoBehaviour
{

    public GameObject playerBallPrefab;
    public GameObject arrowDir;
    public GameObject scoreManager;

    private GameObject currentBall;
    private BallBehavior ballScript;

    [SerializeField] TextMeshProUGUI ballCounterText;
    [SerializeField] TextMeshProUGUI forceCounterText;

    public Vector3 playerPos = new Vector3(0.0f, 1.5f, -8.0f);
    private Quaternion initArrowRot;
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


    // Use Awake() for self init
    void Awake()
    {
        shootingDir = Vector3.forward;
        shootingForce = minShootingForce;

        ballInitPos = playerPos;
        arrowDir.transform.position = ballInitPos + new Vector3(-0.12f, 0.0f, 1.0f);

        initArrowRot = Quaternion.Euler(90.0f, 0.0f, 0.0f);
        arrowDir.transform.rotation = initArrowRot;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    
    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            // UI
            ballCounterText.SetText(ballCounter + " balls to play");
            float displayForce = 10.0f * (shootingForce - minShootingForce) / (maxShootingForce - minShootingForce);
            forceCounterText.SetText("Force: " + displayForce.ToString("#0.0"));
            
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
        
        currentBall = Instantiate(playerBallPrefab, ballInitPos, playerBallPrefab.transform.rotation);
        ballScript = currentBall.GetComponent<BallBehavior>();
        ballScript.AssignToPlayer(true);
        arrowDir.SetActive(true);
    }

    void Shoot()
    {
        ballCounter--;
        ballInHand = false;

        ballScript.Shoot(shootingDir, shootingForce, maxOffset);
        arrowDir.SetActive(false);
    }

    public void RotateArrow(float _rotHorizontal, float _rotVertical)
    {
        arrowDir.transform.RotateAround(ballInitPos, new Vector3(0.0f, 1.0f, 0.0f), _rotHorizontal);
        arrowDir.transform.RotateAround(ballInitPos, -arrowDir.transform.right, _rotVertical);
    }

    public void ReInitArrow()
    {
        arrowDir.transform.rotation = initArrowRot;
        arrowDir.transform.position = ballInitPos + new Vector3(-0.12f, 0.0f, 1.0f);
    }

    public void SetShootingDir(Vector3 _direction)
    {
        shootingDir = _direction;
    }

    public void Activate()
    {
        activated = true;
        Start();

        if (ballCounter>0)
            SpawnBall();

    }

    public void Deactivate()
    {
        activated = false;
    }

    public bool GetBallInHand()
    {
        return ballInHand;
    }

    public int GetBallCounter()
    {
        return ballCounter;
    }

    public void NewRound()
    {
        ballCounter = 3;
        ballInHand = false;
        isKeyDown = false;
        activated = false;
    }
}
