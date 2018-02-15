using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofObjectScript : MonoBehaviour {

	public GameObject visibleRoof;

	void OnTriggerEnter(Collider c){
		Debug.Log ("enter");
		visibleRoof.SetActive (false);
	}

	void OnTriggerExit(Collider c){
		visibleRoof.SetActive (true);
	}
}
