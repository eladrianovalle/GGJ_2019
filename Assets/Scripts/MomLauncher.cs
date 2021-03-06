﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomLauncher : MonoBehaviour {

	public GameObject player;
	public GameObject throwStart;
	public GameObject[] throwableObjects;

	private Dictionary<GameObject, Sprite> throwablePreviews;

	private GameObject[] throwableObjectsShuffleBag;
	private int shuffleBagIndex;

	SpriteRenderer sRenderer;
	public Sprite[] momsprites;

	float momDelay = 15f;
	float throwInterval = 10.0f;
	float throwTimer;
	bool timerRunning = false;

	float momAlertTime = 2.5f;
	float momAlertStaticTime = 0.4f;
	float momAlertItemPreviewTime = 2f;

	public Vector3[] momPositions;
	float momMoveSpeed = 0.35f;

	int throwCount = 0;
	GameObject nextThrownObject;

	float force = 1.5f;
	float heightMult = 3.0f;
	public float throwTorqueForce = 5.0f;
	public Vector2 minMaxThrowTorque;

	private Vector3 lastThrowPosition = Vector3.positiveInfinity;
	private int repeatThrowChance = 20;
	private int throwVarianceChance = 60;
	private float throwVariance = 1.5f;

	public float returnTimescaleSpeed = 0.9f;

	public static System.Action OnDoorOpen;
	public static System.Action OnDoorClosed;
	public static System.Action OnMomThrow;
//	public static System.Action<GameObject> OnMomChooseObject;
	public static System.Action<Sprite> OnMomChooseObjectPreview;

	void OnEnable()
	{
		UIController.OnStartGame 	+= StartMomGame;
	}

	void OnDisable()
	{
		UIController.OnStartGame 	-= StartMomGame;
		LeanTween.cancel(this.gameObject);
	}

	void Awake()
	{
		sRenderer = GetComponent<SpriteRenderer> ();
		throwTimer = throwInterval;
	}

	void Start () 
	{
//		InvokeRepeating ("ThrowObject", throwInterval, throwInterval);

		MomLeaveRoom ();

		throwablePreviews = new Dictionary<GameObject, Sprite>();
		for (int i = 0; i < throwableObjects.Length; i++)
		{
			throwablePreviews.Add(throwableObjects[i], throwableObjects[i].GetComponent<ThrownObject>().PreviewSprite);
		}

		throwableObjectsShuffleBag = (GameObject[])throwableObjects.Clone();
		shuffleBagIndex = throwableObjectsShuffleBag.Length - 1;

		if (OnMomChooseObjectPreview != null)
		{
			OnMomChooseObjectPreview(null);
		}
		//MomChooseObject();

//		LeanTween.delayedCall (momDelay, ()=>{
//			MomEnterRoom();
//			timerRunning = true;
//		});
	}

	void StartMomGame()
	{
		LeanTween.delayedCall (momDelay, ()=>{
			MomAlert();
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
				timerRunning = false;
				float newThrowInterval = throwInterval * 0.95f;
				throwInterval = newThrowInterval;
				throwTimer = throwInterval;

				MomAlert();
			}
		}

		if (Time.timeScale < 1f)
		{
			Time.timeScale += Time.unscaledDeltaTime * returnTimescaleSpeed;
//			Time.timeScale = Mathf.Lerp(Time.timeScale, 1.0f, Time.unscaledDeltaTime * returnTimescaleSpeed);
		}
		else if (Time.timeScale > 1f)
		{
			Time.timeScale = 1f;
		}
	}

	void MomAlert()
	{
		Television.ShowStatic(true);
		LeanTween.delayedCall(momAlertStaticTime, ()=>{
			Television.ShowStatic(false);
		});
		LeanTween.delayedCall(momAlertTime - momAlertItemPreviewTime, ()=>{
			MomChooseObject();
		});
		LeanTween.delayedCall(momAlertTime, ()=>{
			MomEnterRoom();
		});
	}

	void MomEnterRoom()
	{
		if (OnDoorOpen != null)
		{
			OnDoorOpen();
		}

		if (OnMomChooseObjectPreview != null)
		{
			OnMomChooseObjectPreview(null);
		}
		Television.ShowStatic(true);

		LeanTween.moveLocal (this.gameObject, momPositions[1], momMoveSpeed).setEase(LeanTweenType.easeOutQuint).setOnComplete(()=>{
			// open door rotation y at 9.5f
			ThrowObject();

			LeanTween.delayedCall (1f, ()=>{
				timerRunning = true;
			});
		});
	}

	void MomLeaveRoom()
	{
		sRenderer.sprite = momsprites [0];

		LeanTween.moveLocal (this.gameObject, momPositions[0], momMoveSpeed).setEase(LeanTweenType.easeInQuint).setOnComplete(()=>{
			//close door and screenshake
			// closed door at rotation y 120f
			if (OnDoorClosed != null)
			{
				OnDoorClosed();
			}

			Television.ShowStatic(false);
		});
	}

	void ThrowObject () 
	{
		if (GameController.gameOver)
		{
			return;
		}

		if (OnMomThrow != null)
		{
			OnMomThrow ();
		}

		sRenderer.sprite = momsprites [1];

		LeanTween.delayedCall (0.5f, ()=>{
			GameObject objToThrow = Instantiate(nextThrownObject);

			objToThrow.transform.position = throwStart.transform.position;
			Rigidbody objToThrowRbody = objToThrow.GetComponent<Rigidbody> ();

			AddTorque(objToThrowRbody);
			Vector3 currThrowPosition = player.transform.position;
			if ((throwCount > 0) && (repeatThrowChance > Random.Range(0, 100)))
			{
				// Check if repeat last throw
				currThrowPosition = lastThrowPosition;
			}
			else if (throwVarianceChance > Random.Range(0, 100))
			{
				// Check if add variance
				currThrowPosition.x += Random.Range(-throwVariance, throwVariance);
				if (currThrowPosition.x < -HandheldPlayer.MOVE_LIMIT)
				{
					currThrowPosition.x = -HandheldPlayer.MOVE_LIMIT;
				}
				else if (currThrowPosition.x > HandheldPlayer.MOVE_LIMIT)
				{
					currThrowPosition.x = HandheldPlayer.MOVE_LIMIT;
				}
			}

			//Vector3 throwDirection = player.transform.position - objToThrow.transform.position;
			Vector3 throwDirection = currThrowPosition - objToThrow.transform.position;
			Vector3 throwTarget = (Vector3.up * heightMult) + throwDirection;

			objToThrowRbody.AddForce (throwTarget * force, ForceMode.Impulse);

			sRenderer.sprite = momsprites [2];
			LeanTween.delayedCall(0.03f, ()=>{
				Time.timeScale = 0f;
			});

			throwCount++;
			lastThrowPosition = currThrowPosition;

			LeanTween.delayedCall (1f, ()=>{
				MomLeaveRoom();
			});
		});

	}

	void MomChooseObject()
	{
		//nextThrownObject = throwableObjects[Random.Range(0, throwableObjects.Length)];

		/// Shuffle Bag for Throwable Objects
		if (shuffleBagIndex > 0)
		{
			int index = Random.Range(0, shuffleBagIndex + 1);
			if (index != shuffleBagIndex)
			{
				// Swap Items
				GameObject currentGO = throwableObjectsShuffleBag[index];
				throwableObjectsShuffleBag[index] = throwableObjectsShuffleBag[shuffleBagIndex];
				throwableObjectsShuffleBag[shuffleBagIndex] = currentGO;
			}
		}

		nextThrownObject = throwableObjectsShuffleBag[shuffleBagIndex];
		shuffleBagIndex--;
		if (shuffleBagIndex < 0)
		{
			shuffleBagIndex = throwableObjectsShuffleBag.Length - 1;
		}

		Television.ShowStatic(false);

//		if (OnMomChooseObject != null)
//		{
//			OnMomChooseObject(nextThrownObject);
//		}
		if (OnMomChooseObjectPreview != null)
		{
			Sprite nextPreview = null;
			throwablePreviews.TryGetValue(nextThrownObject, out nextPreview);
			OnMomChooseObjectPreview(nextPreview);
//			OnMomChooseObjectPreview(throwablePreviews[nextThrownObject]);
		}
	}

	private void AddTorque(Rigidbody rb)
	{
		var x = UnityEngine.Random.Range(minMaxThrowTorque.x, minMaxThrowTorque.y);
		var y = UnityEngine.Random.Range(minMaxThrowTorque.x, minMaxThrowTorque.y);
		var z = UnityEngine.Random.Range(minMaxThrowTorque.x, minMaxThrowTorque.y);
		rb.AddTorque(new Vector3(x,y,z) * throwTorqueForce);
	}
}
