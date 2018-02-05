using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

	public enum Item{herb,herb2,powerDrag};

	public enum Weapon
	{
		sword,armor
	}

	public static string ItemName(Item item){
		switch (item) {
		case ItemManager.Item.herb:
			return "やくそう";
		case ItemManager.Item.herb2:
			return "上やくそう";
		case ItemManager.Item.powerDrag:
			return "ちから玉";
		default:
			return "";
		}
	}
}
