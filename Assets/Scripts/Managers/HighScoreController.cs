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
	private const int USERNAME_LENGTH = 13;
	private const int MAX_NUMBERS_IN_NAME = 4;

	public RandomUserNameContainer randomUsernames;
	private List<string> usernamesList;
	private string[] usernamesArray;

    private string highScoresKey = "HighScore";

	private string yourScoreTemplateStr = "";

	public bool addInlineStyling = false;
	public string highScoreColor;
	public string yourScoreColor;

	private LeaderBoard leaderBoard = null;
	public HighScoreEntry[] highScoreEntries;
	public GameObject topUserNamesTextContainer;
	private int maxRandomScore = 12;


    private void Awake ()
    {
// 		1 populate usernames list from random name scriptable object
		usernamesArray = PopulateUserNamesList (randomUsernames);

//		2 pick random name prefix from usernames list at random
		currPlayerName = GetRandomPlayerName(usernamesArray);

//		3 set current player name to random name
		SetCurrPlayerTextTo(currPlayerName);

//		4 set current player score to 0
		SetCurrentScoreText(0);

//		5 create leaderboard, checking for saved data
		leaderBoard = MakeLeaderBoard();

//		6 pass all data to highscoreentry[]
		DisplayLeaderBoard(leaderBoard);
    }

	void Update()
	{
		
	}
    
    // PUBLIC METHODS ------------------------------------------------------------------------------------------------
    
    public int GetCurrScore()
    {
        return currScore;
    } 
	// ----------------------------------------------------------------------------------------------------------------
  
	private string[] PopulateUserNamesList(RandomUserNameContainer randomUsernames)
	{
		string[] usernames = new string[randomUsernames.userNames.Length];
		for (int i = 0; i < randomUsernames.userNames.Length; i++)
		{
			usernames [i] = randomUsernames.userNames [i];
		}
		return usernames;
	}

	private string GetRandomPlayerName(string[] usernames)
	{
		string playerName = usernames[UnityEngine.Random.Range(0, usernames.Length)];

		char[] characters = playerName.ToCharArray ();

		int usernameLength = UnityEngine.Random.Range (characters.Length + 2, USERNAME_LENGTH + 1);
		char[] newName = new char[usernameLength];

		for (int i = 0; i < characters.Length; i++)
		{
			newName [i] = characters [i];
		}
			
		int c = characters.Length;
		int totalNumbersInName = 0;
		while (c < newName.Length && totalNumbersInName < MAX_NUMBERS_IN_NAME )
		{
			newName [c] = (UnityEngine.Random.Range (0, 10).ToString().ToCharArray()[0]);
			c++;
			totalNumbersInName++;
		}
		playerName = newName.ArrayToString ();

		return playerName;
	}

	private void SetCurrPlayerTextTo(string username)
	{
		currPlayerNameText.text = username;
	}

	private LeaderBoard MakeLeaderBoard()
	{
		LeaderBoard lb = new LeaderBoard();

//		Debug.Log ("PP has key: " + PlayerPrefs.HasKey(Save.Data));
		if (!PlayerPrefs.HasKey (Save.Data))
		{
			PlayerInfo[] infos = new PlayerInfo[5];
			for (int i = 0; i < infos.Length; i++)
			{
				infos [i] = new PlayerInfo (GetRandomPlayerName (usernamesArray), UnityEngine.Random.Range (1, maxRandomScore));
//				Debug.Log ("High Score " + i + " : " + infos[i].Name + " " + infos[i].Score);
			}
			lb.SetPlayers (infos);
			lb.SaveData ();
		}
		else
		{
			lb.RetrieveData ();
		}

		return lb;
	}

	private void DisplayLeaderBoard(LeaderBoard leaderBoard)
	{
		for (int i = 0; i < highScoreEntries.Length; i++)
		{
			PlayerInfo leaderBoardEntry = leaderBoard.Players [i];
			HighScoreEntry highScoreEntry = highScoreEntries [i];

			highScoreEntry.SetScore (leaderBoardEntry.Name, leaderBoardEntry.Score);
		}
	}
    
    private void AddToCurrScore(int amount)
    {
        currScore += amount;
        SetCurrentScoreText(currScore);
    }

    private void RemoveFromCurrScore(int amount)
    {
        currScore -= amount;

        if (currScore < 0)
            currScore = 0;
        
        SetCurrentScoreText(currScore);
    }

    private void SetCurrentScoreText(int score)
    {
		string text = score.ToString ();
		Debug.Log ("Score to set is " + text);

		if (currPlayerScoreText != null)
		{
			Debug.Log ("Let's set that score!");

			if (addInlineStyling)				
				currPlayerScoreText.text = yourScoreTemplateStr + "<color=" + yourScoreColor + ">" + text + "</color>";
			else
				currPlayerScoreText.text = yourScoreTemplateStr + text;
		}
    }
    
    private void OnEnable()
    {
        onAddToCurrentScore 		+= AddToCurrScore;
        onRemoveFromCurrentScore 	+= RemoveFromCurrScore;
//      onSetHighScore += SetHighScore;
//		onSubmitScore += SubmitScore;
    }

    private void OnDisable()
    {
        onAddToCurrentScore 		-= AddToCurrScore;
        onRemoveFromCurrentScore 	-= RemoveFromCurrScore;
//      onSetHighScore -= SetHighScore;
//		onSubmitScore -= SubmitScore;
    }
}

[System.Serializable]
public class PlayerInfo
{
	public string Name 	{ get; private set; }
	public int Score 	{ get;  set; }

	public PlayerInfo(string name, int points) {
		this.Name = name;
		this.Score = points;
	}
}

public class LeaderBoard 
{ 
	private PlayerInfo[] playerInfos = null;
	public PlayerInfo[] Players {get { return playerInfos; }}
	public const string data = "LeaderBoardData";
	public LeaderBoard(){}

	public void SetPlayers(PlayerInfo[] playerInfos)
	{
		this.playerInfos = playerInfos;
	}

	public void SaveData()
	{
		RankData ();
		Save.SaveData<PlayerInfo[]>(playerInfos as object[]);
	}

	public PlayerInfo[] RetrieveData() 
	{
		playerInfos = Save.GetDataArray<PlayerInfo>();
		return playerInfos;
	}

	public void RankData() 
	{
		Comparison<PlayerInfo> comparer = (a, b) => b.Score.CompareTo(a.Score);
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

		Debug.Log ("Binary Formatter is null? " + (bf == null));
		Debug.Log ("Memory Stream is null? " + (ms == null));
		Debug.Log ("Items is null? " + (items == null));

//		object[] saveObj = new object[items.Length] as T[];
//		for (int i = 0; i < saveObj.Length; i++)
//		{
//			saveObj [i] = items [i];
//		}
//		Debug.Log ("Saved Obj is null?" + (saveObj == null));

		bf.Serialize(ms, items);
//		bf.Serialize(ms, saveObj);
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