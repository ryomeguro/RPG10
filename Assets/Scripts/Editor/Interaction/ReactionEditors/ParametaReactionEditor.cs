using UnityEditor;

[CustomEditor(typeof(ParametaReaction))]
public class ParametaReactionEditor : ReactionEditor {

	protected override string GetFoldoutLabel ()
	{
		return "Parameta Reaction";
	}
}
