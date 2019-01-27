﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandheldPlayer : MonoBehaviour {
	public Animator leftBtnAnimator;
	public Animator centerBtnAnimator;
	public Animator rightBtnAnimator;

	Rigidbody rBody;
	public float moveSpeed = 2.0f;
	float moveLimit = 3.0f;
	public GameObject handheldWrapper;

	void OnEnable()
	{
		GameController.OnHandheldMoveLeft 	+= MoveLeft;
		GameController.OnHandheldMoveRight 	+= MoveRight;
		GameController.OnRightButtonUp 		+= RightButtonUp;
		GameController.OnLeftButtonUp 		+= LeftButtonUp;

	}

	void OnDisable()
	{
		GameController.OnHandheldMoveLeft 	-= MoveLeft;
		GameController.OnHandheldMoveRight	-= MoveRight;
		GameController.OnRightButtonUp 		-= RightButtonUp;
		GameController.OnLeftButtonUp 		-= LeftButtonUp;
	}

	void Awake()
	{
		rBody = GetComponent<Rigidbody> ();
	}

	void Update()
	{
		handheldWrapper.transform.localRotation = Quaternion.Slerp (handheldWrapper.transform.localRotation, Quaternion.identity, Time.deltaTime * 5f);
	}

	void MoveLeft(bool isMovingLeft)
	{
		leftBtnAnimator.Play("Lbtn_GoDown");
		Debug.Log ("Move Left");
		handheldWrapper.transform.localRotation = Quaternion.Euler (0, 15, -3);

		Vector3 movePosition = this.rBody.transform.position + (Vector3.left * moveSpeed) * Time.deltaTime;
		if (movePosition.x <= -moveLimit)
		{
			movePosition.x = -moveLimit;
		}
		rBody.MovePosition (movePosition);
	}

	void MoveRight(bool isMovingRight)
	{
		rightBtnAnimator.Play("Btn_GoDown");
		Debug.Log ("Move Right");
		handheldWrapper.transform.localRotation = Quaternion.Euler (0, -15, 3);

		Vector3 movePosition = this.rBody.transform.position + (Vector3.right * moveSpeed) * Time.deltaTime;
		if (movePosition.x >= moveLimit)
		{
			movePosition.x = moveLimit;
		}

		rBody.MovePosition (movePosition);
	}

	void RightButtonUp()
	{
		rightBtnAnimator.Play("Btn_StayUp");
	}

	void LeftButtonUp()
	{
		leftBtnAnimator.Play("Lbtn_StayUp");
	}
}
