using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FMODAudioController : MonoBehaviour {

	[Header("BANK LOADER")]
	[FMODUnity.BankRef] public string GameplayBank;

	[Space(7)]
	[Header("MUSIC EVENTS")]

	[FMODUnity.EventRef] public string GameMusic;

	// ---- SFX -----
	[Space(7)]
	[Header("SFX EVENTS")]

	[Space(3)]
	[Header("--Impacts")]
	// Impacts
	[FMODUnity.EventRef] public string ImpactGood;

	[FMODUnity.EventRef] public string ImpactBad;

	[FMODUnity.EventRef] public string BatteryPowerUp;

	[Space(3)]
	[Header("--General")]
	// General
	[FMODUnity.EventRef] public string PlayerJump;
	[FMODUnity.EventRef] public string PlayerFall;
	[FMODUnity.EventRef] public string JumpSuccess;
	[FMODUnity.EventRef] public string MomKnock;
	[FMODUnity.EventRef] public string NewHighScore;


	[Space(3)]
	[Header("--Environment")]

	[FMODUnity.EventRef] public string TvStatic;
	[FMODUnity.EventRef] public string DoorOpenSound;
	[FMODUnity.EventRef] public string DoorCloseSound;

	// Ambience
	[FMODUnity.EventRef] public string RoomAmbience;


	// ---- VOX ----
	[Space(7)]
	[Header("VOX EVENTS")]

	[FMODUnity.EventRef] public string MomDialogue;
	[FMODUnity.EventRef] public string MomOutside;
	[FMODUnity.EventRef] public string MomTitleScreen;


	FMOD.Studio.EventInstance GameplayMusic;
	FMOD.Studio.EventInstance RoomAmbienceEvent;

	FMOD.Studio.EventInstance PlayerJumpSound;

//	FMOD.Studio.ParameterInstance Win;
//	FMOD.Studio.ParameterInstance Lose;

	FMOD.Studio.Bus MasterBus;
	FMOD.Studio.Bus SFXBus;

	void Awake ()
	{
		FMODUnity.RuntimeManager.LoadBank (GameplayBank);
		MasterBus = FMODUnity.RuntimeManager.GetBus ("bus:/");
		SFXBus = FMODUnity.RuntimeManager.GetBus ("bus:/SFX-Bus");

		RoomAmbienceEvent = FMODUnity.RuntimeManager.CreateInstance (RoomAmbience);
		GameplayMusic = FMODUnity.RuntimeManager.CreateInstance(GameMusic);


	}

	void OnEnable()
	{
		ThrownObject.OnObjectHit += PlayHitSound;
		MomLauncher.OnMomThrow += PlayMomDialogue;
		MomLauncher.OnDoorOpen += PlayDoorOpen;
		MomLauncher.OnDoorClosed += PlayDoorClose;
		GameController.OnPlayerWinGame += PlayYouWin;
		GameController.OnPlayerLoseGame += PlayYouLose;
		HandheldGame.OnCharacterJump += PlayJumpSound;
		HandheldGame.OnGameStart += PlayGameMusic;
		HandheldGame.OnPowerupPickup += PlayBatteryPickUp;
		HandheldGame.OnCharacterFall += PlayPlayerFall;
	}

	void OnDisable()
	{
		ThrownObject.OnObjectHit -= PlayHitSound;
		MomLauncher.OnMomThrow -= PlayMomDialogue;
		MomLauncher.OnDoorOpen -= PlayDoorOpen;
		MomLauncher.OnDoorClosed -= PlayDoorClose;
		GameController.OnPlayerWinGame -= PlayYouWin;
		GameController.OnPlayerLoseGame -= PlayYouLose;
		HandheldGame.OnCharacterJump -= PlayJumpSound;
		HandheldGame.OnGameStart -= PlayGameMusic;
		HandheldGame.OnPowerupPickup -= PlayBatteryPickUp;
		HandheldGame.OnCharacterFall -= PlayPlayerFall;

	}




//	//.............Music.......................
	void Start ()
	{
		// Reset Music Audio on Start
		MasterBus.stopAllEvents (FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		SFXBus.setVolume (0.5f);


		GameplayMusic.setParameterValue("Win", 0f);
		GameplayMusic.setParameterValue("Lose", 0f);
	
		PlayRoomAmbience();
	}

	void PlayGameMusic()
	{
		GameplayMusic.start ();
		Debug.Log ("Music Started!");
	}

	void PlayYouWin()
	{
		GameplayMusic.setParameterValue("Win", 1f);
	}

	void PlayYouLose ()
	{
		// Play You Lose Transition
		GameplayMusic.setParameterValue("Lose", 1f);

		// Turn off SFX so they don't play in background
		SFXBus.stopAllEvents (FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		SFXBus.setVolume (0f); 

		Debug.Log ("You Suck!");
	}

	void PlayRoomAmbience()
	{
		RoomAmbienceEvent.start ();
	}



//	//.............SFX.............................

	// Impact Sounds

	void PlayHitSound(bool isGood)
	{
		if (isGood) 
		{
			FMODUnity.RuntimeManager.PlayOneShot (ImpactGood, gameObject.transform.position);
		}
		else 
		{
			FMODUnity.RuntimeManager.PlayOneShot (ImpactBad, gameObject.transform.position);		
		}
	}

	void PlayBatteryPickUp()
	{
		FMODUnity.RuntimeManager.PlayOneShot (BatteryPowerUp, gameObject.transform.position);	
	}


//	// General

	void PlayJumpSound ()
	{
		Debug.Log ("You Jumped!");
		FMODUnity.RuntimeManager.PlayOneShot (PlayerJump, gameObject.transform.position);
	}

	void PlayTvStatic()
	{
		FMODUnity.RuntimeManager.PlayOneShot (TvStatic, gameObject.transform.position);
	}

	void PlayMomKnock ()
	{
		FMODUnity.RuntimeManager.PlayOneShot (MomKnock, gameObject.transform.position);	
	}
	

	void PlayNewHighScore ()
	{
		FMODUnity.RuntimeManager.PlayOneShot (NewHighScore, gameObject.transform.position);
	}

	void PlayDoorOpen ()
	{
		FMODUnity.RuntimeManager.PlayOneShot (DoorOpenSound, gameObject.transform.position);
	}

	void PlayDoorClose ()
	{
		FMODUnity.RuntimeManager.PlayOneShot (DoorCloseSound, gameObject.transform.position);
	}

	void PlayPlayerFall()
	{
		FMODUnity.RuntimeManager.PlayOneShot (PlayerFall, gameObject.transform.position);
	}



//	// Ambience


	//..............VOX..........................

	void PlayMomDialogue()
	{
		FMODUnity.RuntimeManager.PlayOneShot (MomDialogue, gameObject.transform.position);
	}

	void PlayMomOutside()
	{
		FMODUnity.RuntimeManager.PlayOneShot (MomOutside, gameObject.transform.position);
	}

	void PlayTitle ()
	{
		FMODUnity.RuntimeManager.PlayOneShot (MomTitleScreen, gameObject.transform.position);	
	}






}
