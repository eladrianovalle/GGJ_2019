using System;
using System.Collections.Generic;
using UnityEngine;

public class HandheldGame : MonoBehaviour
{
	// Settings
	[SerializeField]
	[Header("Frames per GameLoop update")]
	[Tooltip("Essentially inner game \"speed\"")]
	protected int frames = 20;
	private int currFrames = 0;
	private int endFrames = 6;
	private int currEndFrames = 0;

	[SerializeField]
	protected const int COLUMNS = 5;
	[SerializeField]
	protected int[] startingPlatformValues = new int[COLUMNS]{ 1, 1, 1, 1, 1 };


	// Events
	/// <summary>
	/// The character jump event.
	/// </summary>
	public static Action OnCharacterJump;
	/// <summary>
	/// The character fall event.
	/// </summary>
	public static Action OnCharacterFall;
	/// <summary>
	/// The game start event.
	/// </summary>
	public static Action OnGameStart;
	/// <summary>
	/// The game over event.
	/// </summary>
	public static Action OnGameOver;
	/// <summary>
	/// The game loop update event.
	/// </summary>
	public static Action OnGameLoopUpdate;

	// GameObjects
	[Header("Screen Objects")]
	[SerializeField] private GameObject NinjaJump;
	[SerializeField] private GameObject NinjaStand;
	[SerializeField] private GameObject NinjaFall;

	[SerializeField] private SpriteRenderer[] Buildings = new SpriteRenderer[COLUMNS];

	[SerializeField] private SpriteRenderer Battery;

	private const int SCORE_DIGITS = 3;
	[SerializeField] private SpriteRenderer[] Score = new SpriteRenderer[SCORE_DIGITS];

	[SerializeField] private GameObject GameOver;

	[Header("Platform Sprites")]
	[SerializeField] private Sprite buildingSprite;
	[SerializeField] private Sprite pitSprite;

	private const int BATT_COUNT = 4;
	[SerializeField] private Sprite[] batterySprites = new Sprite[BATT_COUNT];


	// Properties
	protected HandheldCharacter character = new HandheldCharacter();
	protected List<int> platformValues;

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
		character.Init();
		NinjaJump.SetActive(false);
		NinjaFall.SetActive(false);
		platformValues = new List<int>(startingPlatformValues);

		CurrentGameState = HandheldGameState.START;
	}

	void Start()
	{
		AssertObjects();
		Init();
	}

	void Update()
	{
		switch (CurrentGameState)
		{
			case HandheldGameState.START:
				StartUpdate();
				break;
			case HandheldGameState.PLAYING:
				PlayingUpdate();
				break;
			case HandheldGameState.END:
				EndUpdate();
				break;
			case HandheldGameState.CREDITS:
				break;
		}

		DrawScreen();
	}

	private void StartUpdate()
	{
		if (buttonPressed)
		{
			buttonPressed = false;
			//Init();
			CurrentGameState = HandheldGameState.PLAYING;
			if (OnGameStart != null)
			{
				OnGameStart();
			}
		}
	}

	private void EndUpdate()
	{
		if (++currFrames >= 5)
		{
			currFrames = 0;

			RunGameLoop();

			if (++currEndFrames >= endFrames)
			{
				currEndFrames = 0;

				buttonPressed = false;
				GameOver.SetActive(false);
				Init();
				CurrentGameState = HandheldGameState.PLAYING;
			}

//			if (buttonPressed)
//			{
//				buttonPressed = false;
//				GameOver.SetActive(false);
//				//CurrentGameState = HandheldGameState.START;
//				Init();
//			}
		}
	}

	private void PlayingUpdate()
	{
		if (++currFrames >= frames)
		{
			currFrames = 0;
			RunGameLoop();
		}
	}

	public void RunGameLoop()
	{
		// Update Game Loop
		if (OnGameLoopUpdate != null)
		{
			OnGameLoopUpdate();
		}

		// GameState?
		if (CurrentGameState == HandheldGameState.PLAYING)
		{

			// UpdateGround
			for (int i = 0; i < COLUMNS; i++)
			{
				if (i + 1 < COLUMNS)
				{
					platformValues[i] = platformValues[i + 1];
				}
				else
				{
					platformValues[i] = (platformValues[i - 1] == 0) ? 1 : UnityEngine.Random.Range(0, 2);
				}
			}

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
				if (OnCharacterJump != null)
				{
					OnCharacterJump();
				}
			}
			else
			{
				character.TryStand();
				if (platformValues[0] == 0)
				{
					fall = character.TryFall();
				}
			}

			if (fall)
			{
				if (OnCharacterFall != null)
				{
					OnCharacterFall();
				}
				// Lives/Game Over?
				CurrentGameState = HandheldGameState.END;
				GameController.PlayerLoseLife();
				GameOver.SetActive(true);
				NinjaFall.SetActive(true);
				if (OnGameOver != null)
				{
					OnGameOver();
				}
			}

			buttonPressed = false;

			// Draw state?
			DrawScreen();

		}
		else if (CurrentGameState == HandheldGameState.END)
		{
			//GameOver.SetActive(!GameOver.activeSelf);
			NinjaFall.SetActive(!NinjaFall.activeSelf);
		}
	}

	private void DrawScreen()
	{
		NinjaJump.SetActive(character.CurrentState == HandheldCharacter.CharacterState.JUMPING);
		NinjaStand.SetActive(character.CurrentState == HandheldCharacter.CharacterState.STANDING);
		//NinjaFall.SetActive(character.CurrentState == HandheldCharacter.CharacterState.FALLING);

		for (int i = 0; i < COLUMNS; i++)
		{
			Buildings[i].sprite = (platformValues[i] != 0) ? buildingSprite : pitSprite;
		}

		Battery.sprite = batterySprites[GameController.playerLives];
	}

	private void ButtonPress(bool b)
	{
		buttonPressed = true;
	}

	private void AssertObjects()
	{
		Debug.Assert(NinjaJump != null, "[HandheldGame] NinjaJump object missing!", this.gameObject);
		Debug.Assert(NinjaStand != null, "[HandheldGame] NinjaStand object missing!", this.gameObject);
		Debug.Assert(NinjaFall != null, "[HandheldGame] NinjaFall object missing!", this.gameObject);
		for (int i = 0; i < COLUMNS; i++)
		{
			Debug.Assert(Buildings[i] != null, "[HandheldGame] Building" + i + " object missing!", this.gameObject);
		}
		Debug.Assert(Battery != null, "[HandheldGame] Battery object missing!", this.gameObject);
		for (int i = 0; i < SCORE_DIGITS; i++)
		{
			Debug.Assert(Score[i] != null, "[HandheldGame] Score" + i + " object missing!", this.gameObject);
		}
		Debug.Assert(GameOver != null, "[HandheldGame] GameOver object missing!", this.gameObject);
	}
}


