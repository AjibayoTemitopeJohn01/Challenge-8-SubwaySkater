using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int CoinScoreAmt = 5;
    public static GameManager Instance
    {
        set;
        get;
    }

    public bool IsDead
    {
        set;
        get;
    }
    private bool isGameStarted = false;
    private PlayerMotor motor;
    
    // UI & UI Fields
    [Header("Game Canvas")]
    public Animator gameCanvas;
    [Header("Score")]
    public Text scoreText;
    [Header("Coins")]
    public Text coinText;
    [Header("Multiplier")]
    public Text modifierText;
    [Header("High Score")]
    public Text highScoreTxt;
    
    // Game Over Menu
    [Header("Game Over Panel")]
    public Animator gameOverAnim;
    public Text gameOverScore;
    public Text gameOverCoins;

    private float score;
    private float coinScore;
    private float modifierScore;
    private int lastScore;

    private void Awake()
    {
        Instance = this;
        modifierScore = 1;
        motor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        
        modifierText.text = "x" + modifierScore.ToString("0.0");
        coinText.text = coinScore.ToString("0");
        //scoreText.text = scoreText.text = score.ToString("0");
        scoreText.text = scoreText.text = scoreText.text = "SCORE " + score.ToString("0");
    }

    private void Update()
    {
        if (MobileInput.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            motor.StartRunning();
            FindObjectOfType<GlacierSpawner>().IsScrolling = true; //

            FindObjectOfType<CameraMotor>().IsMoving = true;
            gameCanvas.SetTrigger("Show");

            //highScoreTxt.text = PlayerPrefs.GetInt("HighScore").ToString();
        }

        // Give Score to Player for continued running
        if (isGameStarted && !IsDead)
        {
            // Bump the Score
            score += (Time.deltaTime * modifierScore);
            if (lastScore != (int)score)
            {
                lastScore = (int)score;
                scoreText.text = "SCORE " + score.ToString("0");
            }
        }
    }

    public void GetCoins()
    {
        coinScore++;
        coinText.text = coinScore.ToString("0");
        score += CoinScoreAmt;
        scoreText.text = scoreText.text = score.ToString("0");
    }

    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 1.0f + modifierAmount;
        modifierText.text = "x" + modifierScore.ToString("0.0");
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void GameOver()
    {
        FindObjectOfType<AudioManager>().gameOver.Play();
        IsDead = true;
        FindObjectOfType<GlacierSpawner>().IsScrolling = false; //
        gameOverScore.text = "HIGH SCORE " + PlayerPrefs.GetInt("HighScore").ToString();
        gameOverCoins.text = "COINS " + coinScore.ToString("0");
        gameOverAnim.SetTrigger("gameOver");
        //highScoreTxt.text = PlayerPrefs.GetInt("HighScore").ToString();
        
        // Check if this is a High Score
        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            var s = score;
            if (s % 1 == 0)
                s += 1;
            PlayerPrefs.SetInt("HighScore", (int)s);
        }
        PlayerPrefs.SetInt("Coin",(int)coinScore);
    }

    public void Menu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
