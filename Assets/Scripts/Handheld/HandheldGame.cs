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


	protected HandheldCharacter character;
	protected Queue<int> floor;


	protected bool buttonPressed = false;


	void OnEnable()
	{
		GameController.OnCharacterJump += ButtonPress;
	}

	void OnDisable()
	{
		GameController.OnCharacterJump -= ButtonPress;
	}

	public void Init()
	{
		character = new HandheldCharacter();
		floor = new Queue<int>(startingFloor);
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
		if (!jump)
		{
			fall = character.TryFall();
		}

		if (fall)
		{
			// Game Over
		}

		buttonPressed = false;
	}

	private void ButtonPress(bool b)
	{
		buttonPressed = true;
	}
}


