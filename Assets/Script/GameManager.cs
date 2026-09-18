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
        // บังคับให้ GameManager ของ Scene ปัจจุบันเป็น instance หลักเสมอ
        instance = this;
    }

    void Start()
    {
        // ป้องกันกรณี Scene ก่อนหน้าถูกหยุดไว้
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

        // ซ่อนปุ่ม 1 และ 2 ตอนเริ่มเกม
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

    // =========================
    // START GAME
    // =========================
    public void StartGame()
    {
        isPlaying = true;

        if (readyPanel != null)
            readyPanel.SetActive(false);
    }

    // =========================
    // SCORE
    // =========================
    public void AddScore()
    {
        score++;

        if (showScoreLog)
            Debug.Log("SCORE = " + score);

        UpdateScoreText();
    }

    // =========================
    // GAME OVER
    // =========================
    public void GameOver()
    {
        isPlaying = false;
        isGameOver = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // หยุดเกม/การเคลื่อนที่ทั้งหมดทันทีที่ชน
        Time.timeScale = 0f;
    }

    // =========================
    // STOP / RESUME
    // =========================
    public void StopGame()
    {
        if (!isPaused)
        {
            // หยุดเกม
            isPaused = true;
            Time.timeScale = 0f;

            // แสดงปุ่ม 1 และ 2
            if (button1 != null)
                button1.SetActive(true);

            if (button2 != null)
                button2.SetActive(true);

            Debug.Log("GAME PAUSED");
        }
        else
        {
            // เล่นต่อ
            isPaused = false;
            Time.timeScale = 1f;

            // ซ่อนปุ่ม 1 และ 2
            if (button1 != null)
                button1.SetActive(false);

            if (button2 != null)
                button2.SetActive(false);

            Debug.Log("GAME RESUMED");
        }
    }

    // =========================
    // กลับ MainMenu
    // =========================
    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;

        SceneManager.LoadScene("MainMenu");
    }

    // =========================
    // RESTART
    // =========================
    public void Restart()
    {
        Time.timeScale = 1f;
        isPaused = false;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    // =========================
    // UPDATE SCORE TEXT
    // =========================
    void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }
}
