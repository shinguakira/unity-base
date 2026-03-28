using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private int score;
    private int comboCount;
    private float lastScoreTime;
    private float comboWindow = 3f;
    private int totalComboBonus;

    public int Score { get { return score; } }
    public int ComboCount { get { return comboCount; } }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterScore(int points)
    {
        score += points;

        if (Time.time - lastScoreTime < comboWindow)
        {
            comboCount++;
            totalComboBonus += comboCount * 50;
        }
        else
        {
            comboCount = 1;
        }
        lastScoreTime = Time.time;
    }

    public int GetTotalScore()
    {
        return score + totalComboBonus;
    }

    public void ResetScore()
    {
        score = 0;
        comboCount = 0;
        totalComboBonus = 0;
        lastScoreTime = 0;
    }
}
