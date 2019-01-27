using System;
using System.Collections.Generic;
using UnityEngine;

public class HandheldGame : MonoBehaviour
{
	// Settings
	[SerializeField]
	[Tooltip("Frames per GameLoop update")]
	protected int frames = 20;

	[SerializeField]
	protected const int COLUMNS = 5;
	[SerializeField]
	protected int[] startingPlatformValues = new int[COLUMNS]{ 1, 1, 1, 1, 1 };


	// Events
	public static Action OnCharacterJump;
	public static Action OnCharacterFall;
	public static Action OnGameStart;
	public static Action OnGameOver;

	// GameObjects
	[Header("Screen Objects")]
	[SerializeField] private GameObject NinjaJump;
	[SerializeField] private GameObject NinjaStand;
	[SerializeField] private GameObject NinjaFall;

	[SerializeField] private GameObject[] Buildings = new GameObject[COLUMNS];

	// Properties
	protected HandheldCharacter character;
	protected Queue<int> platformValues;

	protected bool buttonPressed = false;

	public enum HandheldGameState
	{
		START,
		PLAYING,
		END,
		CREDITS
	}

	public HandheldGameState CurrentGameState { get; private set; }

	void OnEnable()
	{
		GameController.OnHandheldButtonPress += ButtonPress;
	}

	void OnDisable()
	{
		GameController.OnHandheldButtonPress -= ButtonPress;
	}

	public void Init()
	{
		Debug.Assert(NinjaJump != null, "NinjaJump object missing!", this.gameObject);
		Debug.Assert(NinjaStand != null, "NinjaStand object missing!", this.gameObject);
		Debug.Assert(NinjaFall != null, "NinjaFall object missing!", this.gameObject);
		for (int i = 0; i < COLUMNS; i++)
		{
			Debug.Assert(Buildings[i] != null, "Building" + i + " object missing!", this.gameObject);
		}

		character = new HandheldCharacter();
		platformValues = new Queue<int>(startingPlatformValues);

		CurrentGameState = HandheldGameState.START;

	}

	void Update()
	{
		if (CurrentGameState == HandheldGameState.START)
		{
			if (buttonPressed)
			{
				buttonPressed = false;
				CurrentGameState = HandheldGameState.PLAYING;
			}
		}
	}

	public void RunGameLoop()
	{
		// Update Game Loop

		// GameState?

		// Check input
		// if input, then try actions (jump)
		bool jump = false;
		if (buttonPressed)
		{
			jump = character.TryJump();
		}

		bool fall = false;
		if (jump)
		{
			OnCharacterJump();
		}
		else
		{
			fall = character.TryFall();
		}

		if (fall)
		{
			OnCharacterFall();
			// Lives/Game Over
		}

		buttonPressed = false;

		// Draw state?
		DrawScreen();
	}

	private void DrawScreen()
	{
		
	}

	private void ButtonPress(bool b)
	{
		buttonPressed = true;
	}
}


