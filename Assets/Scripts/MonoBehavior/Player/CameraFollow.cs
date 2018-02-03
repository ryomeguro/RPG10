using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public float smoothSpeed;

	Transform target;
	Vector3 offset;

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag("Player").transform;
		offset = transform.position - target.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = Vector3.Lerp (transform.position, target.position + offset, smoothSpeed * Time.deltaTime);
	}
}
