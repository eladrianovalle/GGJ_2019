using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextObjectCallout : MonoBehaviour {

	SpriteRenderer sRenderer;
	Sprite currentSprite;



	void Awake () 
	{
		sRenderer = GetComponent<SpriteRenderer> ();
	}
	
	void Update () 
	{
		
	}

	public void ChangeSprite(Sprite sprite)
	{
		currentSprite = sprite;
		sRenderer.sprite = currentSprite;
	}
}
