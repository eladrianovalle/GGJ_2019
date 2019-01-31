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
			scoreController.AddToCurrScore(10);
		}
		
		if (GUILayout.Button("Remove 10 from curr score"))
		{
			scoreController.RemoveFromCurrScore(10);
		}
		
		if (GUILayout.Button("Reset curr Score"))
		{
			scoreController.ResetCurrScore();
		}
		
		GUILayout.Label("High score");
		
		if (GUILayout.Button("Reset High Score"))
		{
			scoreController.ResetHighScore();
		}
	}
}
