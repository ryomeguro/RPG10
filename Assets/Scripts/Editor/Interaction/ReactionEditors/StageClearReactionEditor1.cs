using UnityEditor;

[CustomEditor(typeof(StageClearReaction))]
public class StageClearReactionEditor : ReactionEditor {

	protected override string GetFoldoutLabel ()
	{
		return "StageClear Reaction";
	}
}
