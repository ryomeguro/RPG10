using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerDragRecord : Record {

	public override void RecordAction(){
		StageManager.Instance.PowerDrug ();
	}
}
