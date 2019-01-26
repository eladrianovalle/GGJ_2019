using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomLauncher : MonoBehaviour {

	public GameObject[] throwableObjects;
	public float throwInterval = 3.0f;

	void Start () 
	{
		InvokeRepeating ("ThrowObject", throwInterval, throwInterval);
	}
	
	void ThrowObject () 
	{
		Debug.Log ("Mom throws a thing!!!");
		GameObject objToThrow = throwableObjects [Random.Range(0, throwableObjects.Length)];

	}
}
