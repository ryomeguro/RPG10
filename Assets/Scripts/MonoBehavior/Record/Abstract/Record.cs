using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Record {

	public float time;

	public virtual void RecordAction (){
	}

	public override string ToString(){
		return time + "";
	}
}
