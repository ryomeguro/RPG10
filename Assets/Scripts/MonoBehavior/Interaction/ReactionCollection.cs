using UnityEngine;

public class ReactionCollection : MonoBehaviour
{
    public Reaction[] reactions = new Reaction[0];

	TextReaction textReaction;
	int textIndex = 0;

    private void Start ()
    {
        for (int i = 0; i < reactions.Length; i++)
        {
            DelayedReaction delayedReaction = reactions[i] as DelayedReaction;

            if (delayedReaction)
                delayedReaction.Init ();
            else
                reactions[i].Init ();
        }

		if (reactions.Length > 0) {
			textReaction = reactions [0] as TextReaction;
		} 

		if(textReaction == null){
			textIndex = -1;
		}
    }


    public void React ()
    {
		for (int i = textIndex + 1; i < reactions.Length; i++)
        {
			TextReaction tmpText = reactions [i] as TextReaction;
			if (tmpText != null) {
				textReaction = tmpText;
				textIndex = i;
				return;
			}

            DelayedReaction delayedReaction = reactions[i] as DelayedReaction;

            if(delayedReaction)
                delayedReaction.React (this);
            else
                reactions[i].React (this);
        }
    }

	public void MonsterInit(int money,int ex){
		Reaction[] tmpReactions = new Reaction[reactions.Length + 2];
		for (int i = 0; i < reactions.Length; i++) {
			tmpReactions [i] = reactions [i];
		}

		//ParametaReaction exRecord = new ParametaReaction (ParametaReaction.Type.Ex, ex, false);
		//ParametaReaction moneyRecord = new ParametaReaction (ParametaReaction.Type.MONEY, money, false);

		ParametaReaction exRecord = ScriptableObject.CreateInstance<ParametaReaction>();
		exRecord.Init(ParametaReaction.Type.Ex, ex, false);
		ParametaReaction moneyRecord = ScriptableObject.CreateInstance<ParametaReaction>();
		moneyRecord.Init(ParametaReaction.Type.MONEY, money, false);

		tmpReactions [reactions.Length + 1] = exRecord;
		tmpReactions [reactions.Length] = moneyRecord;
		reactions = tmpReactions;
	}

	public string GetText(){
		if (textReaction != null) {
			return textReaction.GetText ();
		} else {
			return "";
		}
	}
}
