using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRecord : Record {

	int ID;
	int damage;

	public AttackRecord(int ID,int damage){
		this.ID = ID;
		this.damage = damage;
	}

	public override void RecordAction(){
		//Debug.Log ("AttackRecord");
		MonsterInteractable mi = MonsterManager.Instance.GetMonsterInteractable (ID);
		if (mi != null) {
			mi.Damage (damage);
			//Debug.Log (damage);
		}
	}
}
