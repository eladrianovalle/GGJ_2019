﻿using System.Collections;
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
    public float variableMoveSpeed = 0.001f;
    float baseMoveSpeedMult;

	public const float MOVE_LIMIT = 13.0f;
	public float maxYRot = 7f;
	public float maxZRot = 4f;
	public GameObject handheldWrapper;
    //float timeStep;
    Vector3 velocity = Vector3.zero;
    public float dampTime = 3.0f;

    private bool isMovingLeft = false;
    private bool isMovingRight = false;

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

        baseMoveSpeedMult = variableMoveSpeed;

		// Reset all buttton materials to default brightness
		DefaultButtonColor(leftBtnMat);
		DefaultButtonColor(centerBtnMat);
		DefaultButtonColor(rightBtnMat);
	}

	void Update()
	{
        //timeStep = Time.fixedDeltaTime;

		handheldWrapper.transform.localRotation = Quaternion.Slerp (handheldWrapper.transform.localRotation, Quaternion.identity, MH_Time.timestep * rotationSpeed);

        if (isMovingLeft || isMovingRight)
        {
            variableMoveSpeed += MH_Time.unscaledtimestep;
            if (variableMoveSpeed > 1f)
            {
                variableMoveSpeed = 1f; 
            }
        }
        else
        {
            variableMoveSpeed = baseMoveSpeedMult; 
        }
    }

    private void FixedUpdate()
    {
        //fixedUnscaledDeltaTime = Time.fixedUnscaledDeltaTime;

        HandleMovement();

    }

    void HandleMovement()
    {
        if (isMovingLeft)
        {
            MovePositionLeft();
        }

        if (isMovingRight)
        {
            MovePositionRight();
        }
    }

    void MoveLeft(bool movingLeft)
    {
        this.isMovingLeft = movingLeft;
    }

    void MoveRight(bool movingRight)
    {
        this.isMovingRight = movingRight;
    }

    void MovePositionLeft()
	{
		leftBtnAnimator.Play("Lbtn_GoDown");
		HighlightButtonColor(leftBtnMat);

        Debug.Log ("Move Left");
		var targetRot = Quaternion.Euler (0, maxYRot, -maxZRot);
		handheldWrapper.transform.localRotation = Quaternion.Slerp(handheldWrapper.transform.localRotation, targetRot, MH_Time.fixedTimestep * rotationSpeed);

		Vector3 movePosition = this.rBody.transform.position + (Vector3.left * (moveSpeed * variableMoveSpeed)) * MH_Time.fixedTimestep;
		if (movePosition.x <= -MOVE_LIMIT)
		{
			movePosition.x = -MOVE_LIMIT;
		}

        rBody.MovePosition (movePosition);
    }

    void MovePositionRight()
	{
		rightBtnAnimator.Play("Btn_GoDown");
		HighlightButtonColor(rightBtnMat);

        Debug.Log ("Move Right");
		var targetRot = Quaternion.Euler (0, -maxYRot, maxZRot);
		handheldWrapper.transform.localRotation = Quaternion.Slerp(handheldWrapper.transform.localRotation, targetRot, MH_Time.fixedTimestep * rotationSpeed);

		Vector3 movePosition = this.rBody.transform.position + (Vector3.right * (moveSpeed * variableMoveSpeed)) * MH_Time.fixedTimestep;
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
        isMovingRight = false;
		rightBtnAnimator.Play("Btn_StayUp");
		DefaultButtonColor(rightBtnMat);
	}

	void LeftButtonUp()
	{
        isMovingLeft = false;
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
