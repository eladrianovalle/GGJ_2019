using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandheldPlayer : MonoBehaviour {
	public Animator leftBtnAnimator, centerBtnAnimator, rightBtnAnimator;
	public Material leftBtnMat, centerBtnMat, rightBtnMat;
	public Vector4 brightnessVector;
	private Vector4 defaultBrightnessVector = Vector4.one;
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

		// Reset all buttton materials to default brightness
		DefaultButtonColor(leftBtnMat);
		DefaultButtonColor(centerBtnMat);
		DefaultButtonColor(rightBtnMat);
	}

	void Update()
	{
		handheldWrapper.transform.localRotation = Quaternion.Slerp (handheldWrapper.transform.localRotation, Quaternion.identity, Time.deltaTime * 5f);
	}

	void MoveLeft(bool isMovingLeft)
	{
		leftBtnAnimator.Play("Lbtn_GoDown");
		HighlightButtonColor(leftBtnMat);

		Debug.Log ("Move Left");
		handheldWrapper.transform.localRotation = Quaternion.Euler (0, 9, -3);

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
		HighlightButtonColor(rightBtnMat);

		Debug.Log ("Move Right");
		handheldWrapper.transform.localRotation = Quaternion.Euler (0, -9, 3);

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
		DefaultButtonColor(rightBtnMat);
	}

	void LeftButtonUp()
	{
		leftBtnAnimator.Play("Lbtn_StayUp");
		DefaultButtonColor(leftBtnMat);
	}
	
	private void HighlightButtonColor(Material mat)
	{
		mat.SetVector("_Brightness", brightnessVector);
	}

	private void DefaultButtonColor(Material mat)
	{
		mat.SetVector("_Brightness", defaultBrightnessVector);
	}

}
