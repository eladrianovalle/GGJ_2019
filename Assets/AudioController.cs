using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	// ---- Music ----
	public AK.Wwise.Event GameMusic;
	public AK.Wwise.Event YouWin;
	public AK.Wwise.Event YouLose;

	// ---- SFX -----
	// Impacts
	public AK.Wwise.Event ImpactGood;
	public AK.Wwise.Event ImpactBad;
	public AK.Wwise.Event Battery;

	// General
	public AK.Wwise.Event PlayerJump;
	public AK.Wwise.Event PlayerFall;
	public AK.Wwise.Event JumpSuccess;
	public AK.Wwise.Event TvStatic;
	public AK.Wwise.Event MomKnock;

	// Ambience
	public AK.Wwise.Event RoomAmbience;

	// ---- VOX ----
	public AK.Wwise.Event MomDialogue;
	public AK.Wwise.Event MomOutside;
	public AK.Wwise.Event MaternityHeave;




	void OnEnable()
	{
		ThrownObject.OnObjectHit += PlayHitSound;
		MomLauncher.OnMomThrow += PlayMomDialogue;
		GameController.OnPlayerWinGame += PlayYouWin;
		HandheldGame.OnCharacterJump += PlayJumpSound;
		HandheldGame.OnGameStart += PlayGameMusic;
	}

	void OnDisable(){
		ThrownObject.OnObjectHit -= PlayHitSound;
		MomLauncher.OnMomThrow -= PlayMomDialogue;
		GameController.OnPlayerWinGame -= PlayYouWin;
		HandheldGame.OnCharacterJump -= PlayJumpSound;
		HandheldGame.OnGameStart -= PlayGameMusic;

	}




	//.............Music.......................
	void PlayGameMusic()
	{
		GameMusic.Post (gameObject);
	}

	void PlayYouWin()
	{
		YouWin.Post (gameObject);
	}

	void PlayYouLose ()
	{
		YouLose.Post (gameObject);
	}


	//.............SFX.............................

	// Impact Sounds

	void PlayHitSound(bool isGood)
	{
		if (isGood) 
		{
			ImpactGood.Post (gameObject);
		}
		else 
		{
			ImpactBad.Post (gameObject);
		}
	}

	// General

	void PlayJumpSound ()
	{
		PlayerJump.Post (gameObject);
	}

	void PlayTvStatic()
	{
		TvStatic.Post (gameObject);
	}

	void PlayMomKnock ()
	{
		MomKnock.Post (gameObject);
	}


	// Ambience

	void PlayRoomAmbience()
	{
		RoomAmbience.Post (gameObject);
	}


	//..............VOX..........................

	void PlayMomDialogue()
	{
		MomDialogue.Post (gameObject);
	}

	void PlayMomOutside()
	{
		MomOutside.Post (gameObject);
	}

	void PlayTitle ()
	{
		MaternityHeave.Post (gameObject);
	}






}
