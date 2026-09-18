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
    public GameObject winPanel; // หน้าจอชนะ

    [Header("Pause UI")]
    public GameObject button1; // กลับ MainMenu
    public GameObject button2; // เริ่มใหม่

    [Header("State")]
    public bool isPlaying = false;

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
    }

    public void GameOver()
    {
        isPlaying = false;
        isGameOver = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    // =========================
    // GAME WIN (ฟังก์ชันชนะ)
    // =========================
    public void GameWin()
    {
        isPlaying = false;
        Time.timeScale = 0f; // หยุดเกมทันที

        if (winPanel != null)
            winPanel.SetActive(true);

        Debug.Log("YOU WIN!");
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
