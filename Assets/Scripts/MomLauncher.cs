using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomLauncher : MonoBehaviour {

	public GameObject player;
	public GameObject throwStart;
	public GameObject[] throwableObjects;
	public float throwInterval = 3.0f;
	float force = 1.5f;
	float heightMult = 3.0f;
	public float throwTorqueForce = 5.0f;
	public Vector2 minMaxThrowTorque;
	void Start () 
	{
		InvokeRepeating ("ThrowObject", throwInterval, throwInterval);
	}
	
	void ThrowObject () 
	{
		if (GameController.gameOver)
		{
			return;
		}

		Debug.Log ("Mom throws a thing!!!");
		GameObject objToThrow = Instantiate(throwableObjects [Random.Range(0, throwableObjects.Length)]);

		objToThrow.transform.position = throwStart.transform.position;
		Rigidbody objToThrowRbody = objToThrow.GetComponent<Rigidbody> ();

		AddTorque(objToThrowRbody);
		Vector3 throwDirection = player.transform.position - objToThrow.transform.position;
		Vector3 throwTarget = (Vector3.up * heightMult) + throwDirection;

		objToThrowRbody.AddForce (throwTarget * force, ForceMode.Impulse);
	}

	private void AddTorque(Rigidbody rb)
	{
		var x = UnityEngine.Random.Range(minMaxThrowTorque.x, minMaxThrowTorque.y);
		var y = UnityEngine.Random.Range(minMaxThrowTorque.x, minMaxThrowTorque.y);
		var z = UnityEngine.Random.Range(minMaxThrowTorque.x, minMaxThrowTorque.y);
		rb.AddTorque(new Vector3(x,y,z) * throwTorqueForce);
	}
}
