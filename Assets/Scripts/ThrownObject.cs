using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownObject : MonoBehaviour {

	public enum GoodOrBad
	{
		GOOD,
		BAD
	}
	public GoodOrBad isGoodOrBad;
	public bool canAffect = true; 

	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == "Player")
		{
			if (!canAffect)
			{
				return;
			}
			canAffect = false;

			if (isGoodOrBad == GoodOrBad.GOOD)
			{
				GameController.PlayerGainLife ();
			}
			else if (isGoodOrBad == GoodOrBad.BAD)
			{
				GameController.PlayerLoseLife ();
			}
		}
	}
}
