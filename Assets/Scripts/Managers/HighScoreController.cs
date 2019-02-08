using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[ExecuteInEditMode]
public class HighScoreController : MonoBehaviour
{

    public static Action<int> onAddToCurrentScore;
	public static Action<int> onRemoveFromCurrentScore;
	public static Action<int> onSetHighScore;
	public static Action<int> onSubmitScore;
    
    public TextMeshProUGUI currPlayerNameText;
    public TextMeshProUGUI currPlayerScoreText;
	public string currPlayerName = "player name";
    public int currScore = 0;

	public RandomUserNameContainer randomUsernames;
	private List<string> usernamesList;

    private string highScoresKey = "HighScore";

//    private string highScoreTemplateStr = "HIGH SCORE ";
//    private string yourScoreTemplateStr = "YOUR SCORE ";
	private string yourScoreTemplateStr = "";

	public bool addInlineStyling = false;
	public string highScoreColor;
	public string yourScoreColor;

	public HighScoreEntry[] highScoreEntries;
	public GameObject topUserNamesTextContainer;



//    private int highScore
//    {
//        get
//        {
//            return PGetHighScore();
//        }
//        set
//        {
//            PSetHighScore(value);
//        }
//    }

    private void Awake()
    {
//        SetHighScoreText(GetHighScore().ToString());

	/* 	
 		1 populate usernames list from random name scriptable object
		2 
		3
	*/

        SetCurrentScoreText("0");
    }
    
    // PUBLIC METHODS ------------------------------------------------------------------------------------------------
    
    public int GetCurrScore()
    {
        return currScore;
    }
    
//    public int GetHighScore()
//    {
//        return highScore;
//    }
//    
//    // ----------------------------------------------------------------------------------------------------------------
//    
//    private void SetHighScore(int score)
//    {
//        highScore = score;
//        SetHighScoreText(highScore.ToString());
//    }

//	private void SubmitScore(int score)
//	{
//		if (score > highScore)
//		{
//			SetHighScore(score);
//		}
//	}
    
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

    
//    private int PGetHighScore()
//    {
//        if (!PlayerPrefs.HasKey(highScoreStr))
//            PlayerPrefs.SetInt(highScoreStr, 0);
//        
//        return PlayerPrefs.GetInt(highScoreStr);
//    }
//
//    private void PSetHighScore(int score)
//    {
//        PlayerPrefs.SetInt(highScoreStr, score);
//    }
//
//    private void SetHighScoreText(string text)
//    {
//		if (highScoreText != null)
//		{
//			if (addInlineStyling)
//				highScoreText.text = highScoreTemplateStr + "<color=" + highScoreColor + ">" + text + "</color>";
//			else 
//				highScoreText.text = highScoreTemplateStr + text;
//			
//		}
//    }

    private void SetCurrentScoreText(string text)
    {
		if (currPlayerScoreText != null)
		{
			if (addInlineStyling)				
				currPlayerScoreText.text = yourScoreTemplateStr + "<color=" + yourScoreColor + ">" + text + "</color>";
			else
				currPlayerScoreText.text = yourScoreTemplateStr + text;
		}
    }
    
    private void OnEnable()
    {
        onAddToCurrentScore += AddToCurrScore;
        onRemoveFromCurrentScore += RemoveFromCurrScore;
//        onSetHighScore += SetHighScore;
//		onSubmitScore += SubmitScore;
    }

    private void OnDisable()
    {
        onAddToCurrentScore -= AddToCurrScore;
        onRemoveFromCurrentScore -= RemoveFromCurrScore;
//        onSetHighScore -= SetHighScore;
//		onSubmitScore -= SubmitScore;
    }
}

[System.Serializable]
public class PlayerInfo
{
	public string Name 	{ get; private set; }
	public int Points 	{ get;  set; }

	public PlayerInfo(string name, int points) {
		this.Name = name;
		this.Points = points;
	}
}

public class LeaderBoard 
{ 
	private PlayerInfo[] playerInfos = null;
	public const string data = "LeaderBoardData";
	public LeaderBoard(){}

	public void SetPlayers(PlayerInfo[] playerInfos)
	{
		this.playerInfos = playerInfos;
	}

	public void SaveData()
	{
		RankData ();
		Save.SaveData<PlayerInfo[]>(playerInfos);
	}

	public PlayerInfo[] RetrieveData() 
	{
		playerInfos = Save.GetDataArray<PlayerInfo>();
		return playerInfos;
	}

	public void RankData() 
	{
		Comparison<PlayerInfo> comparer = (a, b) => b.Points.CompareTo(a.Points);
		Array.Sort(playerInfos, comparer);
	}
}

public static class Save 
{
	public const string Data = "LeaderBoardData";
	public static void SaveData<T>(object [] items) where T : class
	{
		BinaryFormatter bf = new BinaryFormatter();
		MemoryStream ms = new MemoryStream();
		bf.Serialize(ms, items as T[]);
		PlayerPrefs.SetString(Data, Convert.ToBase64String(ms.GetBuffer()));
	}

	public static T[] GetDataArray<T>() where T: class
	{
		if (PlayerPrefs.HasKey(Data) == false) { return null; }
		string str = PlayerPrefs.GetString(Data);
		BinaryFormatter bf = new BinaryFormatter();
		MemoryStream ms = new MemoryStream(Convert.FromBase64String(str));
		return bf.Deserialize(ms) as T[];
	}
}