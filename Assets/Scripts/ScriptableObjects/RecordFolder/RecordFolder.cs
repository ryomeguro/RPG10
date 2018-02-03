using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RecordFolder : ScriptableObject {

	public List<Record> Records = new List<Record> ();

	public void Reset(){
		Records.Clear ();
	}

	public void Sort(){
		Records.Sort ((a, b) => (int)(a.time*10) - (int)(b.time*10));
	}

	public void AddRecord(Record record){
		Records.Add (record);
		//Debug.Log (this);
	}

	public override string ToString ()
	{
		string str = "";
		foreach (Record rec in Records) {
			str += rec.ToString () + ",";
		}
		return "[" + str + "]";
	}
}
