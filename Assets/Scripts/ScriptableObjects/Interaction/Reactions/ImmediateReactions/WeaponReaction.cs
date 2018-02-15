using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReaction : Reaction {

	public ItemManager.Weapon weapon;
	public string name;
	public int price;
	public int power;

	protected override void ImmediateReaction()
	{
		Debug.Log ("BuyReact");
		StageManager sm = StageManager.Instance;
		if (sm.Money >= price) {
			sm.Money -= price;
			if (weapon == ItemManager.Weapon.armor) {
				StageManager.Instance.WeaponDef = power;
			} else if (weapon == ItemManager.Weapon.sword) {
				StageManager.Instance.WeaponAtt = power;
			}
			StageManager.Instance.AddRecord (new WeaponRecord (weapon, power));
		}
	}
}
