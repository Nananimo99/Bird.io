using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI")]
    public Text scoreText;
    public GameObject readyPanel;
    public GameObject gameOverPanel;
    public GameObject winPanel;

    [Header("Pause UI")]
    public GameObject button1;
    public GameObject button2;

    [Header("State")]
    public bool isPlaying = false;

    [Header("Win Condition")]
    public int winScore = 0;

    [Header("Debug")]
    public bool showScoreLog = true;

    private bool isGameOver;
    private int score;
    private bool isPaused = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;

        isPlaying = false;
        isGameOver = false;
        isPaused = false;
        score = 0;

        if (scoreText == null)
            Debug.LogWarning("GameManager: Score Text is not assigned.");

        if (readyPanel != null)
            readyPanel.SetActive(true);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (winPanel != null)
            winPanel.SetActive(false);

        if (button1 != null)
            button1.SetActive(false);

        if (button2 != null)
            button2.SetActive(false);

        UpdateScoreText();
    }

    void Update()
    {
        if (!isGameOver) return;

        if (Keyboard.current != null &&
            Keyboard.current.rKey.wasPressedThisFrame)
        {
            Restart();
        }
    }

    public void StartGame()
    {
        isPlaying = true;

        if (readyPanel != null)
            readyPanel.SetActive(false);
    }

    public void AddScore()
    {
        score++;

        if (showScoreLog)
            Debug.Log("SCORE = " + score);

        UpdateScoreText();

        if (winScore > 0 && score >= winScore) GameWin();
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isPlaying = false;
        isGameOver = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void GameWin()
    {
        if (isGameOver) return;

        isPlaying = false;
        isGameOver = true;

        if (winPanel != null)
            winPanel.SetActive(true);

        Debug.Log("YOU WIN!");

        Time.timeScale = 0f;
    }

    public void StopGame()
    {
        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0f;

            if (button1 != null)
                button1.SetActive(true);

            if (button2 != null)
                button2.SetActive(true);
        }
        else
        {
            isPaused = false;
            Time.timeScale = 1f;

            if (button1 != null)
                button1.SetActive(false);

            if (button2 != null)
                button2.SetActive(false);
        }
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;

        SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        isPaused = false;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }
}