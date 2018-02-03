using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToCamera : MonoBehaviour {

	Transform camera;
	// Use this for initialization
	void Start () {
		camera = GameObject.FindGameObjectWithTag ("MainCamera").transform; 
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.LookAt (camera.position);
	}
}
