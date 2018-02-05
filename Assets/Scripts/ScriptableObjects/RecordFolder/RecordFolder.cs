using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RecordFolder : ScriptableObject {

	public List<Record> Records = new List<Record> ();
	int index = 0;

	public void Reset(){
		Records.Clear ();
		index = 0;
		ParametaRecord pr = new ParametaRecord (ParametaReaction.Type.Ex, 0);
		pr.time = -100;
		AddRecord (pr);
	}

	public void Sort(){
		Records.Sort ((a, b) => - (int)(a.time*10) + (int)(b.time*10));
		index = 0;
	}

	public void AddRecord(Record record){
		Records.Add (record);
		//Debug.Log (this);
	}

	public Record GetRecord(){
		return Records [index++];
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
