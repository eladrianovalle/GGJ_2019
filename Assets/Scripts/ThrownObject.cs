using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownObject : MonoBehaviour {

	[SerializeField]
	private Sprite previewSprite;
	public Sprite PreviewSprite 
	{ 
		get { return previewSprite; } 
		set { previewSprite = value; } 
	}

	public enum GoodOrBad
	{
		GOOD,
		BAD
	}
	public GoodOrBad isGoodOrBad;
	public bool canAffect = true; 

	public static System.Action<bool> OnObjectHit;

	void OnCollisionEnter(Collision collision)
	{
		if (!canAffect)
		{
			return;
		}

		if (collision.collider.tag == "Room")
		{
			canAffect = false;
			return;
		}

		if (collision.collider.tag == "Player")
		{
			canAffect = false;

			if (isGoodOrBad == GoodOrBad.GOOD)
			{
				if (OnObjectHit != null)
				{
					OnObjectHit (true);
				}
				if (HeadUpDisplay.DisplayMessage != null)
				{
					HeadUpDisplay.DisplayMessage("Recharged!", 1f);
				}
				GameController.PlayerGainLife ();
			}
			else if (isGoodOrBad == GoodOrBad.BAD)
			{
				if (OnObjectHit != null)
				{
					OnObjectHit (false);
				}
				GameController.PlayerLoseLife ();
			}
		}
	}

	void OnTriggerExit (Collider collider)
	{
		if (!canAffect)
		{
			return;
		}

		if (collider.tag == "NearMiss")
		{
			Debug.Log ("NEAR MISS!!!");
			if (HeadUpDisplay.DisplayMessage != null)
			{
				HeadUpDisplay.DisplayMessage("Near miss!", 1f);
			}
			if (HighScoreController.onAddToCurrentScore != null)
			{
				HighScoreController.onAddToCurrentScore(10);
			}
		}
	}
}
