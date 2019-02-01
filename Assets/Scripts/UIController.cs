using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {
	public AK.Wwise.Event titleOpeningSound;
	public AK.Wwise.Event winSound;
	public AK.Wwise.Event loseSound;
	public TextMeshProUGUI text;

	public float startPosition_y = -1200;
	bool hasTriggered = false;

	bool panelShowing = false;
	bool buttonPressToRestart = false;

	public CanvasGroup retryPanel;
	public Button retryButton;
	TextMeshProUGUI buttonText;

	public static System.Action OnStartGame;

	void OnEnable()
	{
		GameController.OnPlayerWinGame 		+= ShowWinText;
		GameController.OnPlayerLoseGame 	+= ShowLoseText;
		GameController.OnPressButtonToStart += RestartButtonPressed;
	}

	void OnDisable()
	{
		GameController.OnPlayerWinGame 		-= ShowWinText;
		GameController.OnPlayerLoseGame		-= ShowLoseText;
		GameController.OnPressButtonToStart -= RestartButtonPressed;

	}

	void Start()
	{
		Vector3 startingPos = new Vector3 (this.transform.position.x, startPosition_y, this.transform.position.z);
		text.transform.position = startingPos;
		buttonText = retryButton.GetComponentInChildren<TextMeshProUGUI> ();
		SetupButtons ();
		panelShowing = true;

		titleOpeningSound.Post(gameObject);
	}

	void Update()
	{
		if (!panelShowing)
		{
			return;
		}
		if (buttonPressToRestart)
		{
			buttonPressToRestart = false;

			if (buttonText.text == "START")
			{
				HideRetryPanel();
			}
			else if (buttonText.text == "RETRY")
			{
				RestartGame();
			}
		}
	}

	public void RestartButtonPressed()
	{
		buttonPressToRestart = true;
	}

	void SetupButtons()
	{
		buttonText.text = "START";
		retryButton.onClick.RemoveAllListeners();
		retryButton.onClick.AddListener(()=>{
			HideRetryPanel();
		});
		panelShowing = true;
	}

	void ShowWinText()
	{
		if (hasTriggered)
		{
			return;
		}

		winSound.Post(gameObject);
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
		
		loseSound.Post(gameObject);
		hasTriggered = true;
		text.color = Color.red;
		text.text = "YOU\nLOSE";
		LeanTween.moveY (text.rectTransform, 0, 2f).setEase(LeanTweenType.easeOutBounce).setOnComplete(()=>{
			LeanTween.delayedCall(1f, ()=>{
				ShowRetryPanel();
			});
		});	
	}

	void HideRetryPanel()
	{
		panelShowing = false;
		LeanTween.alphaCanvas (retryPanel, 0f, 1f).setEase(LeanTweenType.easeOutExpo).setOnComplete(()=>{
			retryButton.interactable = false;
			OnStartGame();
		});
	}

	void ShowRetryPanel()
	{
		panelShowing = true;
		buttonText.text = "RETRY";
		LeanTween.alphaCanvas (retryPanel, 1f, 1f).setEase(LeanTweenType.easeOutExpo).setOnComplete(()=>{
			retryButton.interactable = true;
			GameController.gameReadyToRestart = false;

			retryButton.onClick.RemoveAllListeners();
			Debug.Log("Let's add the RestartGame func, son!!!!");
			retryButton.onClick.AddListener(()=>{
				RestartGame();
			});
		});
	}

	public void RestartGame()
	{
		panelShowing = false;
		Debug.Log ("Let's load the scene over!!!");
		Scene thisScene = SceneManager.GetActiveScene ();
		SceneManager.LoadScene (thisScene.buildIndex);
	}
}