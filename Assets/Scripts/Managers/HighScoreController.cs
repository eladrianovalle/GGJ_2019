using System;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class HighScoreController : MonoBehaviour
{

    public static Action<int> onAddToCurrentScore;
	public static Action<int> onRemoveFromCurrentScore;
	public static Action<int> onSetHighScore;
	public static Action<int> onSubmitScore;
    
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
        SetCurrentScoreText("0");
    }
    
    // PUBLIC METHODS ------------------------------------------------------------------------------------------------
    
    public int GetCurrScore()
    {
        return currScore;
    }
    
    public int GetHighScore()
    {
        return highScore;
    }
    
    // ----------------------------------------------------------------------------------------------------------------
    
    private void SetHighScore(int score)
    {
        highScore = score;
        SetHighScoreText(highScore.ToString());
    }

	private void SubmitScore(int score)
	{
		if (score > highScore)
		{
			SetHighScore(score);
		}
	}
    
    private void AddToCurrScore(int amount)
    {
        currScore += amount;
        SetCurrentScoreText(currScore.ToString());
    }

    private void RemoveFromCurrScore(int amount)
    {
        currScore -= amount;

        if (currScore < 0)
            currScore = 0;
        
        SetCurrentScoreText(currScore.ToString());
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
            highScoreText.text = highScoreTemplateStr + " <color=red>" + text + "</color>";
    }

    private void SetCurrentScoreText(string text)
    {
        if (currScoreText != null)
            currScoreText.text = yourScoreTemplateStr + " <color=blue>" + text + "</color>";
    }
    
    private void OnEnable()
    {
        onAddToCurrentScore += AddToCurrScore;
        onRemoveFromCurrentScore += RemoveFromCurrScore;
        onSetHighScore += SetHighScore;
		onSubmitScore += SubmitScore;
    }

    private void OnDisable()
    {
        onAddToCurrentScore -= AddToCurrScore;
        onRemoveFromCurrentScore -= RemoveFromCurrScore;
        onSetHighScore -= SetHighScore;
		onSubmitScore -= SubmitScore;
    }
}
