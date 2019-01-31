using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreController : MonoBehaviour
{

    public Action<int> onAddScore;
    public Action<int> onRemoveScore;
    public TextMeshProUGUI scoreText;
    private string highScoreStr = "HighScore";
    
    private int highScore
    {
        get
        {
            return PGetHighScore();
        }
        set
        {
            PSetHighScore(value);
        }
    }

    private void Awake()
    {
        SetHighScoreText(GetHighScore().ToString());
    }
    
    public int GetHighScore()
    {
        return highScore;
    }
    
    public void AddToHighScore(int amount)
    {
        highScore += amount;
        SetHighScoreText(highScore.ToString());
    }

    public void RemoveFromScore(int amount)
    {
        highScore -= amount;

        if (highScore < 0)
            highScore = 0;
        
        SetHighScoreText(highScore.ToString());
    }

    public void ResetHighScore()
    {
        highScore = 0;
        SetHighScoreText(highScore.ToString());
    }
    
    private void OnEnable()
    {
        onAddScore += AddToHighScore;
        onRemoveScore += RemoveFromScore;
    }

    private void OnDisable()
    {
        onAddScore -= AddToHighScore;
        onRemoveScore -= RemoveFromScore;
    }

    private int PGetHighScore()
    {
        if (!PlayerPrefs.HasKey(highScoreStr))
            PlayerPrefs.SetInt(highScoreStr, 0);
        
        return PlayerPrefs.GetInt(highScoreStr);
    }

    private void PSetHighScore(int score)
    {
        PlayerPrefs.SetInt(highScoreStr, score);
    }

    private void SetHighScoreText(string text)
    {
        if (scoreText != null)
        {
            scoreText.text = text;
        }
    }
}
