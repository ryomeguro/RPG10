using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParametaRecord : Record {

	public ParametaReaction.Type type;
	public int amount;

	public ParametaRecord(ParametaReaction.Type type,int amount){
		this.type = type;
		this.amount = amount;
	}
		
	public void RecordAction(Transform pPosition){
		switch (type) {
		case  ParametaReaction.Type.Ex:
			StageManager.Instance.Ex += amount;
			break;
		case  ParametaReaction.Type.MONEY:
			StageManager.Instance.Money += amount;
			break;
		}
	}

}
