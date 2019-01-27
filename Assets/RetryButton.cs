using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RetryButton : MonoBehaviour, IPointerClickHandler {

	public void OnPointerClick(PointerEventData pointerEventData)
	{
		//Output to console the clicked GameObject's name and the following message. You can replace this with your own actions for when clicking the GameObject.
		Debug.Log(name + " Game Object Clicked!");
	}

}
