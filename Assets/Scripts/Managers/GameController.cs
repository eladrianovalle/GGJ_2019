﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour {

	public static Action<bool> OnHandheldMoveLeft;
	public static Action<bool> OnHandheldMoveRight;
	public static Action<bool> OnHandheldButtonPress;

	public static Action OnRightButtonUp;
	public static Action OnLeftButtonUp;


	public static Action OnPlayerLoseLife;
	public static Action OnPlayerGainLife;
	public static Action OnPlayerLoseGame;
	public static Action OnPlayerWinGame;

	public static int playerLives = 3;
	float timeLimit = 180f;
	float timer;

	public static bool gameOver;

	void Start () 
	{
		ResetGame ();
	}

	void ResetGame()
	{
		timer = timeLimit;
		playerLives = 3;
		gameOver = false;
	}

	void Update () 
	{
		if (gameOver)
		{
			return;
		}

		if (Input.GetKey (KeyCode.LeftArrow))
		{
			if (OnHandheldMoveLeft != null)
			{
				OnHandheldMoveLeft (true);
			}
		}
		else if (Input.GetKey (KeyCode.RightArrow))
		{
			if (OnHandheldMoveRight != null)
			{
				OnHandheldMoveRight (true);
			}
		}
		else if (Input.GetKeyDown (KeyCode.UpArrow))
		{
			if (OnHandheldButtonPress != null)
			{
				OnHandheldButtonPress (true);
			}
		}
		
		// Used for making the right button go back up
		if (!Input.GetKey(KeyCode.RightArrow))
		{
			
			if (OnRightButtonUp != null)
			{
				OnRightButtonUp();
			}
		}

		// Used for making the left button go back up
		if (!Input.GetKey(KeyCode.LeftArrow))
		{
			// Left button animation to play
			if (OnLeftButtonUp != null)
			{
				OnLeftButtonUp();
			}
		}

		if (playerLives <= 0)
		{
			if (OnPlayerLoseGame != null)
			{
				OnPlayerLoseGame ();
				gameOver = true;
			}
		}

		if (!RunningTimer())
		{
			Debug.Log ("Player wins!!!!");
			if (OnPlayerWinGame != null)
			{
				OnPlayerWinGame ();
				gameOver = true;
			}
		}
	}

	bool RunningTimer()
	{
		bool timerIsRunning = timer > 0;
		if (timerIsRunning)
		{
			timer -= Time.deltaTime; 
		}
		return timerIsRunning;
	}

	public static void PlayerLoseLife()
	{
		if (playerLives > 0)
		{
			playerLives--;
			if (OnPlayerLoseLife != null)
			{
				OnPlayerLoseLife ();
			}
		}
	}

	public static void PlayerGainLife()
	{
		if (playerLives < 3)
		{
			playerLives++;
		}
	}
}
