/*********************************************************************************************************************
 *
 * 3_GameManager.cs
 * 
 * Unity_demo
 * Scene 3_playground
 * 
 * Ludovic Blache
 *
 *********************************************************************************************************************/


using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

// Handles main game sequence
public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject opponent;
    public GameObject scoreManager;
    public GameObject target;
    public GameObject mainCamera;
    public int pointsToWin = 13;

    [SerializeField] TextMeshProUGUI messageBoxText;
    [SerializeField] TextMeshProUGUI playerScoreText;
    [SerializeField] TextMeshProUGUI opponentScoreText;
    [SerializeField] TextMeshProUGUI pointsToWinText;

    [SerializeField] TextMeshProUGUI infoMsgText;

    public GameObject startPanel;
    [SerializeField] TextMeshProUGUI startMsgText;
    public Button startButton;
    public Button restartButton;

    private Player playerScript;
    private OpponentBehavior opponentScript;
    private ScoringManager scoringScript;
    private TargetBehavior targetScript;
    private CameraController cameraScript;

    private GameObject[] listBalls;


    private int totalPlayerPoints = 0;
    private int totalOpponentPoints = 0;

    private int timer;
  
    private bool isSceneStill = true;      // flag indicating if balls are rolling or not
    private bool isPlayersTurn = true;     // flag indicating if player or opponent is now playing
    private bool isTurnFinished = false;   // flag indicating if current turn is finished or not
    private bool isRoundFinished = false;  // flag indicating if current round is finished or not
    private bool isGameActive = false;     // flag indicating if the scene is active or not

    private bool clearScene = false;
    void Awake()
    {
        clearScene = false;

        listBalls = new GameObject[0];

        totalPlayerPoints = 0;
        totalOpponentPoints = 0;

        isSceneStill = true;
        isTurnFinished = false;
        isRoundFinished = false; 

    }

    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<Player>();
        opponentScript = opponent.GetComponent<OpponentBehavior>();
        scoringScript = scoreManager.GetComponent<ScoringManager>();
        targetScript = target.GetComponent<TargetBehavior>();
        cameraScript = mainCamera.GetComponent<CameraController>();

        pointsToWinText.SetText(pointsToWin + " points to win");

        PauseGame();
    }

    void FixedUpdate()
    {
        if (isGameActive)
        {
            if (clearScene && isRoundFinished)
            {
                ResetScene();
                clearScene = false;
                isRoundFinished = false;
            }
        }
    }

    void Update()
    {

    }

    void LateUpdate()
    {
        if (isGameActive && !clearScene)
        {
            if (isRoundFinished)
            {
                NewRound();
                clearScene = true;
            }
            else
            {
                if (isTurnFinished)
                {
                    if (opponentScript.GetBallCounter() <= 0 && playerScript.GetBallCounter() <= 0)
                    {
                        isRoundFinished = true;
                        return;
                    }

                    isTurnFinished = false;
                    // ... to calculate score and decide who plays next
                    NewTurn();

                    // Activate player or opponent for next turn depending on score
                    if (isPlayersTurn)
                    {
                        opponentScript.Deactivate();
                        playerScript.Activate();
                        infoMsgText.SetText("You play");
                    }
                    else
                    {
                        playerScript.Deactivate();
                        opponentScript.Activate(scoringScript.GetClosestBall());
                        infoMsgText.SetText("Computer plays");
                    }

                    AfterActivation();
                } // end if isTurnFinished


                if (!isSceneStill)
                {
                    isTurnFinished = false;
                    // Wait for scene to be still ...
                    isSceneStill = CheckStillness();
                }
                else
                    isTurnFinished = true;

            } // end if !isRoundFinished
            
        } // end if !gameFinished
    }

    void AfterActivation()
    {
        // update list of balls in scene
        UpdateListBalls();

        // Scene is not still anymore as-soon-as player or opponent has been activated
        isSceneStill = false;
    }

    bool CheckStillness()
    {
        bool isStill = true;

        if (listBalls.Length > 0)
        {
            // for each ball in the scene
            foreach (GameObject ball in listBalls)
            {
                // check if it is still
                BallBehavior ballScript = ball.GetComponent<BallBehavior>();
                bool currentBallStill = ballScript.Stopped();

                // if one ball is checkStillness moving, scene is not still
                if (!currentBallStill)
                    isStill = false;
            }

            // check if target is also still
            if (!targetScript.Stopped())
                isStill = false;

            // check if player does not have ball in hand
            if (playerScript.GetBallInHand())
                isStill = false;

        }
        return isStill;
    }

    void EndGame()
    {
        if (totalPlayerPoints >= pointsToWin)
            startMsgText.SetText("You win!");
        else if (totalOpponentPoints >= pointsToWin)
            startMsgText.SetText("You loose!");
        else
            Debug.Log("INCONSISTENT ENDGAME");

        PauseGame();
        restartButton.gameObject.SetActive(true);
        startPanel.SetActive(true);
    }

    public void StartGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        NewGame();

        startButton.gameObject.SetActive(false);
        startPanel.SetActive(false);

        string text = "Game starting ...\n";
        if (isPlayersTurn)
            text += "You play first";
        else
            text += "Computer plays first";
        infoMsgText.SetText(text);

        timer = 5;
        StartCoroutine(Countdown());
    }

    public void RestartGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        playerScoreText.SetText("Your score: " + 0);
        opponentScoreText.SetText("Computer score: " + 0);

        ResetScene();

        Awake();

        restartButton.gameObject.SetActive(false);
        startPanel.SetActive(false);

        Start();
        
        StartGame();
    }

    void ResetScene()
    {
        playerScript.NewRound();
        opponentScript.NewRound();
        scoringScript.NewRound();

        // Remove balls from the game
        UpdateListBalls();
        foreach (GameObject ball in listBalls)
        {
            Object.Destroy(ball);
        }
        listBalls = new GameObject[0];

        targetScript.ReInit();
        cameraScript.ReInit();
    }

    // Starts fresh new game
    public void NewGame()
    {
        // Player or opponent randomly starts the game
        int playerStarts = UnityEngine.Random.Range(0, 2);
        isPlayersTurn = playerStarts == 0 ? false : true;
    }

    // Starts new round when all balls have been used
    public void NewRound()
    {
        PauseGame();

        // Update scores
        totalPlayerPoints += scoringScript.GetPlayerScore();
        totalOpponentPoints += scoringScript.GetOpponentScore();
        playerScoreText.SetText("Your score: " + totalPlayerPoints);
        opponentScoreText.SetText("Computer score: " + totalOpponentPoints);

        string text = "";
        int points = Mathf.Max(scoringScript.GetPlayerScore(), scoringScript.GetOpponentScore());
        
        if (points == scoringScript.GetPlayerScore())
        {
            text += "You get " + scoringScript.GetPlayerScore() + " point";
        }
        else
        {
            text += "Computer gets " + scoringScript.GetOpponentScore() + " point";
        }

        if (points > 1)
            text += "s";

        infoMsgText.SetText(text);

        if (totalPlayerPoints >= pointsToWin || totalOpponentPoints >= pointsToWin)
        {
            // Game is finished when pointsToWin has been reached
            EndGame();
            // Early exit
            return;
        }

        text += "\nNew round starting ...";
        infoMsgText.SetText(text);

        // if game is not finished, prepare scene for a new round
        isSceneStill = true;

        // Whoever won the last round starts the next one
        isPlayersTurn = scoringScript.GetPlayerHasPoint();

        //Debug.Log("newRound(): isPlayersTurn = " + isPlayersTurn);

        timer = 5;
        StartCoroutine(Countdown());

        //ResetScene();
    }

    // Starts new shooting sequence in the current round
    public void NewTurn()
    {
        if (listBalls.Length > 0)
        {
            // calculates score
            scoringScript.CalculateScore();

            // Defines who currently gets the point
            bool playerHasPoint = scoringScript.GetPlayerHasPoint();

            if (playerHasPoint)
            {
                // case 1: player has the point
                if (opponentScript.GetBallCounter() > 0)
                {
                    // if opponent still has balls, let it play
                    isPlayersTurn = false;
                }
                else if (playerScript.GetBallCounter() > 0)
                {
                    // if not, player continues as long as it can
                    isPlayersTurn = true;
                }
            }
            else
            {
                // case 1: opponent has the point
                if (playerScript.GetBallCounter() > 0)
                {
                    // if player still has balls, let it play
                    isPlayersTurn = true;
                }
                else if (opponentScript.GetBallCounter() > 0)
                {
                    // if not, opponent continues as long as it can
                    isPlayersTurn = false;
                }
            }
        }

        //Debug.Log("newTurn(): isPlayersTurn = " + isPlayersTurn);
    }


    IEnumerator Countdown()
    {
        while (timer > 0 && !isGameActive)
        {
            yield return new WaitForSeconds(1);
            timer -= 1;

            if (timer == 0)
            {
                ResumeGame();
                //isRoundFinished = false;
            }
        }
    }

    void UpdateListBalls()
    {
        listBalls = GameObject.FindGameObjectsWithTag("Ball");
    }

    void PauseGame()
    {
        isGameActive = false;
        cameraScript.GamePaused(false);
    }

    void ResumeGame()
    {
        cameraScript.GamePaused(true);
        isGameActive = true;
    }
}
