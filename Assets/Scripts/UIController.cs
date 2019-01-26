using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour {

	public TextMeshProUGUI text;
	public float startPosition_y = -1200;
	bool hasTriggered = false;

	void OnEnable()
	{
		GameController.OnPlayerWinGame 		+= ShowWinText;
		GameController.OnPlayerLoseGame 	+= ShowLoseText;
	}

	void OnDisable()
	{
		GameController.OnPlayerWinGame 		-= ShowWinText;
		GameController.OnPlayerLoseGame		-= ShowLoseText;
	}

	void Start()
	{
		Vector3 startingPos = new Vector3 (this.transform.position.x, startPosition_y, this.transform.position.z);
		text.transform.position = startingPos;
	}

	void ShowWinText()
	{
		if (hasTriggered)
		{
			return;
		}
		hasTriggered = true;

		LeanTween.moveY (text.rectTransform, 0, 2f).setEase(LeanTweenType.easeOutBounce);
		text.color = Color.green;
		text.text = "YOU\nWIN";
	}

	void ShowLoseText()
	{
		if (hasTriggered)
		{
			return;
		}
		hasTriggered = true;

		LeanTween.moveY (text.rectTransform, 0, 2f).setEase(LeanTweenType.easeOutBounce);
		text.color = Color.red;
		text.text = "YOU\nLOSE";
	}

}
