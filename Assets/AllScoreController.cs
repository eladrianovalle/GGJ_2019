using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AllScoreController : MonoBehaviour
{

    public TextMeshProUGUI[] highScoreEntries;
    public RandomUserNameContainer randomUsernames;
    public GameObject topUserNamesTextContainer;
    public bool hideTopScores;
    
   // private HighScorePlayer[] highScorePlayers; 

    private List<string> usernameList;

    [SerializeField]
    private int numHighScorePlayers;
    private string numHighScorePlayersStr = "NumHighScorePlayers";

    private void Start()
    {
        // Check how many high score text entries we have when making a high score player array
        //highScorePlayers = new HighScorePlayer[highScoreEntries.Length];

        // Cache a list of user names which we will grab from later
        usernameList = randomUsernames.userNames.ToList();
        
        if (hideTopScores)
            topUserNamesTextContainer.SetActive(false);

        // Get the number of high score players that exist
       numHighScorePlayers = GetNumHighScorePlayers();
    }

    public void ResetHighScorePlayers()
    {
        PlayerPrefs.SetInt(numHighScorePlayersStr, 0);
        numHighScorePlayers = 0;
    }

    public void AddHighScorePlayer(int score)
    {
        numHighScorePlayers = GetNumHighScorePlayers();
        numHighScorePlayers++;
        CreateHighScorePlayerEntry(score);
        PlayerPrefs.SetInt(numHighScorePlayersStr, numHighScorePlayers);
    }

     public void RemoveHighScorePlayer()
    {
        numHighScorePlayers = GetNumHighScorePlayers();
        numHighScorePlayers--;

        if (numHighScorePlayers < 0)
            numHighScorePlayers = 0;

        PlayerPrefs.SetInt(numHighScorePlayersStr, numHighScorePlayers);
    }
    
    private string PopRandomUsername()
    {
        int index = Random.Range(0, usernameList.Count);
		// username pluse a random 2 digit number
		// or 
		// check length of usernam and then make up the diff from 12 with numbers
		string username = usernameList[index];
        usernameList.RemoveAt(index);

        return username;
    }

    private int GetNumHighScorePlayers()
    {
        int numPlayers = 0;
        if (PlayerPrefs.HasKey(numHighScorePlayersStr))
        {
            numPlayers = PlayerPrefs.GetInt(numHighScorePlayersStr);
        }
        
        return numPlayers;
    }

    private void CreateHighScorePlayerEntry(int score)
    {
        string username = PopRandomUsername();
        PlayerPrefs.SetInt(username, score);
    }
}

// public class HighScorePlayer {
//     //public string playerPrefsName = "";
//     public string userName = "";
// }
