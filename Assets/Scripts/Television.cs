using System;
using UnityEngine;

public class Television : MonoBehaviour
{
	public static Action<bool> PlayStatic;

	[SerializeField]
	private Animator animator;

	void OnEnable()
	{
		Television.PlayStatic += SetStatic;	
	}

	void OnDisable()
	{
		Television.PlayStatic -= SetStatic;
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
		animator.Play(enabled ? "StaticAnim" : "Idle");
	}
}

