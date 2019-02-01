using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HighScoreController))]
public class HighScoreControllerInspector : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		HighScoreController scoreController = (HighScoreController) target;
		
		GUILayout.Label("Your score");
		
		if (GUILayout.Button("Add 10 to curr score"))
		{

			if (scoreController.onAddToCurrentScore != null)
				scoreController.onAddToCurrentScore(10);
		}
		
		if (GUILayout.Button("Remove 10 from curr score"))
		{
			if (scoreController.onRemoveFromCurrentScore != null)
				scoreController.onRemoveFromCurrentScore(10);
		}
		
		if (GUILayout.Button("Reset curr Score"))
		{
			if (scoreController.onRemoveFromCurrentScore != null)
				scoreController.onRemoveFromCurrentScore(99999999);
		}
		
		GUILayout.Label("High score");
		
		if (GUILayout.Button("Set High Score"))
		{
			var currScore = scoreController.GetCurrScore();
			if (scoreController.onSetHighScore != null)
				scoreController.onSetHighScore(currScore);
		}
		
		if (GUILayout.Button("Reset High Score"))
		{
			if (scoreController.onSetHighScore != null)
				scoreController.onSetHighScore(0);
		}
		
		
	}
}
