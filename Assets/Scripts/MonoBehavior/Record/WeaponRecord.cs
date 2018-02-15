using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecord : Record {

	public ItemManager.Weapon weapon;
	public int power;

	public WeaponRecord(ItemManager.Weapon weapon, int power){
		this.weapon = weapon;
		this.power = power;
	}

	public override void RecordAction ()
	{
		switch (weapon) {
		case ItemManager.Weapon.sword:
			StageManager.Instance.WeaponAtt = Mathf.Max (StageManager.Instance.WeaponAtt, power);
			break;
		case ItemManager.Weapon.armor:
			StageManager.Instance.WeaponDef = Mathf.Max (StageManager.Instance.WeaponDef, power);
			break;
		}
	}
}
