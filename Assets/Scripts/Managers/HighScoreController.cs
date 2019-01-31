using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreController : MonoBehaviour
{

    public Action<int> onAddToCurrentScore;
    public Action<int> onRemoveFromCurrentScore;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI currScoreText;
    public int currScore = 0;
    private string highScoreStr = "HighScore";

    private string highScoreTemplateStr = "HIGH SCORE: ";
    private string yourScoreTemplateStr = "YOUR SCORE: ";

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
    
    public void ResetHighScore()
    {
        highScore = 0;
        SetHighScoreText(highScore.ToString());
    }

    public int GetCurrScore()
    {
        return currScore;
    }

    public void ResetCurrScore()
    {
        currScore = 0;
    }
    
    public void AddToCurrScore(int amount)
    {
        currScore += amount;
        SetHighScoreText(currScore.ToString());
    }

    public void RemoveFromCurrScore(int amount)
    {
        currScore -= amount;

        if (currScore < 0)
            currScore = 0;
        
        SetHighScoreText(highScore.ToString());
    }

    
    
    private void OnEnable()
    {
        onAddToCurrentScore += AddToCurrScore;
        onRemoveFromCurrentScore += RemoveFromCurrScore;
    }

    private void OnDisable()
    {
        onAddToCurrentScore -= AddToCurrScore;
        onRemoveFromCurrentScore -= RemoveFromCurrScore;
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
        if (highScoreText != null)
            highScoreText.text = "HIGH SCORE <color=red>" + text + "</color>";
    }

    private void SetCurrentScoreText(string text)
    {
        if (currScoreText != null)
            currScoreText.text = "YOUR SCORE <color=blue>" + text + "</color>";
    }
}
