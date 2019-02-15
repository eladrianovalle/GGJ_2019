using UnityEngine;

public class CameraBreathing : MonoBehaviour {

	Vector3 lastLocalPosition;
	Vector3 targetLocalPosition;
    private float lowestY;

	public float breathingRadius = 0.075f;
	public float breathingDamping = 1f;

	private float currTime = 0f;

	void Start ()
	{
        lowestY = transform.localPosition.y;
		UpdateTargetPosition();
	}

	void LateUpdate ()
	{
		
		//targetPosition = new Vector3(targetTransform.position.x, transform.position.y, transform.position.z);

//		currTime += (1 - Mathf.Exp(-20 * Time.unscaledDeltaTime)) * breathingDamping;
//
//		transform.localPosition = Vector3.Lerp (lastLocalPosition, targetLocalPosition, currTime);
//		if (transform.localPosition == targetLocalPosition)
//		{
//			currTime = 0f;
//			UpdateTargetPosition();
//		}

		float breath = Mathf.Sin(Time.time * breathingDamping) * breathingRadius;
		transform.localPosition = new Vector3(transform.localPosition.x, breath, transform.localPosition.z);
		//this.transform.position = new Vector3(this.transform.position.x, posY + speed, this.transform.position.z);
	}

	void UpdateTargetPosition()
	{
		lastLocalPosition = transform.localPosition;
		Vector2 randomPosition = Random.insideUnitCircle * breathingRadius;
        float yPos = randomPosition.y;
        if (yPos < lowestY)
        {
            yPos = lowestY;
        }
        targetLocalPosition = new Vector3(randomPosition.x, yPos);
	}
}
