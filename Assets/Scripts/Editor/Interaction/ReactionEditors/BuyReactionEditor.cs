using UnityEditor;

[CustomEditor(typeof(BuyReaction))]
public class BuyReactionEditor : ReactionEditor {

	protected override string GetFoldoutLabel ()
	{
		return "Buy Reaction";
	}
}
