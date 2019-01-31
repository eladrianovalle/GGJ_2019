using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomLauncher : MonoBehaviour {

	public GameObject player;
	public GameObject throwStart;
	public GameObject[] throwableObjects;

	private Dictionary<GameObject, Sprite> throwablePreviews;

	SpriteRenderer sRenderer;
	public Sprite[] momsprites;

	float momDelay = 15f;
	float throwInterval = 10.0f;
	float throwTimer;
	bool timerRunning = false;

	public Vector3[] momPositions;
	float momMoveSpeed = 0.35f;

	GameObject nextThrownObject;

	float force = 1.5f;
	float heightMult = 3.0f;
	public float throwTorqueForce = 5.0f;
	public Vector2 minMaxThrowTorque;

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

		MomChooseObject();

//		LeanTween.delayedCall (momDelay, ()=>{
//			MomEnterRoom();
//			timerRunning = true;
//		});
	}

	void StartMomGame()
	{
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
				timerRunning = false;
				float newThrowInterval = throwInterval * 0.95f;
				throwInterval = newThrowInterval;
				throwTimer = throwInterval;

				MomEnterRoom ();

				LeanTween.delayedCall (1f, ()=>{
					timerRunning = true;
				});
			}
		}
	}

	void MomEnterRoom()
	{
		OnDoorOpen ();

		LeanTween.moveLocal (this.gameObject, momPositions[1], momMoveSpeed).setEase(LeanTweenType.easeOutQuint).setOnComplete(()=>{
			// open door rotation y at 9.5f
			ThrowObject();
		});
	}

	void MomLeaveRoom()
	{
		sRenderer.sprite = momsprites [0];

		LeanTween.moveLocal (this.gameObject, momPositions[0], momMoveSpeed).setEase(LeanTweenType.easeInQuint).setOnComplete(()=>{
			//close door and screenshake
			// closed door at rotation y 120f
			OnDoorClosed();
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
			//		Debug.Log ("Mom throws a thing!!!");
			GameObject objToThrow = Instantiate(nextThrownObject);

			objToThrow.transform.position = throwStart.transform.position;
			Rigidbody objToThrowRbody = objToThrow.GetComponent<Rigidbody> ();

			AddTorque(objToThrowRbody);
			Vector3 throwDirection = player.transform.position - objToThrow.transform.position;
			Vector3 throwTarget = (Vector3.up * heightMult) + throwDirection;

			objToThrowRbody.AddForce (throwTarget * force, ForceMode.Impulse);

			sRenderer.sprite = momsprites [2];

			MomChooseObject();

			LeanTween.delayedCall (1f, ()=>{
				MomLeaveRoom();
			});
		});

	}

	void MomChooseObject()
	{
		nextThrownObject = throwableObjects[Random.Range(0, throwableObjects.Length)];
//		if (OnMomChooseObject != null)
//		{
//			OnMomChooseObject(nextThrownObject);
//		}
		if (OnMomChooseObjectPreview != null)
		{
//			Sprite nextPreview = null;
//			throwablePreviews.TryGetValue(nextThrownObject, out nextPreview);
//			OnMomChooseObjectPreview(nextPreview);
			OnMomChooseObjectPreview(throwablePreviews[nextThrownObject]);
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
