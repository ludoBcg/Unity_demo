using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringManager : MonoBehaviour
{
    //public GameObject scoringMarkerPrefab;
    public GameObject target;

    public GameObject scoringMarker;
    private GameObject scoringBall;

    private GameObject[] listBalls;

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

        scoringMarker.SetActive(true);
    }
}
