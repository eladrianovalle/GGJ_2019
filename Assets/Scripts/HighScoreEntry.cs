using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreEntry : MonoBehaviour {

	public TextMeshProUGUI nameText;
	public TextMeshProUGUI scoreText;

	public string entryName 	{ get; private set; }
	public string entryScore 	{ get; private set; }
	
	void SetScore (string name, string score) 
	{
		this.entryName 	= name;
		this.entryScore = score;

		nameText.text 	= name;
		scoreText.text 	= score;
	}
}
