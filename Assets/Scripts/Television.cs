using System;
using UnityEngine;

public class Television : MonoBehaviour
{
	public static Action<bool> ShowStatic;
	public static Action<Sprite> ShowSprite;

	[SerializeField]
	private Animator animator;
	[SerializeField]
	private SpriteRenderer sRenderer;

	void OnEnable()
	{
		Television.ShowStatic += SetStatic;	
		Television.ShowSprite += SetSprite;
	}

	void OnDisable()
	{
		Television.ShowStatic -= SetStatic;	
		Television.ShowSprite -= SetSprite;
	}

	void Start()
	{
		if (animator == null)
		{
			gameObject.SetActive(false);
		}
	}

	public void SetStatic(bool enabled = true)
	{
		//animator.Play(enabled ? "StaticAnim" : "Idle");
		animator.SetBool("staticAnim", enabled);
	}

	public void SetSprite(Sprite sprite = null)
	{
		sRenderer.sprite = sprite;
	}
}

