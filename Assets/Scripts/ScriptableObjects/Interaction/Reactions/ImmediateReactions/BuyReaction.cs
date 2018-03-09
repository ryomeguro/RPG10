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
			if (price > 0) {
				SoundUtility.Instance.PlayOneShot (SoundUtility.Instance.register, 0.5f);
			}
			sm.Money -= price;
			sm.ItemAdd (item);
			sm.AddRecord (new ItemRecord (item));
		}
	}
}
