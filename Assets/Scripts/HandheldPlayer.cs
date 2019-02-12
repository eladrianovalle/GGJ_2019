using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandheldPlayer : MonoBehaviour {
	public Animator leftBtnAnimator, centerBtnAnimator, rightBtnAnimator;
	public Material leftBtnMat, centerBtnMat, rightBtnMat;
	public float rotationSpeed = 5.0f;
	public Vector4 brightnessVector;
	private Vector4 defaultBrightnessVector = Vector4.one;
	Rigidbody rBody;
	public float moveSpeed = 3.0f;
	public const float MOVE_LIMIT = 13.0f;
	public float maxYRot = 7f;
	public float maxZRot = 4f;
	public GameObject handheldWrapper;

	void OnEnable()
	{
		GameController.OnHandheldMoveLeft 		+= MoveLeft;
		GameController.OnHandheldMoveRight 		+= MoveRight;
		GameController.OnHandheldButtonPress 	+= CenterButtonDown;
		GameController.OnRightButtonUp 			+= RightButtonUp;
		GameController.OnLeftButtonUp 			+= LeftButtonUp;
		GameController.OnCenterButtonUp 		+= CenterButtonUp;
	}

	void OnDisable()
	{
		GameController.OnHandheldMoveLeft 		-= MoveLeft;
		GameController.OnHandheldMoveRight		-= MoveRight;
		GameController.OnHandheldButtonPress 	-= CenterButtonDown;
		GameController.OnRightButtonUp 			-= RightButtonUp;
		GameController.OnLeftButtonUp 			-= LeftButtonUp;
		GameController.OnCenterButtonUp 		-= CenterButtonUp;
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
		handheldWrapper.transform.localRotation = Quaternion.Slerp (handheldWrapper.transform.localRotation, Quaternion.identity, Time.unscaledDeltaTime * rotationSpeed);
	}

	void MoveLeft(bool isMovingLeft)
	{
		leftBtnAnimator.Play("Lbtn_GoDown");
		HighlightButtonColor(leftBtnMat);

		// Debug.Log ("Move Left");
		var targetRot = Quaternion.Euler (0, maxYRot, -maxZRot);
		handheldWrapper.transform.localRotation = Quaternion.Slerp(handheldWrapper.transform.localRotation, targetRot, Time.unscaledDeltaTime * rotationSpeed);

		Vector3 movePosition = this.rBody.transform.position + (Vector3.left * moveSpeed) * Time.unscaledDeltaTime;
		if (movePosition.x <= -MOVE_LIMIT)
		{
			movePosition.x = -MOVE_LIMIT;
		}
		rBody.MovePosition (movePosition);
	}

	void MoveRight(bool isMovingRight)
	{
		rightBtnAnimator.Play("Btn_GoDown");
		HighlightButtonColor(rightBtnMat);

		// Debug.Log ("Move Right");
		var targetRot = Quaternion.Euler (0, -maxYRot, maxZRot);
		handheldWrapper.transform.localRotation = Quaternion.Slerp(handheldWrapper.transform.localRotation, targetRot, Time.deltaTime * rotationSpeed);

		Vector3 movePosition = this.rBody.transform.position + (Vector3.right * moveSpeed) * Time.unscaledDeltaTime;
		if (movePosition.x >= MOVE_LIMIT)
		{
			movePosition.x = MOVE_LIMIT;
		}
		rBody.MovePosition (movePosition);
	}

	void CenterButtonDown(bool buttonPress)
	{
		centerBtnAnimator.Play("Cbtn_GoDown");
		HighlightButtonColor(centerBtnMat);
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

	void CenterButtonUp()
	{
		centerBtnAnimator.Play("Cbtn_StayUp");
		DefaultButtonColor(centerBtnMat);
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
