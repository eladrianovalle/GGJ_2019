using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour {

	public static Action<bool> OnHandheldMoveLeft;
	public static Action<bool> OnHandheldMoveRight;
	public static Action<bool> OnCharacterJump;




	void Start () {
		
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
	}
}
