using UnityEditor;

[CustomEditor(typeof(WeaponReaction))]
public class WeaponReactionEditor : ReactionEditor {

	protected override string GetFoldoutLabel ()
	{
		return "Weapon Reaction";
	}
}
