using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {


	public MoveRecordFolder folder;
	public float maxSpeed;

	CharacterController cc;
	InteractableScript interactable = null;

	Vector3 lastPosition;
	float recordDuration;
	bool isStart = false;

	float time = 0;

	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController> ();
		recordDuration = StageManager.Instance.recordDuration;

		lastPosition = transform.position;
	}

	void Update(){
		if (!isStart) {
			return;
		}

		Vector3 moveDirection = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
		if (moveDirection != Vector3.zero) {
			transform.eulerAngles = new Vector3 (0, Mathf.Atan2 (moveDirection.x, moveDirection.z) * Mathf.Rad2Deg, 0);
		}
		cc.Move (moveDirection * Time.deltaTime * maxSpeed);

		time += Time.deltaTime;
		if (time >= recordDuration) {
			RecordPosition (transform.position);
			time = 0;
		}

		//Debug.Log (Vector3.Distance (lastPosition, transform.position)/Time.deltaTime);
		lastPosition = transform.position;

		//INTERACT
		if (interactable != null) {
			SelectableInteractable sInt = interactable as SelectableInteractable;
			if (sInt != null) {
				if (Input.GetKeyDown (KeyCode.Alpha1)) {
					sInt.Interact (0);
				}else if (Input.GetKeyDown (KeyCode.Alpha2)) {
					sInt.Interact (1);
				}else if (Input.GetKeyDown (KeyCode.Alpha3)) {
					sInt.Interact (2);
				}
			} else {
				Interactable nInt = interactable as Interactable;
				if (Input.GetKeyDown (KeyCode.Alpha1)) {
					nInt.Interact ();
				}
			}
		}
	}

	public void MoveStart(){
		folder.Reset ();
		isStart = true;
	}

	void RecordPosition(Vector3 pos){
		folder.AddRecord (transform.position);
	}

	void OnTriggerEnter(Collider c){
		interactable = c.gameObject.GetComponent<InteractableScript> ();
		if (interactable != null) {
			interactable.DisplayText ();
		}
		Debug.Log (interactable);
	}

	void OnTriggerExit(Collider c){
		if (interactable != null) {
			interactable.CloseText ();
			interactable = null;
		}
	}
}