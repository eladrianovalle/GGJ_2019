using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomLauncher : MonoBehaviour {

	public GameObject player;
	public GameObject throwStart;
	public GameObject[] throwableObjects;
	public float throwInterval = 3.0f;
	public float force = 5.0f;

	void Start () 
	{
		InvokeRepeating ("ThrowObject", throwInterval, throwInterval);
	}
	
	void ThrowObject () 
	{
		Debug.Log ("Mom throws a thing!!!");
		GameObject objToThrow = Instantiate(throwableObjects [Random.Range(0, throwableObjects.Length)]);

		objToThrow.transform.position = throwStart.transform.position;
		Rigidbody objToThrowRbody = objToThrow.GetComponent<Rigidbody> ();
		Vector3 throwDirection = player.transform.position - objToThrow.transform.position;
		objToThrowRbody.AddForce (throwDirection * force, ForceMode.Force);
	}
}
