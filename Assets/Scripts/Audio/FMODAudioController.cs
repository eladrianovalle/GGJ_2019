using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODAudioController : MonoBehaviour {

	[FMODUnity.BankRef]
	public string GameplayBank;

	[FMODUnity.EventRef]
	public string 
	// ---- Music ----
	GameMusic, 
	YouWin, 
	YouLose, 

	// ---- SFX -----
	// Impacts
	ImpactGood,
	ImpactBad,
	BatteryPowerUp,

	// General
	PlayerJump,
	PlayerFall,
	JumpSuccess,
	TvStatic,
	MomKnock,
	NewHighScore,
	DoorOpenSound,
	DoorCloseSound,

	// Ambience
	RoomAmbience,

	// ---- VOX ----
	MomDialogue,
	MomOutside,
	MomTitleScreen;


	FMOD.Studio.EventInstance GameplayMusic;
	FMOD.Studio.EventInstance GamestateYouWin;
	FMOD.Studio.EventInstance GamestateYouLose;
	FMOD.Studio.EventInstance RoomAmbienceEvent;

	void Awake ()
	{
		FMODUnity.RuntimeManager.LoadBank (GameplayBank);
	}

	void OnEnable()
	{
		ThrownObject.OnObjectHit += PlayHitSound;
		MomLauncher.OnMomThrow += PlayMomDialogue;
		MomLauncher.OnDoorOpen += PlayDoorOpen;
		MomLauncher.OnDoorClosed += PlayDoorClose;
		GameController.OnPlayerWinGame += PlayYouWin;
		HandheldGame.OnCharacterJump += PlayJumpSound;
		HandheldGame.OnGameStart += PlayGameMusic;

	}

	void OnDisable(){
		ThrownObject.OnObjectHit -= PlayHitSound;
		MomLauncher.OnMomThrow -= PlayMomDialogue;
		MomLauncher.OnDoorOpen -= PlayDoorOpen;
		MomLauncher.OnDoorClosed -= PlayDoorClose;
		GameController.OnPlayerWinGame -= PlayYouWin;
		HandheldGame.OnCharacterJump -= PlayJumpSound;
		HandheldGame.OnGameStart -= PlayGameMusic;
	}




//	//.............Music.......................
	void Start ()
	{
		GameplayMusic = FMODUnity.RuntimeManager.CreateInstance(GameMusic);
		GamestateYouWin = FMODUnity.RuntimeManager.CreateInstance(YouWin);
		GamestateYouLose = FMODUnity.RuntimeManager.CreateInstance(YouLose);
		RoomAmbienceEvent = FMODUnity.RuntimeManager.CreateInstance (RoomAmbience);

//		GameplayMusic.start ();


	}

	void PlayGameMusic()
	{
		GameplayMusic.start ();
	}

	void PlayYouWin()
	{
		//Fade Out code
		GamestateYouWin.start ();
	}

	void PlayYouLose ()
	{
		GamestateYouLose.start ();
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

//	// General

	void PlayJumpSound ()
	{
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
