/*********************************************************************************************************************
 *
 * 3_ScoringManager.cs
 * 
 * Unity_demo
 * Scene 3_playground
 * 
 * Ludovic Blache
 *
 *********************************************************************************************************************/

using System;
using TMPro;
using UnityEngine;

// Handles points calculation
public class ScoringManager : MonoBehaviour
{
    public GameObject scoringMarkerPrefab;
    public GameObject target;

    private GameObject[] listBalls;
    private GameObject[] scoringMarkers;
    private GameObject closestBall = null;

    [SerializeField] TextMeshProUGUI pointsText;

    private bool playerHasPoint = false; // flag to indicate who is currently leading
    private int playerPtsCount = 0;
    private int opponentPtsCount = 0;

    void Awake()
    {
        closestBall = null;
        playerHasPoint = false;
        playerPtsCount = 0;
        opponentPtsCount = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        scoringMarkers = new GameObject[3];
        for (int i = 0; i < scoringMarkers.GetLength(0); i++)
        {
            scoringMarkers[i] = Instantiate(scoringMarkerPrefab);
            scoringMarkers[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < scoringMarkers.GetLength(0); i++)
        {
            if (scoringMarkers[i].activeInHierarchy)
                scoringMarkers[i].transform.position = listBalls[i].transform.position + new Vector3(0.12f, 1.0f, 0.12f);
        }
    }

    public void CalculateScore()
    {
        listBalls = GameObject.FindGameObjectsWithTag("Ball");

        int nbBallsInGame = listBalls.GetLength(0);

        if (nbBallsInGame == 0)
        {
            Debug.Log("CANNOT CALCULATE SCORE IN EMPTY SCENE");
            return;
        }

        double[] listDistances = new double[nbBallsInGame];

        // for each ball in the game ...
        for (int i=0; i< nbBallsInGame; i++)
        {
            // calculate distance between current ball and target, and store it in listDistances
            GameObject ball = listBalls[i];
            float distToTarget = (ball.transform.position - target.transform.position).magnitude;
            listDistances[i] = distToTarget;
        }

        // sort listBalls using values from listDistances
        Array.Sort(listDistances, listBalls);

        closestBall = listBalls[0];


        // counters for points
        playerPtsCount = 0;
        opponentPtsCount = 0;
        // for each ball in the game ...
        foreach (GameObject ball in listBalls)
        {
            BallBehavior ballScript = ball.GetComponent<BallBehavior>();

            // sum nb of closest balls for each player
            if (ballScript.BelongsToPlayer())
            {
                if (opponentPtsCount > 0)
                {
                    // early escape as soon as player's ball are not closest anymore
                    break;
                }
                playerPtsCount++;
                playerHasPoint = true;
            }
            else
            {
                if (playerPtsCount > 0)
                {
                    break;
                }
                opponentPtsCount++;
                playerHasPoint = false;
            }
        }

        if (playerHasPoint)
        {
            string text = "You have:\n" + playerPtsCount + " point";
            if (playerPtsCount > 1)
                text += "s";
            pointsText.SetText(text);
            playerHasPoint = true;
            for (int i = 0, j = 0; i < scoringMarkers.GetLength(0); i++, j++)
            {
                scoringMarkers[i].SetActive(false);
                if (j < playerPtsCount)
                    scoringMarkers[j].SetActive(true);
            }
        }
        else
        {
            string text = "Computer has:\n" + opponentPtsCount + " point";
            if (opponentPtsCount > 1)
                text += "s";
            pointsText.SetText(text);
            playerHasPoint = false;
            for (int i = 0, j = 0; i < scoringMarkers.GetLength(0); i++, j++)
            {
                scoringMarkers[i].SetActive(false);
                if(j < opponentPtsCount)
                    scoringMarkers[j].SetActive(true);
            }
        }
        
    }

    public bool GetPlayerHasPoint()
    {
        return playerHasPoint;
    }

    public GameObject GetClosestBall()
    {
        return closestBall;
    }

    public int GetPlayerScore()
    {
        CalculateScore();
        if ( playerHasPoint && playerPtsCount < 1 ||
           !playerHasPoint && playerPtsCount != 0 ||
            playerHasPoint && opponentPtsCount != 0 ||
           !playerHasPoint && opponentPtsCount < 1 )
            Debug.Log("INCONSISTENT COUNTER");
        return playerPtsCount;
    }

    public int GetOpponentScore()
    {
        CalculateScore();
        if (playerHasPoint && playerPtsCount < 1 ||
           !playerHasPoint && playerPtsCount != 0 ||
            playerHasPoint && opponentPtsCount != 0 ||
           !playerHasPoint && opponentPtsCount < 1)
            Debug.Log("INCONSISTENT COUNTER");
        return opponentPtsCount;
    }

    public void NewRound()
    {
        closestBall = null;
        playerPtsCount = 0;
        opponentPtsCount = 0;

        foreach (GameObject marker in scoringMarkers)
        {
            marker.SetActive(false);
        }
        pointsText.SetText("No points yet");
    }

    public void ReInit()
    {
        closestBall = null;
        playerPtsCount = 0;
        opponentPtsCount = 0;
        playerHasPoint = false;

        pointsText.SetText("No points yet");
    }
}
