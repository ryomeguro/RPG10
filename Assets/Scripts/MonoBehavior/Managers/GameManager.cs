using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager Instance;

	StageManager stageManager;
	//public int walkSpeed;
	//public int hp, mp, money;

	//public int lv;

	//public RecordFolder[] RecordFolders;

	// Use this for initialization
	void Awake () {
		if (Instance == null) {
			Instance = this;
		}
	}

	void Start(){
		stageManager = GameObject.FindObjectOfType<StageManager> ();
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.A)) {
			SceneController.Instance.GoToStage (1);
		}
	}
}
