using UnityEngine;

public class HandheldCharacter// : MonoBehaviour
{
	/// <summary>
	/// Character states.
	/// </summary>
	public enum CharacterState
	{
		FALLING,
		STANDING,
		JUMPING
	}

	[SerializeField]
	private const CharacterState START_STATE = CharacterState.STANDING;

	public CharacterState CurrentState { get; private set; }

	/// <summary>
	/// Init the startState.
	/// </summary>
	/// <param name="startState">Start state.</param>
	public void Init(CharacterState startState = START_STATE)
	{
		CurrentState = startState;
	}


	/// <summary>
	/// Sets the state.
	/// </summary>
	/// <param name="newState">New state.</param>
	public void SetState(CharacterState newState = CharacterState.STANDING)
	{
		CurrentState = newState;
	}


	/// <summary>
	/// Determines whether the character is jumping.
	/// </summary>
	/// <returns><c>true</c> if the character is jumping; otherwise, <c>false</c>.</returns>
	public bool IsJumping()
	{
		return CurrentState == CharacterState.JUMPING;
	}

	/// <summary>
	/// Determines whether the character is standing.
	/// </summary>
	/// <returns><c>true</c> if the character is standing; otherwise, <c>false</c>.</returns>
	public bool IsStanding()
	{
		return CurrentState == CharacterState.STANDING;
	}

	/// <summary>
	/// Determines whether the character is falling.
	/// </summary>
	/// <returns><c>true</c> if the character is falling; otherwise, <c>false</c>.</returns>
	public bool IsFalling()
	{
		return CurrentState == CharacterState.FALLING;
	}


	/// <summary>
	/// Tries to jump.
	/// </summary>
	/// <returns><c>true</c>, if jump was successful, <c>false</c> otherwise.</returns>
	public bool TryJump()
	{
		if (CurrentState == CharacterState.STANDING)
		{
			CurrentState = CharacterState.JUMPING;
			return true;
		}

		return false;
	}

	/// <summary>
	/// Tries to fall.
	/// </summary>
	/// <returns><c>true</c>, if fall was successful, <c>false</c> otherwise.</returns>
	public bool TryFall()
	{
		if (CurrentState == CharacterState.STANDING)
		{
			CurrentState = CharacterState.FALLING;
			return true;
		}

		return false;
	}

	/// <summary>
	/// Tries to return to standing.
	/// </summary>
	/// <returns><c>true</c>, if stand was successful, <c>false</c> otherwise.</returns>
	public bool TryStand()
	{
		if (CurrentState != CharacterState.FALLING)
		{
			CurrentState = CharacterState.STANDING;
			return true;
		}

		return false;
	}

}