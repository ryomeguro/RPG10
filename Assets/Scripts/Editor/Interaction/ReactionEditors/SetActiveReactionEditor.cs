using UnityEditor;

[CustomEditor(typeof(SetEnableReaction))]
public class SetActiveReactionEditor : ReactionEditor {

	protected override string GetFoldoutLabel ()
	{
		return "StageEnable Reaction";
	}
}
