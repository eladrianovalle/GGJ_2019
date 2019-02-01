using System;
using System.Collections.Generic;
using UnityEngine;

public class HandheldGame : MonoBehaviour
{
	#region Game Settings
	[SerializeField]
	[Header("GameLoop Frame Time")]
	[Tooltip("Essentially inner game \"speed\"")]
	protected float frameTime = 0.5f;
	private float currFrameTime = 0f;
	private int endFrames = 6;
	private int currEndFrames = 0;
	private int startFrames = 6;
	private int currStartFrames = 0;

	[SerializeField]
	protected const int COLUMNS = 5;
	[SerializeField]
	protected int[] startingPlatformValues 	= new int[COLUMNS]{ 1, 1, 1, 1, 1 };
	protected int[] startingPowerupValues 	= new int[COLUMNS]{ 0, 0, 0, 0, 0 };

	[SerializeField]
	protected const int PLATFORM_LIMIT = 8;
	//protected const int POWERUP_COOLDOWN = 4;
	[SerializeField]
	protected const int POWERUP_CHANCE = 10;	// Percent chance
	#endregion

	private bool started = false;

	// Values
	[SerializeField] private int JumpValue = 1;
	[SerializeField] private int PowerupValue = 2;

	#region Events
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
	/// <summary>
	/// The character jump event.
	/// </summary>
	public static Action OnCharacterJump;
	/// <summary>
	/// The character fall event.
	/// </summary>
	public static Action OnCharacterFall;
	/// <summary>
	/// The powerup pickup event.
	/// </summary>
	public static Action OnPowerupPickup;
	/// <summary>
	/// The score change event from old score to new score.
	/// </summary>
	public static Action<int, int> OnScoreChange;
	#endregion

	#region GameObjects
	[Header("Screen Objects")]
	[SerializeField] private GameObject NinjaJump;
	[SerializeField] private GameObject NinjaStand;
	[SerializeField] private GameObject NinjaFall;

	[SerializeField] private Animator NinjaStandAnimator;

	[SerializeField] private SpriteRenderer[] Buildings = new SpriteRenderer[COLUMNS];

	[SerializeField] private SpriteRenderer Battery;

	[SerializeField] private GameObject[] BatteryItems;

	[SerializeField] private GameObject BatteryPickup;

	private const int SCORE_DIGITS = 3;
	[SerializeField] private SpriteRenderer[] ScoreDigits = new SpriteRenderer[SCORE_DIGITS];

	[SerializeField] private GameObject GameOver;
	[SerializeField] private SpriteRenderer Timer;

	[Header("Platform Sprites")]
	[SerializeField] private Sprite buildingSprite;
	[SerializeField] private Sprite pitSprite;

	private const int BATT_COUNT = 4;
	[SerializeField] private Sprite[] batterySprites = new Sprite[BATT_COUNT];

	[SerializeField] private Sprite[] numberSprites = new Sprite[10];
	#endregion

	// Properties
	protected HandheldCharacter character = new HandheldCharacter();
	protected List<int> platformValues;
	protected List<int> powerupValues;

	protected int currPlatformSize = 5;

	protected bool buttonPressed = false;

	protected int score = 0;

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
		UIController.OnStartGame += StartHandheldGame;
		GameController.OnHandheldButtonPress += ButtonPress;
		GameController.OnPlayerWinGame += SubmitScore;
		GameController.OnPlayerLoseGame += SubmitScore;
	}

	void OnDisable()
	{
		UIController.OnStartGame -= StartHandheldGame;
		GameController.OnHandheldButtonPress -= ButtonPress;
		GameController.OnPlayerWinGame -= SubmitScore;
		GameController.OnPlayerLoseGame -= SubmitScore;
	}

	public void Init()
	{
		character.Init();
		NinjaJump.SetActive(false);
		NinjaFall.SetActive(false);
		platformValues = new List<int>(startingPlatformValues);
		powerupValues = new List<int>(startingPowerupValues);

		CurrentGameState = HandheldGameState.START;
	}

	void Start()
	{
		AssertObjects();
		Init();
	}

	void Update()
	{
		if (!started)
		{
			return;
		}

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
		currFrameTime += Time.deltaTime;
		if (currFrameTime >= frameTime)
		{
			currFrameTime = 0f;

			int timer = Mathf.FloorToInt(((1 + startFrames) - currStartFrames) * frameTime);
			if (timer >= 0 && timer <= 3)
			{
				Timer.gameObject.SetActive(timer != 0);
				Timer.sprite = numberSprites[timer];
			}
			//RunGameLoop();
			if (++currStartFrames > startFrames)
			{
				currStartFrames = 0;

				buttonPressed = false;
				GameOver.SetActive(false);
				Timer.gameObject.SetActive(false);
				Init();
				CurrentGameState = HandheldGameState.PLAYING;
				NinjaStandAnimator.SetBool("gameStart", true);
				return;
			}
		}
	}

	private void EndUpdate()
	{
		currFrameTime += Time.deltaTime;
		if (currFrameTime >= .25f)
		{
			currFrameTime = 0f;

			RunGameLoop();

			if (++currEndFrames > endFrames)
			{
				currEndFrames = 0;

				buttonPressed = false;

				if (GameController.playerLives > 0)
				{
					GameOver.SetActive(false);
					Init();
					CurrentGameState = HandheldGameState.START;
				}
				else
				{
					NinjaFall.SetActive(true);
					GameOver.SetActive(true);
				}
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
		currFrameTime += Time.deltaTime;
		if (currFrameTime >= frameTime)
		{
			currFrameTime = 0f;
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

			// Update Ground and Items location
			if (platformValues[0] == 0)
			{
				// Successful jump land, add points
				AddPoints(JumpValue);
			}
			for (int i = 0; i < COLUMNS; i++)
			{
				if (i + 1 < COLUMNS)
				{
					platformValues[i] = platformValues[i + 1];
					powerupValues[i] = powerupValues[i + 1];
				}
				else
				{
					if (platformValues[i - 1] == 0)
					{
						// 1
						platformValues[i] = 1;
					}
					else if (currPlatformSize > PLATFORM_LIMIT)
					{
						// 0
						platformValues[i] = 0;
					}
					else
					{
						//random
						platformValues[i] = UnityEngine.Random.Range(0, 2);
					}
					//platformValues[i] = (platformValues[i - 1] == 0) ? 1 : UnityEngine.Random.Range(0, 2);
					currPlatformSize = (platformValues[i] == 0) ? 0 : currPlatformSize + 1;

					powerupValues[i] = ((powerupValues[i - 1] == 0) && (POWERUP_CHANCE > UnityEngine.Random.Range(0, 100))) ? 1 : 0;
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
				if (powerupValues[0] == 1)
				{
					// Powerup!
					AddPoints(PowerupValue);
					GameController.PlayerGainLife();
					if (OnPowerupPickup != null)
					{
						OnPowerupPickup();
					}
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
		Battery.sprite = batterySprites[GameController.playerLives];

//		if (character.IsJumping())
//		{
//			NinjaJump.SetActive(true);
//			BatteryPickup.SetActive(powerupValues[0] == 1);
//		}
		NinjaJump.SetActive(character.IsJumping());
		NinjaStand.SetActive(character.IsStanding());
		if (NinjaStand.activeSelf && CurrentGameState == HandheldGameState.PLAYING)
		{
			NinjaStandAnimator.SetBool("gameStart", character.IsStanding());
		}
		//NinjaFall.SetActive(character.IsFalling());

		BatteryPickup.SetActive((powerupValues[0] == 1) && (character.IsJumping()));
		//BatteryItems[0].SetActive((powerupValues[0] == 1) && (!character.IsJumping()));

		for (int i = 0; i < COLUMNS; i++)
		{
			Buildings[i].sprite = (platformValues[i] != 0) ? buildingSprite : pitSprite;
			BatteryItems[i].SetActive(((i > 0) || (!character.IsJumping())) && (powerupValues[i] != 0));
			//BatteryItems[i].SetActive((powerupValues[i] != 0));
		}

		DrawScore();
	}

	private void DrawScore()
	{
		int currentScore = score;

		for (int i = 0; i < SCORE_DIGITS; i++)
		{
			ScoreDigits[i].sprite = numberSprites[currentScore % 10];
			currentScore /= 10;
		}
	}

	private void StartHandheldGame()
	{
		started = true;
		DrawScreen();
		if (OnGameStart != null)
		{
			OnGameStart();
		}
	}

	private void ButtonPress(bool b)
	{
		buttonPressed = true;
	}

	private void AddPoints(int points = 1)
	{
		int oldScore = score;
		score += points;
		if (OnScoreChange != null)
		{
			OnScoreChange(oldScore, score);
		}
		if (HighScoreController.onAddToCurrentScore != null)
		{
			HighScoreController.onAddToCurrentScore(points);
		}
		DrawScore();
	}

	private void SubmitScore()
	{
		if (HighScoreController.onSubmitScore != null)
		{
			HighScoreController.onSubmitScore(score);
		}
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
			Debug.Assert(ScoreDigits[i] != null, "[HandheldGame] Score" + i + " object missing!", this.gameObject);
		}
		Debug.Assert(GameOver != null, "[HandheldGame] GameOver object missing!", this.gameObject);
	}
}


