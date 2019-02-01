using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextObjectCallout : MonoBehaviour {

	SpriteRenderer sRenderer;
	public Sprite currentSprite;

	void OnEnable()
	{
		MomLauncher.OnMomChooseObjectPreview += ChangeSprite;
	}

	void OnDisable()
	{
		MomLauncher.OnMomChooseObjectPreview -= ChangeSprite;
	}

	void Awake () 
	{
		sRenderer = GetComponent<SpriteRenderer> ();
	}
	
	void Update () 
	{
		
	}

	public void ChangeSprite(Sprite sprite)
	{
		Debug.Log ("I'm calling you, " + sprite.name + "!!!");
		currentSprite = sprite;
		sRenderer.sprite = currentSprite;
	}
}
