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

	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == "Player")
		{
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
