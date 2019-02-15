using UnityEngine;

public class CameraBreathing : MonoBehaviour {

	Vector3 lastLocalPosition;
	Vector3 targetLocalPosition;
    private float lowestY;

	public float breathingRadius = 0.3f;
	public float breathingDamping = 0.2f;

	private float currTime = 0f;
    Vector3 velocity = Vector3.zero;
    //public float dampTime = 0.3f;

	void Start ()
	{
        lowestY = transform.localPosition.y;
		UpdateTargetPosition();
	}

	void FixedUpdate ()
	{

        //targetPosition = new Vector3(targetTransform.position.x, transform.position.y, transform.position.z);

        currTime += (1 - Mathf.Exp(-20 * MH_Time.fixedTimestep)) * breathingDamping;
        //currTime += Time.fixedDeltaTime * breathingDamping;

        //transform.localPosition = Vector3.SmoothDamp(lastLocalPosition, targetLocalPosition, ref velocity, dampTime);

        //transform.localPosition = Vector3.Lerp (lastLocalPosition, targetLocalPosition, currTime);
		if (transform.localPosition == targetLocalPosition)
		{
			currTime = 0f;
			UpdateTargetPosition();
		}
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
