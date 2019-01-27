using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomLauncher : MonoBehaviour {

	public GameObject player;
	public GameObject throwStart;
	public GameObject[] throwableObjects;

	SpriteRenderer sRenderer;
	public Sprite[] momsprites;

	float momDelay = 3f;
	public float throwInterval = 3.0f;
	float throwTimer;
	bool timerRunning = false;

	public Vector3[] momPositions;

	float force = 1.5f;
	float heightMult = 3.0f;
	public float throwTorqueForce = 5.0f;
	public Vector2 minMaxThrowTorque;

	void Awake()
	{
		sRenderer = GetComponent<SpriteRenderer> ();
		throwTimer = throwInterval;
	}

	void Start () 
	{
//		InvokeRepeating ("ThrowObject", throwInterval, throwInterval);

		MomLeaveRoom ();

		LeanTween.delayedCall (momDelay, ()=>{
			MomEnterRoom();
			timerRunning = true;
		});
	}

	void Update()
	{
		if (timerRunning)
		{
			throwTimer -= Time.deltaTime;

			if (throwTimer < 0)
			{
				MomEnterRoom ();

				float newThrowInterval = throwInterval * 0.95f;
				throwInterval = newThrowInterval;
				throwTimer = throwInterval;
			}

		}
	}

	void MomEnterRoom()
	{
		LeanTween.moveLocal (this.gameObject, momPositions[1], 2f).setEase(LeanTweenType.easeOutQuint).setOnComplete(()=>{
			ThrowObject();
		});
	}

	void MomLeaveRoom()
	{
		sRenderer.sprite = momsprites [0];

		LeanTween.moveLocal (this.gameObject, momPositions[0], 2f).setEase(LeanTweenType.easeInQuint).setOnComplete(()=>{
			//close door and screenshake
		});
	}

	void ThrowObject () 
	{
		if (GameController.gameOver)
		{
			return;
		}

		sRenderer.sprite = momsprites [1];

		LeanTween.delayedCall (0.5f, ()=>{
			//		Debug.Log ("Mom throws a thing!!!");
			GameObject objToThrow = Instantiate(throwableObjects [Random.Range(0, throwableObjects.Length)]);

			objToThrow.transform.position = throwStart.transform.position;
			Rigidbody objToThrowRbody = objToThrow.GetComponent<Rigidbody> ();

			AddTorque(objToThrowRbody);
			Vector3 throwDirection = player.transform.position - objToThrow.transform.position;
			Vector3 throwTarget = (Vector3.up * heightMult) + throwDirection;

			objToThrowRbody.AddForce (throwTarget * force, ForceMode.Impulse);

			sRenderer.sprite = momsprites [2];

			LeanTween.delayedCall (1f, ()=>{
				MomLeaveRoom();
			});
		});

	}

	private void AddTorque(Rigidbody rb)
	{
		var x = UnityEngine.Random.Range(minMaxThrowTorque.x, minMaxThrowTorque.y);
		var y = UnityEngine.Random.Range(minMaxThrowTorque.x, minMaxThrowTorque.y);
		var z = UnityEngine.Random.Range(minMaxThrowTorque.x, minMaxThrowTorque.y);
		rb.AddTorque(new Vector3(x,y,z) * throwTorqueForce);
	}
}
