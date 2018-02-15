using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParametaReaction : Reaction {

	public enum Type {MONEY,Ex};

	public Type type;
	public int amount;
	public bool addRecord = true;

	/*public ParametaReaction(Type type, int amount, bool addRecord){
		this.type = type;
		this.amount = amount;
		this.addRecord = addRecord;
	}*/

	public void Init(Type type, int amount, bool addRecord){
		this.type = type;
		this.amount = amount;
		this.addRecord = addRecord;
	}

	protected override void ImmediateReaction(){
		if (amount > 0 && addRecord) {
			ParametaRecord pr = new ParametaRecord (type, amount);
			StageManager.Instance.AddRecord (pr);
		}
		switch (type) {
		case Type.Ex:
			StageManager.Instance.Ex += amount;
			break;
		case Type.MONEY:
			StageManager.Instance.Money += amount;
			break;
		}
	}
}
