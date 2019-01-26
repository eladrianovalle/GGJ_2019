using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour {

	public static Action<bool> OnHandheldMoveLeft;
	public static Action<bool> OnHandheldMoveRight;
	public static Action<bool> OnHandheldButtonPress;

	public static Action OnPlayerLoseLife;
	public static Action OnPlayerGainLife;
	public static Action OnPlayerLoseGame;
	public static Action OnPlayerWinGame;

	public static int playerLives = 3;
	float timeLimit = 30f;
	float timer;

	public static bool gameOver = false;

	void Start () 
	{
		timer = timeLimit;
	}
	
	void Update () 
	{
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
