using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEnableReaction : Reaction {

	public GameObject gObject;
	public bool activeFlg;

	protected override void ImmediateReaction()
	{
		gObject.SetActive (activeFlg);
	}
}
