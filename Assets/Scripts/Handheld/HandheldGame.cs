using System;
using System.Collections.Generic;
using UnityEngine;

public class HandheldGame : MonoBehaviour
{
	// Settings
	[SerializeField]
	protected const int COLUMNS = 5;
	[SerializeField]
	protected int[] startingFloor = new int[COLUMNS]{ 1, 1, 1, 1, 1 };

	[SerializeField]
	[Tooltip("Frames per GameLoop update")]
	protected int frames = 20;

	// Events
	public static Action OnCharacterJump;
	public static Action OnCharacterFall;
	public static Action OnGameStart;
	public static Action OnGameOver;

	// Properties
	protected HandheldCharacter character;
	protected Queue<int> floor;


	protected bool buttonPressed = false;


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
		character = new HandheldCharacter();
		floor = new Queue<int>(startingFloor);
	}

	void Update()
	{
		
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

	}

	private void ButtonPress(bool b)
	{
		buttonPressed = true;
	}
}


