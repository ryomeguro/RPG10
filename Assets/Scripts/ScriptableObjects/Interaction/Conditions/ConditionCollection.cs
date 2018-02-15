using UnityEngine;

public class ConditionCollection : ScriptableObject
{
	public string description;
	public Condition[] requiredConditions = new Condition[0];
	public ReactionCollection reactionCollection;

    public bool CheckAndReact()
    {
		for (int i = 0; i < requiredConditions.Length; i++) {
			if (!AllConditions.CheckCondition (requiredConditions [i])) {
				return false;
			}
		}

		if (reactionCollection) {
			reactionCollection.React ();
		}
        return true;
    }

	public bool CheckAndString(out string str){
		str = "";

		for (int i = 0; i < requiredConditions.Length; i++) {
			if (!AllConditions.CheckCondition (requiredConditions [i])) {
				return false;
			}
		}

		if (reactionCollection) {
			/*TextReaction tr = reactionCollection.reactions [0] as TextReaction;
			if (tr != null) {
				str = tr.GetText ();
				return true;
			} else {
				return false;
			}*/
			str = reactionCollection.GetText ();
			return true;
		}
		return false;
	}
}