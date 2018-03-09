using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhost : MonoBehaviour {

	public MoveRecordFolder folder;
	public Animator animator;
	public float maxSpeed;

	bool isStart = false;
	Vector3 lastPosition;

	Vector3 []records;

	int speedHash;

	// Use this for initialization
	void Start () {
		//speed = GameManager.Instance.walkSpeed;
		records = folder.GetArray ();
		lastPosition = transform.position;
		speedHash = Animator.StringToHash ("Speed");
		MoveStart ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 moveDirection = transform.position - lastPosition;
		if (moveDirection != Vector3.zero) {
			transform.eulerAngles = new Vector3 (0, Mathf.Atan2 (moveDirection.x, moveDirection.z) * Mathf.Rad2Deg, 0);
		}

		float speed = Vector3.Distance (lastPosition, transform.position) / Time.deltaTime;
		animator.SetFloat (speedHash, speed / maxSpeed);
		lastPosition = transform.position;
	}

	void MoveStart(){
		isStart = true;

		if (records.Length > 0) {
			//float time = records.Length * StageManager.Instance.recordDuration;
			float time = (records.Length + 1) * StageManager.Instance.recordDuration;
			iTween.MoveTo (gameObject, iTween.Hash ("path", records, "time", time, "easetype", iTween.EaseType.linear));
		}
	}

	/*void Action(Record rec){
		MoveRecord mRec = rec as MoveRecord;
		if (mRec != null) {
			StartCoroutine (MoveAction (mRec));
		} else {
			rec.RecordAction ();
		}
	}

	IEnumerator MoveAction(MoveRecord mRec){
		float duration = mRec.endTime - mRec.time;
		float moveTime = 0;
		float distance = Vector3.Distance (transform.position, mRec.endPosition);
		speed = distance / duration;
		while (mRec.endTime > nowTime) {
		//while(duration > moveTime){
			//Debug.Log ("nowTime=" + nowTime+":"+mRec.endTime);
			transform.position += mRec.direction * speed * Time.deltaTime;
			moveTime += Time.deltaTime;
			yield return null;
		}
	}*/
}
