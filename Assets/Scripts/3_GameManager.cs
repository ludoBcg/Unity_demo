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
    private bool roundFinished = false;  // flag if current round is finished or not


    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<Player>();
        opponentScript = opponent.GetComponent<OpponentBehavior>();
        scoringScript = scoreManager.GetComponent<ScoringManager>();
        targetScript = target.GetComponent<TargetBehavior>();
    }

    void FixedUpdate()
    {

    }

    void Update()
    {

    }

    void LateUpdate()
    {
        if (!roundFinished)
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

            // activate player or opponent depending on score
            if (turnFinished)
            {
                beforeShot();

                if (scoringScript.getPlayerHasPoint())
                {
                    // if player has the point...

                    if (opponentScript.getBallCounter() > 0)
                    {
                        // if opponent still has balls, let it play
                        playerScript.deactivate();
                        opponentScript.activate();
                    }
                    else if (playerScript.getBallCounter() > 0)
                    {
                        // if not, player continues as lon as it can
                        opponentScript.deactivate();
                        playerScript.activate();
                    }
                }
                else if (!scoringScript.getPlayerHasPoint())
                {
                    // if opponent has the point...

                    if (playerScript.getBallCounter() > 0)
                    {
                        // if player still has balls, let it play
                        opponentScript.deactivate();
                        playerScript.activate();
                    }
                    else if(opponentScript.getBallCounter() > 0)
                    {
                        // if not, opponent continues as long as it can
                        playerScript.deactivate();
                        opponentScript.activate();
                    }
                }


                afterShot();
            }
        } // end if !roundFinished
    }

    void beforeShot()
    {
        turnFinished = false;

        if (opponentScript.getBallCounter() <= 0 && playerScript.getBallCounter() <= 0)
        {
            Debug.Log("round finished");
            roundFinished = true;
        }
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
        if (playerScript.getBallInHand())
            isStill = false;


        return isStill;
    }
}
