/*********************************************************************************************************************
 *
 * 3_OpponentBehavior.cs
 * 
 * Unity_demo
 * Scene 3_playground
 * 
 * Ludovic Blache
 *
 *********************************************************************************************************************/

using UnityEngine;

// Represents the opponent, i.e., computer player
public class OpponentBehavior : MonoBehaviour
{
    public GameObject opponentBallPrefab;
    public GameObject target;

    public int ballCounter = 3;
    public Vector3 opponentPos = new Vector3(0.0f, 1.5f, -8.0f);

    private GameObject currentBall;
    private BallBehavior ballScript;
    private GameObject closestBall = null;

    private bool ballInHand = false;
    private bool activated = false;

    private float minForce = 30.0f;
    private float maxForce = 50.0f;
    public float maxOffset = 0.2f; // maxOffset for random shooting direction
    private float shootingThreshold = 5.0f; // minimum distance from target to consider shooting


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
        currentBall = Instantiate(opponentBallPrefab, opponentPos, opponentBallPrefab.transform.rotation);
        ballScript = currentBall.GetComponent<BallBehavior>();
        ballScript.AssignToPlayer(false);
    }

    public void Shoot()
    {
        ballCounter--;
        ballInHand = false;

        // Random behavior: shooting (only if closest ball belongs to player) or pointing (default behavior)
        int isPointing = 1;
        Vector3 shootingDir = target.transform.position - transform.position;
        float shootingForce = UnityEngine.Random.Range(minForce, maxForce * 0.5f);

        if (closestBall != null)
        {
            BallBehavior ballScript = closestBall.GetComponent<BallBehavior>();
            Vector3 closestBallToTarget = closestBall.transform.position - target.transform.position;
            
            if (ballScript.BelongsToPlayer() && closestBallToTarget.magnitude < shootingThreshold)
                isPointing = UnityEngine.Random.Range(0, 2);

            if (isPointing == 0)
            {
                shootingDir = closestBall.transform.position - transform.position;
                shootingForce *= 2.0f;
            }
        }


        ballScript.Shoot(shootingDir, shootingForce, maxOffset);
    }


    public int GetBallCounter()
    {
        return ballCounter;
    }

    public bool HasBallInHand()
    {
        return ballInHand;
    }

    public bool BallStopped()
    {
        return ballScript.Stopped();
    }

    public void Activate(GameObject _closestBall)
    {
        activated = true;
        closestBall = _closestBall;
        if (ballCounter > 0)
            SpawnBall();

    }

    public void Deactivate()
    {
        activated = false;
    }

    public void NewRound()
    {
        ballCounter = 3;
        closestBall = null;
        ballInHand = false;
        activated = false;
    }
}
