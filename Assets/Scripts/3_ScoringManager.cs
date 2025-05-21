using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoringManager : MonoBehaviour
{
    //public GameObject scoringMarkerPrefab;
    public GameObject target;

    public GameObject scoringMarker;
    private GameObject scoringBall;

    private GameObject[] listBalls;

    [SerializeField] TextMeshProUGUI pointsText;

    private bool playerHasPoint = false; // flag to indicate who is currently leading

    // Start is called before the first frame update
    void Start()
    {
        scoringMarker.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(scoringMarker.activeInHierarchy)
            scoringMarker.transform.position = scoringBall.transform.position + new Vector3(0.12f, 1.0f, 0.12f);
    }

    public void calculateScore()
    {
        float minDist = 9999.9f;

        listBalls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject ball in listBalls)
        {
            float distToTarget = (ball.transform.position - target.transform.position).magnitude;
            if (distToTarget < minDist)
            {
                minDist = distToTarget;
                scoringBall = ball;
            }
        }

        BallBehavior ballScript = scoringBall.GetComponent<BallBehavior>();
        if (ballScript.belongsToPlayer())
        {
            pointsText.SetText("Player: 1 point");
            playerHasPoint = true;
        }
        else
        {
            pointsText.SetText("Opponent: 1 point");
            playerHasPoint = false;
        }

        scoringMarker.SetActive(true);
    }

    public bool getPlayerHasPoint()
    {
        return playerHasPoint;
    }
}
