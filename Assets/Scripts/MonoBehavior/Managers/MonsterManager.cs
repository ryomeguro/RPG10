using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour {

	public static MonsterManager Instance;

	MonsterInteractable[] monsters;

	// Use this for initialization
	void Awake () {
		if (Instance == null) {
			Instance = this;
		}
	}

	void Start(){
		monsters = FindObjectsOfType<MonsterInteractable> ();
	}
	
	public MonsterInteractable GetMonsterInteractable(int ID){
		foreach (MonsterInteractable mi in monsters) {
			if (mi.ID == ID) {
				return mi;
			}
		}
		return null;
	}
}
