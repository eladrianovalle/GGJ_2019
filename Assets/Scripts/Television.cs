using System;
using UnityEngine;

public class Television : MonoBehaviour
{
	public static Action<bool> ShowStatic;
	public static Action<Sprite> ShowSprite;

	public static Action OnStaticStart;
	public static Action OnStaticEnd;

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

	public void SetStatic(bool enabled)
	{
		animator.SetBool("staticAnim", enabled);
		if (enabled)
		{
			if (OnStaticStart != null)
			{
				OnStaticStart();
			}
		}
		else
		{
			if (OnStaticEnd != null)
			{
				OnStaticEnd();
			}
		}
	}

	public void SetSprite(Sprite sprite = null)
	{
		sRenderer.sprite = sprite;
	}
}

