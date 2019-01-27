﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {

	public TextMeshProUGUI text;
	public float startPosition_y = -1200;
	bool hasTriggered = false;

	public CanvasGroup retryPanel;
	public Button retryButton;
//	float originalPanelAlpha;

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
		HideRetryPanel ();
	}

	void ShowWinText()
	{
		if (hasTriggered)
		{
			return;
		}
		hasTriggered = true;

		text.color = Color.green;
		text.text = "YOU\nWIN";
		LeanTween.moveY (text.rectTransform, 0, 2f).setEase(LeanTweenType.easeOutBounce).setOnComplete(()=>{
			ShowRetryPanel();
		});
	}

	void ShowLoseText()
	{
		if (hasTriggered)
		{
			return;
		}
		hasTriggered = true;

		text.color = Color.red;
		text.text = "YOU\nLOSE";
		LeanTween.moveY (text.rectTransform, 0, 2f).setEase(LeanTweenType.easeOutBounce).setOnComplete(()=>{
			ShowRetryPanel();
		});	
	}

	void HideRetryPanel()
	{
		retryPanel.alpha = 0;
		retryButton.interactable = false;
	}

	void ShowRetryPanel()
	{
		LeanTween.alphaCanvas (retryPanel, 1f, 1f).setEase(LeanTweenType.easeOutExpo).setOnComplete(()=>{
//			retryPanel.blocksRaycasts = false;
			retryButton.interactable = true;

			retryButton.onClick.RemoveAllListeners();
			Debug.Log("Let's add the RestartGame func, son!!!!");
			retryButton.onClick.AddListener(()=>{
				RestartGame();
			});
		});
	}

	public void RestartGame()
	{
		Debug.Log ("Let's load the scene over!!!");
		GameController.gameOver = false;
		Scene thisScene = SceneManager.GetActiveScene ();
		SceneManager.LoadScene (thisScene.buildIndex);
	}

}