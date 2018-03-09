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
		if (Canbuy()) {
			if (price > 0) {
				SoundUtility.Instance.PlayOneShot (SoundUtility.Instance.register, 0.5f);
			}
			sm.Money -= price;
			if (weapon == ItemManager.Weapon.armor) {
				sm.WeaponDef = power;
			} else if (weapon == ItemManager.Weapon.sword) {
				sm.WeaponAtt = power;
			}
			sm.AddRecord (new WeaponRecord (weapon, power));
		}
	}

	bool Canbuy(){
		StageManager sm = StageManager.Instance;
		bool moneyFlg = sm.Money >= price;
		bool powerFlg = false;
		if (weapon == ItemManager.Weapon.armor) {
			powerFlg = sm.WeaponDef < power;
		} else if (weapon == ItemManager.Weapon.sword) {
			powerFlg = sm.WeaponAtt < power;
		}

		return moneyFlg && powerFlg;
	}
}
