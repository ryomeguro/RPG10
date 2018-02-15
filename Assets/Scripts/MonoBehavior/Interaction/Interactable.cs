using UnityEngine;

public class Interactable : InteractableScript
{
	//public Transform interactionLocation;
    public ConditionCollection[] conditionCollections = new ConditionCollection[0];
    public ReactionCollection defaultReactionCollection;


    public void Interact ()
    {
        for (int i = 0; i < conditionCollections.Length; i++)
        {
			if (conditionCollections [i].CheckAndReact ()) {
				TextReset ();
				return;
			}
        }

        defaultReactionCollection.React ();
		TextReset ();
    }



	public override void TextReset(){
		for (int i = 0; i < conditionCollections.Length; i++)
		{
			string str;
			if (conditionCollections [i].CheckAndString (out str)) {
				textMesh.text = str;
				return;
			}
		}

		/*TextReaction tr = defaultReactionCollection.reactions [0] as TextReaction;
		textMesh.text = tr.GetText();*/
		textMesh.text = defaultReactionCollection.GetText ();
	}
}
