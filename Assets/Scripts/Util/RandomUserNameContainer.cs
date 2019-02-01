using UnityEngine;
using SubjectNerd.Utilities;

[CreateAssetMenu (menuName="Username list")]
public class RandomUserNameContainer : ScriptableObject
{
	[Reorderable]
	public string[] userNames;
}
