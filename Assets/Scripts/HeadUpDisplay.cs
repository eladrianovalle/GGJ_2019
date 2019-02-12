using System;
using TMPro;
using UnityEngine;

public class HeadUpDisplay : MonoBehaviour
{
	[SerializeField]
	private TextMeshPro oldMessageText;

	private string message;
	private float timer;

	/// <summary>
	/// Call to display message for a duration.
	/// </summary>
	public static Action<string, float> DisplayMessage;

	private void Start()
	{
		oldMessageText.alpha = 0f;
	}

	private void SetMessage(string msg)
	{
		message = msg;
		oldMessageText.text = message;
	}

	private void ShowMessageDuration(string msg, float dur)
	{
		SetMessage(msg);
		if (dur <= 0)
		{
			timer = 0;
			return;
		}
		oldMessageText.alpha = 1f;
		timer = dur;
	}

	private void OnEnable()
	{
		DisplayMessage += ShowMessageDuration;
	}

	private void OnDisable()
	{
		DisplayMessage -= ShowMessageDuration;
	}

	private void Update()
	{
		if (timer <= 0)
		{
			return;
		}

		timer -= Time.unscaledDeltaTime;
		if (timer <= 0)
		{
			oldMessageText.alpha = 0f;
		}
	}

}