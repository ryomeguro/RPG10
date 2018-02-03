using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
public class PlayerMoveOld : MonoBehaviour {

	public RecordFolder folder;
	public float speed;

	CharacterController cc;
	bool isStart = false;
	bool hitFlg = false;

	Vector3 lastMoveDirection = Vector3.zero;
	float startTime = 0;
	//float moveStartTime = 0;

	MoveRecord record;

	InteractableScript interactable = null;

	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController> ();

		GameStart ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isStart)
			return;

		Vector3 moveDirection = Vector3.zero;

		//TEST
		if (Input.GetKeyDown (KeyCode.A)) {
			SceneManager.LoadScene (0);
		}

		//MOVE & RECORD
		if (Input.GetKey (KeyCode.LeftArrow)) {
			moveDirection = Vector3.left;
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			moveDirection = Vector3.right;
		} else if (Input.GetKey (KeyCode.UpArrow)) {
			moveDirection = Vector3.forward;
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			moveDirection = Vector3.back;
		}

		cc.Move (moveDirection * Time.deltaTime*speed);

		if (hitFlg) {
			moveDirection = Vector3.zero;
			hitFlg = false;
		}

		if (lastMoveDirection != moveDirection) {
			float nowTime = Time.realtimeSinceStartup - startTime;
			if (moveDirection == Vector3.zero && record != null) {
				record.RecordEnd(nowTime,transform.position);
				folder.AddRecord (record as Record);
				record = null;
			} else if (lastMoveDirection == Vector3.zero) {
				record = new MoveRecord (nowTime, moveDirection);
				//Debug.Log (record);
			} else {
				record.RecordEnd(nowTime,transform.position);
				folder.AddRecord (record);
				record = new MoveRecord (nowTime, moveDirection);
				//Debug.Log (record);
			}
			lastMoveDirection = moveDirection;
		}

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

	void GameStart(){
		startTime = Time.realtimeSinceStartup;
		isStart = true;
	}

	void OnControllerColliderHit(ControllerColliderHit c){
		//Debug.Log ("hit");
		hitFlg = true;
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
