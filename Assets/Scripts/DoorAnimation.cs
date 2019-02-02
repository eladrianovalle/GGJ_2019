using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour {

	public AK.Wwise.Event doorOpenSound;
	public AK.Wwise.Event doorCloseSound;

	public GameObject door;
	Vector3 openRot 	= new Vector3 (0, 9.5f, 0);
	Vector3 closedRot 	= new Vector3 (0, 120f, 0);
	float doorMoveSpeed = 0.5f;

	private Animation anim;

	private void Awake()
	{
		anim = GetComponent<Animation>();
	}
	void OnEnable () 
	{
		MomLauncher.OnDoorOpen 		+= OpenDoor;
		MomLauncher.OnDoorClosed 	+= CloseDoor;
	}

	void OnDisable ()
	{
		MomLauncher.OnDoorOpen 		-= OpenDoor;
		MomLauncher.OnDoorClosed 	-= CloseDoor;
	}
	
	void OpenDoor () 
	{
		anim.Play("DoorOpen");
		doorOpenSound.Post(gameObject);
		//print ("Door open");

		//LeanTween.rotate (door, openRot, doorMoveSpeed).setEase(LeanTweenType.easeShake);
//		LeanTween.rotateLocal (door, openRot, 1f).setEase(LeanTweenType.easeShake);
//		door.transform.localRotation = Quaternion.Slerp (door.transform.localRotation, Quaternion.Euler(openRot), Time.deltaTime * 5f);
	}

	void CloseDoor ()
	{
		anim.Play("DoorClose");
		doorCloseSound.Post(gameObject);
		//LeanTween.rotate (door, closedRot, doorMoveSpeed).setEase(LeanTweenType.easeShake);
//		LeanTween.rotateLocal (door, closedRot, 1f).setEase(LeanTweenType.easeShake);
//		door.transform.localRotation = Quaternion.Slerp (door.transform.localRotation, Quaternion.Euler(closedRot), Time.deltaTime * 5f);
	}
}
