using UnityEditor;

[CustomEditor(typeof(DropItemReaction))]
public class DropItemReactionEditor : ReactionEditor {

	protected override string GetFoldoutLabel ()
	{
		return "DropItem Reaction";
	}
}
