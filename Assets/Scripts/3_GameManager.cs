using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject opponent;
    public GameObject scoreManager;
    public GameObject target;

    private Player playerScript;
    private OpponentBehavior opponentScript;
    private ScoringManager scoringScript;
    private TargetBehavior targetScript;

    private GameObject[] listBalls;

    private bool isSceneStill = true;   // flag if balls are rolling or not
    private bool turnFinished = true;   // flag if current turn is finished or not


    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<Player>();
        opponentScript = opponent.GetComponent<OpponentBehavior>();
        scoringScript = scoreManager.GetComponent<ScoringManager>();
        targetScript = target.GetComponent<TargetBehavior>();


        //opponentScript.deactivate();
        //playerScript.activate();
    }

    void FixedUpdate()
    {

    }

    void Update()
    {

    }

    void LateUpdate()
    {
        // wait for scene to be still
        if (!isSceneStill)
        {
            isSceneStill = checkStillness();
        }

        // update score
        if (isSceneStill && !turnFinished)
        {
            scoringScript.calculateScore();
            turnFinished = true;
        }

        // activate player of oponent depending on score
        if (turnFinished)
        {
            beforeShot();

            if (scoringScript.getPlayerHasPoint())
            {
                playerScript.deactivate();
                opponentScript.activate();
            }
            else
            {
                opponentScript.deactivate();
                playerScript.activate();
            }

            afterShot();
        }
    }

    void beforeShot()
    {
        turnFinished = false;
    }

    void afterShot()
    {
        // update list of balls in scene
        listBalls = GameObject.FindGameObjectsWithTag("Ball");

        isSceneStill = false;
    }

    bool checkStillness()
    {
        bool isStill = true;

        // for each ball in the scene
        foreach (GameObject ball in listBalls)
        {
            // check if it is still
            BallBehavior ballScript = ball.GetComponent<BallBehavior>();
            bool currentBallStill = ballScript.stopped();

            // if one ball is checkStillness moving, scene is not still
            if(!currentBallStill)
                isStill = false;
        }

        // check if target is also still
        if(!targetScript.stopped())
            isStill = false;

        // check if player does not have ball in hand
        if (playerScript.getballInHand())
            isStill = false;


        return isStill;
    }
}
