using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandheldPlayer : MonoBehaviour {

	Rigidbody rBody;
	public float moveSpeed = 2.0f;

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

	void MoveLeft(bool isMovingLeft)
	{
		Debug.Log ("Move Left");

		Vector3 movePosition = this.rBody.transform.position += (Vector3.left * moveSpeed) * Time.deltaTime;
		rBody.MovePosition (movePosition);
	}

	void MoveRight(bool isMovingRight)
	{
		Debug.Log ("Move Right");
		Vector3 movePosition = this.rBody.transform.position += (Vector3.right * moveSpeed) * Time.deltaTime;
		rBody.MovePosition (movePosition);
	}
}
