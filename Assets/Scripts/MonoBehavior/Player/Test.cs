using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	Vector3 lastPos;
	void Start () {
		Vector3[] movepath = { Vector3.left, Vector3.left * 2, Vector3.forward * 5, Vector3.left * 5.1f, Vector3.left * -3 };


		iTween.MoveTo (gameObject, iTween.Hash ("path", movepath, "time", 10, "easetype",iTween.EaseType.easeOutSine));
		lastPos = transform.position;
	}

	void Update(){
		Vector3 moveDirection = transform.position - lastPos;
		if (moveDirection != Vector3.zero) {
			transform.eulerAngles = new Vector3 (0, Mathf.Atan2 (moveDirection.x, moveDirection.z) * Mathf.Rad2Deg, 0);
		}
		lastPos = transform.position;
	}
}
