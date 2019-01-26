using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour {

	public static Action<bool> OnHandheldMoveLeft;
	public static Action<bool> OnHandheldMoveRight;
	public static Action<bool> OnCharacterJump;

	public static Action OnPlayerLoseLife;
	public static Action OnPlayerGainLife;
	public static Action OnPlayerLoseGame;
	public static Action OnPlayerWinGame;

	public static int playerLives = 3;
	float timeLimit = 30f;
	float timer;

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
			if (OnCharacterJump != null)
			{
				OnCharacterJump (true);
			}
		}

		if (playerLives <= 0)
		{
			if (OnPlayerLoseGame != null)
			{
				OnPlayerLoseGame ();
			}
		}

		if (!RunningTimer())
		{
			Debug.Log ("Player wins!!!!");
			if (OnPlayerWinGame != null)
			{
				OnPlayerWinGame ();
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
		}
	}

	public static void PlayerGainLife()
	{
		if (playerLives > 0)
		{
			playerLives++;
		}
	}
}
