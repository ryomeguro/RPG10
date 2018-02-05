using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClearReaction : Reaction {

	protected override void ImmediateReaction(){
		StageManager.Instance.StageClear ();
	}
}
