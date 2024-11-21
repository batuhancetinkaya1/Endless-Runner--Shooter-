using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreManager : Singleton<ScoreManager>
{
    public static event System.Action<int> OnScoreChanged;
    public static event System.Action<int> OnHighScoreChanged;

    [System.Serializable]
    public class HighScoreDataClass // private yerine public yapýldý
    {
        public int Score;
        public int Coins;
        public float Distance;
        public int Blocks;
    }

    private HighScoreDataClass highScoreData = new HighScoreDataClass();
    public HighScoreDataClass HighScoreData => highScoreData;

    // ... geri kalan kod ayný ...
    private const int MAX_HIGH_SCORES = 5;
    private const string HIGH_SCORE_KEY = "HighScore";
    private float lastScoredDistance = 0f;
    private const float DISTANCE_SCORE_INTERVAL = 1f;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI highScoreCoinsText;
    [SerializeField] private TextMeshProUGUI highScoreDistanceText;
    [SerializeField] private TextMeshProUGUI highScoreBlocksText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI finalCoinsText;
    [SerializeField] private TextMeshProUGUI finalDistanceText;
    [SerializeField] private TextMeshProUGUI finalBlocksText;

    public enum ScoreType
    {
        Standard,
        Coin,
        BlockDestruction,
        TimeBonus,
        Distance
    }

    [System.Serializable]
    public class GameStats
    {
        public int CurrentScore;
        public int TotalCoinsCollected;
        public int BlocksDestroyed;
        public float DistanceTraveled;
        public float GameDuration;

        public void Reset()
        {
            CurrentScore = 0;
            TotalCoinsCollected = 0;
            BlocksDestroyed = 0;
            DistanceTraveled = 0;
            GameDuration = 0f;
        }
    }

    public GameStats Stats { get; private set; } = new GameStats();
    public int CurrentScore => Stats.CurrentScore;
    public int HighScore { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        //ResetHighScore(); //tek seferlik
        LoadHighScore();

        UpdateScoreUI();
        UpdateHighScoreUI();
    }

    private void LoadHighScore()
    {
        highScoreData.Score = PlayerPrefs.GetInt($"{HIGH_SCORE_KEY}Score", 0);
        highScoreData.Coins = PlayerPrefs.GetInt($"{HIGH_SCORE_KEY}Coins", 0);
        highScoreData.Distance = PlayerPrefs.GetFloat($"{HIGH_SCORE_KEY}Distance", 0);
        highScoreData.Blocks = PlayerPrefs.GetInt($"{HIGH_SCORE_KEY}Blocks", 0);
    }

    public void SaveHighScore()
    {
        if (Stats.CurrentScore > highScoreData.Score)
        {
            highScoreData.Score = Stats.CurrentScore;
            highScoreData.Coins = Stats.TotalCoinsCollected;
            highScoreData.Distance = Stats.DistanceTraveled;
            highScoreData.Blocks = Stats.BlocksDestroyed;

            PlayerPrefs.SetInt($"{HIGH_SCORE_KEY}Score", highScoreData.Score);
            PlayerPrefs.SetInt($"{HIGH_SCORE_KEY}Coins", highScoreData.Coins);
            PlayerPrefs.SetFloat($"{HIGH_SCORE_KEY}Distance", highScoreData.Distance);
            PlayerPrefs.SetInt($"{HIGH_SCORE_KEY}Blocks", highScoreData.Blocks);
            PlayerPrefs.Save();

            UpdateHighScoreUI();
            OnHighScoreChanged?.Invoke(highScoreData.Score);
        }
    }

    private void UpdateHighScoreUI()
    {
        if (highScoreText != null)
            highScoreText.text = $"Highscore: {highScoreData.Score}";
        if (highScoreCoinsText != null)
            highScoreCoinsText.text = $"Coins: {highScoreData.Coins}";
        if (highScoreDistanceText != null)
            highScoreDistanceText.text = $"Distance: {highScoreData.Distance:F2}m";
        if (highScoreBlocksText != null)
            highScoreBlocksText.text = $"Blocks: {highScoreData.Blocks}";
    }

    public void ResetStats()
    {
        Stats.CurrentScore = 0;
        Stats.TotalCoinsCollected = 0;
        Stats.BlocksDestroyed = 0;
        Stats.DistanceTraveled = 0f;
        Stats.GameDuration = 0f;
        lastScoredDistance = 0f;

        UpdateScoreUI();
        UpdateHighScoreUI();
        OnScoreChanged?.Invoke(0);
    }

    public void AddScore(int points, ScoreType type = ScoreType.Standard)
    {
        float multiplier = GetScoreMultiplier(type);
        int scoreToAdd = Mathf.RoundToInt(points * multiplier);
        Stats.CurrentScore += scoreToAdd;

        UpdateScoreUI();
        OnScoreChanged?.Invoke(Stats.CurrentScore);

        if (Stats.CurrentScore > HighScore)
        {
            HighScore = Stats.CurrentScore;
            UpdateHighScoreUI();
            OnHighScoreChanged?.Invoke(HighScore);
        }
    }

    private float GetScoreMultiplier(ScoreType type) => type switch
    {
        ScoreType.Coin => 1f,
        ScoreType.BlockDestruction => 2f,
        ScoreType.Distance => 0.5f,
        ScoreType.TimeBonus => 3f,
        _ => 1f
    };

    public void IncrementCoins()
    {
        Stats.TotalCoinsCollected++;
        AddScore(1, ScoreType.Coin);
    }

    public void IncrementBlocksDestroyed()
    {
        Stats.BlocksDestroyed++;
        AddScore(5, ScoreType.BlockDestruction);
    }

    public void UpdateDistanceScore(float totalDistance)
    {
        Stats.DistanceTraveled = totalDistance;

        float newIntervals = Mathf.Floor(totalDistance / DISTANCE_SCORE_INTERVAL) -
                            Mathf.Floor(lastScoredDistance / DISTANCE_SCORE_INTERVAL);

        if (newIntervals > 0)
        {
            AddScore(Mathf.RoundToInt(newIntervals * 2), ScoreType.Distance);
            lastScoredDistance = Mathf.Floor(totalDistance / DISTANCE_SCORE_INTERVAL) * DISTANCE_SCORE_INTERVAL;
        }
    }

    public List<int> GetHighScores()
    {
        List<int> highScores = new List<int>();
        for (int i = 0; i < MAX_HIGH_SCORES; i++)
        {
            highScores.Add(PlayerPrefs.GetInt($"{HIGH_SCORE_KEY}{i}", 0));
        }
        return highScores;
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {Stats.CurrentScore}";
    }

    public void UpdateDeathPanelUI()
    {
        if (finalScoreText != null)
            finalScoreText.text = $"Final Score: {Stats.CurrentScore}";
        if (finalCoinsText != null)
            finalCoinsText.text = $"Coins Collected: {Stats.TotalCoinsCollected}";
        if (finalDistanceText != null)
            finalDistanceText.text = $"Distance Traveled: {Stats.DistanceTraveled:F2}m";
        if (finalBlocksText != null)
            finalBlocksText.text = $"Blocks Destroyed: {Stats.BlocksDestroyed}";
    }

    public void ResetHighScore()
    {
        HighScore = 0;
        Stats.TotalCoinsCollected = 0;
        Stats.DistanceTraveled = 0f;
        Stats.BlocksDestroyed = 0;

        for (int i = 0; i < MAX_HIGH_SCORES; i++)
        {
            PlayerPrefs.DeleteKey($"{HIGH_SCORE_KEY}{i}");
            PlayerPrefs.DeleteKey($"{HIGH_SCORE_KEY}Coins{i}");
            PlayerPrefs.DeleteKey($"{HIGH_SCORE_KEY}Distance{i}");
            PlayerPrefs.DeleteKey($"{HIGH_SCORE_KEY}Blocks{i}");
        }
        PlayerPrefs.Save();

        UpdateHighScoreUI();
    }
}