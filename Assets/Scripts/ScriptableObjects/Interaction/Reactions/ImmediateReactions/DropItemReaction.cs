using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemReaction : Reaction {

	public Transform monsterTransform;
	public GameObject itemObject;

	protected override void ImmediateReaction()
	{
		itemObject.transform.position = monsterTransform.position;
		itemObject.SetActive (true);
	}
}
