using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInteractable : SelectableInteractable {

	public override void TextReset(){
		if (type == InteractableScript.Type.shop) {
			string str = interactName + "\n<indent=5%>";
			for (int i = 0; i < ReactionCollections.Length; i++) {
				str += TextUtility.NumberSprite (i + 1) + ItemString (ReactionCollections [i].reactions [0]);
			}
			str += "</indent>";
			textMesh.text = str;
		}
	}

	string ItemString(Reaction re){
		BuyReaction bReaction = re as BuyReaction;
		if (bReaction != null) {
			string str = ItemManager.ItemName(bReaction.item) + ":$" + bReaction.price;
			if (!CanBuy (bReaction)) {
				str = "<color=#999999>" + str + "</color>";
			}
			str += "\n";
			return str;
		}
		WeaponReaction wReaction = re as WeaponReaction;
		if (wReaction != null) {
			string str = wReaction.name + ":$" + wReaction.price;
			if (!CanBuy (wReaction)) {
				str = "<color=#999999>" + str + "</color>";
			}
			str += "\n";
			return str;
		}
		return "";
	}

	bool CanBuy(BuyReaction bReaction){
		return StageManager.Instance.Money >= bReaction.price;
	}

	bool CanBuy(WeaponReaction wReaction){
		int currentPower = 0;
		if (wReaction.weapon == ItemManager.Weapon.armor) {
			currentPower = StageManager.Instance.WeaponDef;
		}else if (wReaction.weapon == ItemManager.Weapon.sword){
			currentPower = StageManager.Instance.WeaponAtt;
		}
		return (StageManager.Instance.Money >= wReaction.price) && (currentPower < wReaction.power);
	}
}
