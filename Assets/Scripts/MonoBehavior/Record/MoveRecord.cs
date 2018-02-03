using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRecord : Record {

	public Vector3 direction;

	public float endTime;
	public Vector3 endPosition;

	public MoveRecord(float time, Vector3 direction){
		this.time = time;
		this.direction = direction;
	}

	public void RecordEnd(float endTime,Vector3 endPosition){
		this.endTime = endTime;
		this.endPosition = endPosition;
	}

	/*public void RecordAction(Transform pPosition){
		MonoBehaviour.
	}

	IEnumerator MoveAction(){

	}*/
}
