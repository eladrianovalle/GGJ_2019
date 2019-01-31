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
		
		if (GUILayout.Button("Add 10 to high score"))
		{
			scoreController.AddToHighScore(10);
		}
		
		if (GUILayout.Button("Remove 10 from high score"))
		{
			scoreController.RemoveFromScore(10);
		}

		if (GUILayout.Button("Reset High Score"))
		{
			scoreController.ResetHighScore();
		}
	}
}
