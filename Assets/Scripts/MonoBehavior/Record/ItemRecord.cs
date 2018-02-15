using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRecord : Record {

	ItemManager.Item item;

	public ItemRecord(ItemManager.Item item){
		this.item = item;
	}

	public override void RecordAction ()
	{
		StageManager.Instance.ItemAdd (item);
	}
}
