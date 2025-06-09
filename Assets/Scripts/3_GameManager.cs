using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject opponent;
    public GameObject scoreManager;
    public GameObject target;
    public GameObject mainCamera;
    public int pointsToWin = 3;  // nb of points to reach to win

    [SerializeField] TextMeshProUGUI messageBoxText;
    [SerializeField] TextMeshProUGUI playerScoreText;
    [SerializeField] TextMeshProUGUI opponentScoreText;
    [SerializeField] TextMeshProUGUI pointsToWinText;

    public GameObject startPanel;
    [SerializeField] TextMeshProUGUI startMsgText;
    [SerializeField] TextMeshProUGUI infoMsgText;
    public Button startButton;
    public Button restartButton;

    private Player playerScript;
    private OpponentBehavior opponentScript;
    private ScoringManager scoringScript;
    private TargetBehavior targetScript;
    private CameraController cameraScript;

    private GameObject[] listBalls;

    private bool isSceneStill = true;    // flag if balls are rolling or not
    private bool turnFinished = true;    // flag if current turn is finished or not
    private bool roundFinished = false;  // flag if current round is finished or not
    private bool playerSarts = true;     // flag if players starts or not

    private bool isGameFinished = true; // flag if game is finished or not

    private int totalPlayerPoints = 0;
    private int totalOpponentPoints = 0;

    private int timer;

    void Awake()
    {
        isGameFinished = true;
        isSceneStill = true;
        turnFinished = true;
        playerSarts = true;
        roundFinished = false;

        totalPlayerPoints = 0;
        totalOpponentPoints = 0;
}

    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<Player>();
        opponentScript = opponent.GetComponent<OpponentBehavior>();
        scoringScript = scoreManager.GetComponent<ScoringManager>();
        targetScript = target.GetComponent<TargetBehavior>();
        cameraScript = mainCamera.GetComponent<CameraController>();

        cameraScript.gameStarted(true);
        pointsToWinText.SetText(pointsToWin + " points to win");
    }

    void FixedUpdate()
    {

    }

    void Update()
    {

    }

    void LateUpdate()
    {
        if (!isGameFinished)
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
                    playerSarts = !scoringScript.getPlayerHasPoint();

                    turnFinished = true;
                }

                // activate player or opponent depending on score
                if (turnFinished)
                {
                    beforeShot();

                    if (!playerSarts)
                    {
                        // if player has the point...

                        if (opponentScript.getBallCounter() > 0)
                        {
                            // if opponent still has balls, let it play
                            playerScript.deactivate();
                            opponentScript.activate(scoringScript.getClosestBall());
                        }
                        else if (playerScript.getBallCounter() > 0)
                        {
                            // if not, player continues as long as it can
                            opponentScript.deactivate();
                            playerScript.activate();
                        }
                    }
                    else
                    {
                        // if opponent has the point...

                        if (playerScript.getBallCounter() > 0)
                        {
                            // if player still has balls, let it play
                            opponentScript.deactivate();
                            playerScript.activate();
                        }
                        else if (opponentScript.getBallCounter() > 0)
                        {
                            // if not, opponent continues as long as it can
                            playerScript.deactivate();
                            opponentScript.activate(scoringScript.getClosestBall());
                        }

                    }


                    afterShot();
                }
            } // end if !roundFinished
            else
            {
                isGameFinished = true;

                startNewRound();

                timer = 5;
                StartCoroutine(Countdown());
                //startNewRound();
            }
        } // end if !gameFinished
    }

    void beforeShot()
    {
        turnFinished = false;

        if (opponentScript.getBallCounter() <= 0 && playerScript.getBallCounter() <= 0)
        {
            messageBoxText.SetText("Round finished");
            messageBoxText.gameObject.SetActive(true);
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

        
        if (listBalls.Length >= 0)
        {
            // for each ball in the scene
            foreach (GameObject ball in listBalls)
            {
                // check if it is still
                BallBehavior ballScript = ball.GetComponent<BallBehavior>();
                bool currentBallStill = ballScript.stopped();

                // if one ball is checkStillness moving, scene is not still
                if (!currentBallStill)
                    isStill = false;
            }

            // check if target is also still
            if (!targetScript.stopped())
                isStill = false;

            // check if player does not have ball in hand
            if (playerScript.getBallInHand())
                isStill = false;

        }
        return isStill;
    }

    void startNewRound()
    {
        totalPlayerPoints += scoringScript.getPlayerScore();
        totalOpponentPoints += scoringScript.getOpponentScore();

        playerScoreText.SetText("Player score: " + totalPlayerPoints);
        opponentScoreText.SetText("Opponent score: " + totalOpponentPoints);

        if (totalPlayerPoints >= pointsToWin || totalOpponentPoints >= pointsToWin)
        {
            endGame();

            // early exit
            return;
        }

        playerScript.newRound();
        opponentScript.newRound();
        scoringScript.newRound();

        listBalls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in listBalls)
        {
            Object.Destroy(ball);
        }

        messageBoxText.SetText("New round");
        messageBoxText.gameObject.SetActive(true);
        targetScript.reInit();
        cameraScript.reInit();
        playerSarts = scoringScript.getPlayerHasPoint();
        isSceneStill = true;
        roundFinished = false;
        turnFinished = true;

    }


    void endGame()
    {
        if (totalPlayerPoints >= pointsToWin)
        {
            isGameFinished = true;
            startMsgText.SetText("You win!");
        }
        else if (totalOpponentPoints >= pointsToWin)
        {
            isGameFinished = true;
            startMsgText.SetText("You loose!");
        }
        else
            Debug.Log("INCONSISTENT ENDGAME");

        cameraScript.gameStarted(false);
        messageBoxText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(true);
        startPanel.SetActive(true);
    }

    public void StartGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        cameraScript.gameStarted(true);
        startButton.gameObject.SetActive(false);
        infoMsgText.gameObject.SetActive(false);
        startPanel.SetActive(false);
        isGameFinished = false;
    }

    public void RestartGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        playerScoreText.SetText("Player score: " + 0);
        opponentScoreText.SetText("Opponent score: " + 0);

        playerScript.newRound();
        opponentScript.newRound();
        scoringScript.newRound();

        listBalls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in listBalls)
        {
            Object.Destroy(ball);
        }

        targetScript.reInit();
        cameraScript.reInit();
 

        Awake();

        restartButton.gameObject.SetActive(false);
        startPanel.SetActive(false);

        
        Start();
        StartGame();
    }


    IEnumerator Countdown()
    {
        while (timer > 0)
        {
            yield return new WaitForSeconds(1);
            timer -= 1;
            //Debug.Log("countdown");
            if (timer == 0)
            {
                isGameFinished = false;
            }
        }
    }
}
