using UnityEngine;
using System;
using Rewired;

public class GameController : MonoBehaviour {
	public GameObject[] gosToBeEnabled;
	public static bool isInvulnerable = false;
	public bool IS_INVULNERABLE = false;

	private Player playerController;

	public static Action<bool> OnHandheldMoveLeft;
	public static Action<bool> OnHandheldMoveRight;
	public static Action<bool> OnHandheldButtonPress;

	public static Action OnPressButtonToStart;

	public static Action OnRightButtonUp;
	public static Action OnLeftButtonUp;
	public static Action OnCenterButtonUp;

	public static Action OnPlayerLoseLife;
	public static Action OnPlayerGainLife;
	public static Action OnPlayerLoseGame;
	public static Action OnPlayerWinGame;

	public static int playerLives = 3;
	readonly float timeLimit = 180f;
	float timer;

	float inputX = 0f;

	public static bool gameReadyToRestart;
	public static bool gameOver;

	void OnEnable()
	{
		UIController.OnStartGame 	+= ResetGame;
	}

	void OnDisable()
	{
		UIController.OnStartGame 	-= ResetGame;
	}

	void Awake()
	{
		foreach (GameObject go in gosToBeEnabled)
		{
			go.SetActive (true);
		}

		playerController = ReInput.players.GetPlayer (0);

		isInvulnerable = IS_INVULNERABLE;
	}

	void Start () 
	{
		timer = timeLimit;
		playerLives = 3;
		gameReadyToRestart = true;
	}

	void ResetGame()
	{
		timer = timeLimit;
		playerLives = 3;
		gameOver = false;
	}

	void Update () 
	{
		if (gameReadyToRestart)
		{
			if (playerController.GetAnyButtonDown ())
			{
				if (OnPressButtonToStart != null)
				{
					gameReadyToRestart = false;
					OnPressButtonToStart ();
				}
			}
		}

		if (gameOver)
		{
			return;
		}

#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			IS_INVULNERABLE = !IS_INVULNERABLE;
			isInvulnerable = !isInvulnerable;
		}
#endif

		inputX = 0f;
		inputX = playerController.GetAxis(1);

		if (inputX != 0)
		{
			if (inputX < 0)
			{
				if (OnHandheldMoveLeft != null)
				{
					OnHandheldMoveLeft (true);
				}
			}
			else
			{
				if (OnHandheldMoveRight != null)
				{
					OnHandheldMoveRight (true);
				}
			}
		}

		// separate this from left/right movement so it won't get blocked while dodging mom
		if (playerController.GetButtonDown(3))
		{
			if (OnHandheldButtonPress != null)
			{
				OnHandheldButtonPress (true);
			}
		}
		
		// Used for making the right button go back up
		if (playerController.GetAxis(1) <= 0)
		{
			if (OnRightButtonUp != null)
			{
				OnRightButtonUp();
			}
		}

		// Used for making the left button go back up
		if (playerController.GetAxis(1) >= 0)
		{
			// Left button animation to play
			if (OnLeftButtonUp != null)
			{
				OnLeftButtonUp();
			}
		}

		if (playerController.GetButtonUp(3))
		{
			if (OnCenterButtonUp != null)
			{
				OnCenterButtonUp();
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
		if (isInvulnerable)
		{
			return;
		}

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