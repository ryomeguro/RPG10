using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MoveRecordFolder : ScriptableObject {

	List<Vector3> Records = new List<Vector3> ();

	public void Reset(){
		Records.Clear ();
	}

	public void AddRecord(Vector3 record){
		Records.Add (record);
		//Debug.Log (this);
	}

	public Vector3[] GetArray(){
		return Records.ToArray ();
	}

	public override string ToString ()
	{
		string str = "";
		foreach (Vector3 rec in Records) {
			str += rec.ToString () + ",";
		}
		return "[" + str + "]";
	}
}
