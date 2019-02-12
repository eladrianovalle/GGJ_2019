using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour {

	public Transform targetTransform;
	Vector3 targetPosition;
	Quaternion targetRotation;

	float distance; 	// distance in the z plane
	float height;		// height in the y plane

	public float heightDamping = 2.0f;
	public float distanceDamping = 0.6f;
	public float rotationDamping = 10.0f;

	public float breathingRadius = 0.5f;

	public bool keepOriginalRotation = true;
	public bool smoothRotation = true;
	public bool followBehind = true;

	void Start () 
	{
		if (!HaveTarget())
		{
			return;
		}	

		// set starting values
		distance = targetTransform.position.z - transform.position.z;
		height = transform.position.y;
	}
	
	void LateUpdate () 
	{
		if (!HaveTarget())
		{
			return;
		}	

		if (followBehind) 
		{
			targetPosition = targetTransform.TransformPoint (targetTransform.position.x, height, -distance);
		} 
		else 
		{
			targetPosition = targetTransform.TransformPoint (targetTransform.position.x, height, distance);
		}

		transform.position = Vector3.Lerp (transform.position, targetPosition, Time.deltaTime * distanceDamping);

//		if (smoothRotation) 
//		{
//			Quaternion wantedRotation = Quaternion.LookRotation (targetTransform.position - transform.position, targetTransform.up);
//
//			if (keepOriginalRotation) 
//			{
//				wantedRotation = targetRotation;
//			} 
//
//			transform.rotation = Quaternion.Slerp (transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
//		} 
//		else 
//		{
//			transform.LookAt (targetTransform, targetTransform.up);
//		}
	}

	bool HaveTarget()
	{
		bool haveTarget = true;
		if (targetTransform == null)
		{
			haveTarget = false;
			Debug.Log ("NO TARGET! I'M OUT!!!");
 		}	
		return haveTarget;
	}
}
