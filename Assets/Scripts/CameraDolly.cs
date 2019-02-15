using UnityEngine;

public class CameraDolly : MonoBehaviour {

	public Transform targetTransform;
	Vector3 targetPosition;
	//Quaternion targetRotation;

	float distance; 	// distance in the z plane
	float height;		// height in the y plane

	//public float heightDamping = 2.0f;
	//public float distanceDamping = 10.0f;//0.6f;
	//public float rotationDamping = 10.0f;
    public float followDamping = 10.0f;

    //public float breathingRadius = 0.5f;

	public bool keepOriginalRotation = true;
	//public bool smoothRotation = true;
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

		targetPosition = new Vector3(targetTransform.position.x, height, targetTransform.position.z - distance);

		transform.position = Vector3.Lerp (transform.position, targetPosition, (1 - Mathf.Exp(-20 * Time.unscaledDeltaTime)) * followDamping);

        //(1 - Mathf.Exp(-20 * Time.unscaledDeltaTime))     crazy algorithm to replace vanilla time.deltatime
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
