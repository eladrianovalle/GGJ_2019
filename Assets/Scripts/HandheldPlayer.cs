using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandheldPlayer : MonoBehaviour {

	Rigidbody rBody;

	public float moveSpeed = 2.0f;

	public GameObject handheldWrapper;

	void OnEnable()
	{
		GameController.OnHandheldMoveLeft 	+= MoveLeft;
		GameController.OnHandheldMoveRight 	+= MoveRight;
	}

	void OnDisable()
	{
		GameController.OnHandheldMoveLeft 	-= MoveLeft;
		GameController.OnHandheldMoveRight	-= MoveRight;
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
		Debug.Log ("Move Left");
		handheldWrapper.transform.localRotation = Quaternion.Euler (0, 15, 0);
		Vector3 movePosition = this.rBody.transform.position += (Vector3.left * moveSpeed) * Time.deltaTime;
		rBody.MovePosition (movePosition);
	}

	void MoveRight(bool isMovingRight)
	{
		Debug.Log ("Move Right");
		handheldWrapper.transform.localRotation = Quaternion.Euler (0, -15, 0);

		Vector3 movePosition = this.rBody.transform.position += (Vector3.right * moveSpeed) * Time.deltaTime;
		rBody.MovePosition (movePosition);
	}
}
