using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandheldPlayer : MonoBehaviour {
	public Animator leftBtnAnimator;
	public Animator centerBtnAnimator;
	public Animator rightBtnAnimator;

	Rigidbody rBody;
	public float moveSpeed = 2.0f;

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

	void MoveLeft(bool isMovingLeft)
	{
		leftBtnAnimator.Play("Lbtn_GoDown");
		Debug.Log ("Move Left");
		Vector3 movePosition = this.rBody.transform.position += (Vector3.left * moveSpeed) * Time.deltaTime;
		rBody.MovePosition (movePosition);
	}

	void MoveRight(bool isMovingRight)
	{
		rightBtnAnimator.Play("Btn_GoDown");
		Debug.Log ("Move Right");
		Vector3 movePosition = this.rBody.transform.position += (Vector3.right * moveSpeed) * Time.deltaTime;
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
