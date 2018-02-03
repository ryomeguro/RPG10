using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableInteractable : InteractableScript{

	public ReactionCollection [] ReactionCollections;

	public virtual void Interact(int index){
		if(index >= 0 && index < ReactionCollections.Length){
			ReactionCollections [index].React ();
		}
	}

	public override void TextReset(){
		if (type == InteractableScript.Type.shop) {
			string str = interactName + "\n<indent=5%>";
			for (int i = 0; i < ReactionCollections.Length; i++) {
				BuyReaction reaction = ReactionCollections [i].reactions [0] as BuyReaction;
				if (reaction != null) {
					str += ItemManager.ItemName(reaction.item) + ":$" + reaction.price + "\n";
				}
			}
			str += "</indent>";
			textMesh.text = str;
		}
	}

}
