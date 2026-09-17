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

    [Header("State")]
    public bool isPlaying = false;

    [Header("Debug")]
    public bool showScoreLog = true;

    private bool isGameOver;
    private int score;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("GameManager: duplicate found on " + gameObject.name + ", destroying it.");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Start()
    {
        isPlaying = false;
        isGameOver = false;
        score = 0;

        if (scoreText == null) Debug.LogWarning("GameManager: Score Text is not assigned.");

        if (readyPanel != null) readyPanel.SetActive(true);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        UpdateScoreText();
    }

    void Update()
    {
        if (!isGameOver) return;
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame) Restart();
    }

    public void StartGame()
    {
        isPlaying = true;
        if (readyPanel != null) readyPanel.SetActive(false);
    }

    public void AddScore()
    {
        score++;
        if (showScoreLog) Debug.Log("SCORE = " + score);
        UpdateScoreText();
    }

    public void GameOver()
    {
        isPlaying = false;
        isGameOver = true;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateScoreText()
    {
        if (scoreText != null) scoreText.text = score.ToString();
    }
}
