using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreEntry : MonoBehaviour {

	public TextMeshProUGUI nameText;
	public TextMeshProUGUI scoreText;

	public string entryName 	{ get; private set; }
	public int entryScore 	{ get; private set; }
	
	public void SetScore (string name, int score) 
	{
		this.entryName 	= name;
		this.entryScore = score;

		this.nameText.text 	= name;
		this.scoreText.text 	= score.ToString();
	}
}
