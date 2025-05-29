using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class ScoringManager : MonoBehaviour
{
    public GameObject scoringMarkerPrefab;
    public GameObject target;

    private GameObject[] listBalls;
    private GameObject[] scoringMarkers;

    [SerializeField] TextMeshProUGUI pointsText;

    private bool playerHasPoint = false; // flag to indicate who is currently leading

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

    public void calculateScore()
    {
        listBalls = GameObject.FindGameObjectsWithTag("Ball");

        int nbBallsInGame = listBalls.GetLength(0);
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


        // counters for points
        int playerPtsCount = 0;
        int opponentPtsCount = 0;
        // for each ball in the game ...
        foreach (GameObject ball in listBalls)
        {
            BallBehavior ballScriptX = ball.GetComponent<BallBehavior>();

            // sum nb of closest balls for each player
            if (ballScriptX.belongsToPlayer())
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
            pointsText.SetText("Player: " + playerPtsCount  + " point");
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
            pointsText.SetText("Opponent: " + opponentPtsCount + " point");
            playerHasPoint = false;
            for (int i = 0, j = 0; i < scoringMarkers.GetLength(0); i++, j++)
            {
                scoringMarkers[i].SetActive(false);
                if(j < opponentPtsCount)
                    scoringMarkers[j].SetActive(true);
            }
        }
        
    }

    public bool getPlayerHasPoint()
    {
        return playerHasPoint;
    }
}
