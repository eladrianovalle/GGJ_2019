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
}
