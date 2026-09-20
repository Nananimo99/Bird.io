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

    [Header("Debug")]
    public bool showScoreLog = true;

    private bool isGameOver = false;
    private bool isWin = false;
    private bool isPaused = false;
    private int score = 0;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        Debug.Log("GameManager READY");
    }

    void Start()
    {
        Time.timeScale = 1f;

        isPlaying = false;
        isGameOver = false;
        isWin = false;
        isPaused = false;
        score = 0;

        // -------------------------
        // READY
        // -------------------------
        if (readyPanel != null)
            readyPanel.SetActive(true);
        else
            Debug.LogWarning("GameManager: READY PANEL ยังไม่ได้ใส่");

        // -------------------------
        // GAME OVER
        // -------------------------
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        else
            Debug.LogWarning("GameManager: GAME OVER PANEL ยังไม่ได้ใส่");

        // -------------------------
        // WIN
        // -------------------------
        if (winPanel != null)
            winPanel.SetActive(false);
        else
            Debug.LogWarning("GameManager: WIN PANEL ยังไม่ได้ใส่");

        // -------------------------
        // PAUSE BUTTONS
        // -------------------------
        if (button1 != null)
            button1.SetActive(false);

        if (button2 != null)
            button2.SetActive(false);

        UpdateScoreText();
    }

    void Update()
    {
        // กด R เพื่อ Restart ตอนแพ้หรือชนะ
        if (isGameOver || isWin)
        {
            if (Keyboard.current != null &&
                Keyboard.current.rKey.wasPressedThisFrame)
            {
                Restart();
            }

            return;
        }
    }

    // ==================================================
    // START GAME
    // ==================================================
    public void StartGame()
    {
        if (isGameOver || isWin)
            return;

        isPlaying = true;

        if (readyPanel != null)
            readyPanel.SetActive(false);

        Debug.Log("GAME START");
    }

    // ==================================================
    // SCORE
    // ==================================================
    public void AddScore()
    {
        if (isGameOver || isWin)
            return;

        score++;

        if (showScoreLog)
            Debug.Log("SCORE = " + score);

        UpdateScoreText();
    }

    // ==================================================
    // GAME OVER
    // ==================================================
    public void GameOver()
    {
        if (isGameOver || isWin)
            return;

        Debug.Log("GAME OVER CALLED");

        isPlaying = false;
        isGameOver = true;

        // ปิด Ready
        if (readyPanel != null)
            readyPanel.SetActive(false);

        // เปิด Game Over
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Debug.Log("Game Over Panel เปิดแล้ว");
        }
        else
        {
            Debug.LogError(
                "GAME OVER ไม่ขึ้น เพราะยังไม่ได้ใส่ Game Over Panel ใน GameManager!"
            );
        }

        // ซ่อน Pause
        if (button1 != null)
            button1.SetActive(false);

        if (button2 != null)
            button2.SetActive(false);

        Time.timeScale = 0f;
    }

    // ==================================================
    // GAME WIN
    // ==================================================
    public void GameWin()
    {
        if (isGameOver || isWin)
            return;

        Debug.Log("GAME WIN CALLED");

        isPlaying = false;
        isWin = true;

        // ปิด Ready
        if (readyPanel != null)
            readyPanel.SetActive(false);

        // ปิด Game Over
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // เปิด Win Panel
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            Debug.Log("WIN PANEL เปิดแล้ว");
        }
        else
        {
            Debug.LogError(
                "WIN ไม่ขึ้น เพราะยังไม่ได้ใส่ Win Panel ใน GameManager!"
            );
        }

        // ซ่อน Pause
        if (button1 != null)
            button1.SetActive(false);

        if (button2 != null)
            button2.SetActive(false);

        // หยุดเกม
        Time.timeScale = 0f;
    }

    // ==================================================
    // PAUSE / STOP
    // ==================================================
    public void StopGame()
    {
        if (isGameOver || isWin)
            return;

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

    // ==================================================
    // BACK TO MAIN MENU
    // ==================================================
    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;

        SceneManager.LoadScene("MainMenu");
    }

    // ==================================================
    // RESTART
    // ==================================================
    public void Restart()
    {
        Time.timeScale = 1f;
        isPaused = false;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    // ==================================================
    // SCORE TEXT
    // ==================================================
    void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }
}

