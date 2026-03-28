using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool gameEnded;
    private bool isPaused;
    private float levelStartTime;
    private float pausedTime;

    public float ElapsedTime { get { return (isPaused ? pausedTime : Time.time) - levelStartTime; } }
    public bool GameEnded { get { return gameEnded; } }
    public bool IsPaused { get { return isPaused; } }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        gameEnded = false;
        isPaused = false;
        levelStartTime = Time.time;
    }

    public void Pause()
    {
        if (gameEnded || isPaused) return;
        isPaused = true;
        pausedTime = Time.time;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        if (!isPaused) return;
        levelStartTime += Time.time - pausedTime;
        isPaused = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void GameOver()
    {
        if (gameEnded) return;
        gameEnded = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UIManager ui = FindObjectOfType<UIManager>();
        if (ui != null) ui.ShowGameOver();
    }

    public void LevelClear()
    {
        if (gameEnded) return;
        gameEnded = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UIManager ui = FindObjectOfType<UIManager>();
        if (ui != null) ui.ShowStageClear();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
