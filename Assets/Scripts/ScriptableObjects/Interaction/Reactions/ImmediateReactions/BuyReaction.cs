using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyReaction : Reaction {

	public ItemManager.Item item;
	public int price;

	protected override void ImmediateReaction()
	{
		Debug.Log ("BuyReact");
		StageManager sm = StageManager.Instance;
		if (sm.Money >= price) {
			sm.Money -= price;
			sm.ItemAdd (item);
		}
	}
}
