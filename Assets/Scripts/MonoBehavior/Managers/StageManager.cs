using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StageManager : MonoBehaviour {

	public RecordFolder folder;
	public MoveRecordFolder[] moveFolders;
	public GameObject playerGhost;
	public float recordDuration;

	public static StageManager Instance;

	public int Att, WeaponAtt, Def, WeaponDef, Lv;

	SortedDictionary<ItemManager.Item,int> itemList = new SortedDictionary<ItemManager.Item, int>();

	int maxHP,maxMP;
	int maxEx;
	TextMeshProUGUI informationText;
	StageInfo stageInfo;

	PlayerMove playerMove;

	float time;
	int turnNumber;

	bool isStart = false;

	int hp;
	public int HP {
		get {
			return hp;
		}
		set{ 
			hp = value;
			UIUpdate ();
		}
	}

	int mp;
	public int MP {
		get {
			return mp;
		}
		set{ 
			mp = value;
			UIUpdate ();
		}
	}

	int money;
	public int Money {
		get {
			return money;
		}
		set{ 
			money = value;
			UIUpdate ();
		}
	}

	int experience;
	public int Ex {
		get {
			return experience;
		}
		set{ 
			while (value > 10) {
				value -= 10;
				LevelUp ();
			}
			experience = value;
		}
	}

	// Use this for initialization
	void Awake(){
		if (Instance == null) {
			Instance = this;
		}
	}

	void Start () {
		itemList.Add (ItemManager.Item.herb, 0);
		itemList.Add (ItemManager.Item.herb2, 0);
		itemList.Add (ItemManager.Item.powerDrag, 0);

		//StageInit ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isStart) {
			return;
		}

		time -= Time.deltaTime;
		if (time <= 0) {
			time = 0;
			UIUpdate ();
			TurnEnd ();
			return;
		}
		UIUpdate ();
	}

	public void StageInit(){
		//stageInfo = GameObject.Find ("StageInfo").GetComponent<StageInfo> ();
		//informationText = GameObject.Find ("InformationText").GetComponent<TextMeshProUGUI> ();
		turnNumber = 1;

		AllConditions.Instance.Reset ();
		TurnInit ();

		isStart = true;
	}

	public void TurnInit(){
		stageInfo = GameObject.Find ("StageInfo").GetComponent<StageInfo> ();
		informationText = GameObject.Find ("InformationText").GetComponent<TextMeshProUGUI> ();

		ParamCalc (stageInfo.startLv);
		time = stageInfo.turnDuration;
		AllRecover ();
		Money = stageInfo.startMoney;

		playerMove = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMove> ();
		playerMove.folder = moveFolders [turnNumber - 1];

		for (int i = 0; i < turnNumber - 1; i++) {
			Transform initPosition = stageInfo.initPosition;
			PlayerGhost pg = Instantiate (playerGhost, initPosition.position, initPosition.rotation).GetComponent<PlayerGhost> ();
			pg.folder = moveFolders [i];
		}

		playerMove.MoveStart ();
		isStart = true;
	}

	public void TurnEnd(){
		isStart = false;
		turnNumber++;
		SceneController.Instance.TurnEnd ();
	}

	public void StageEnd(){

	}

	public void UIUpdate(){
		string str = turnNumber + "/" + stageInfo.turnNumber + "回目\n" +
		             "残り" + TimeToString (time) + "\n" +
		             "<size=70%><color=#80FF3B>HP</color>:" + hp + "/" + maxHP + "\n" +
		             "<color=#FFC43B>MP</color>:" + mp + "/" + maxMP + "</size>\n" +
		             "$" + Money + "\n" +
		             "<size=90%>レベル:" + Lv + "</size>\n" +
		             "<size=50%>次のレベルまで</size>\n" +
		             "Ex:" + (maxEx - experience) + "\n" +
		             "<size=70%>";

		foreach (ItemManager.Item key in itemList.Keys) {
			str += "O" + ItemManager.ItemName(key) + "x" + itemList[key] + "\n";
		}
		str += "</size>";
		informationText.text = str;
	}

	public void ItemAdd(ItemManager.Item item){
		itemList [item]++;
		UIUpdate ();
	}
	public bool ItemUse(ItemManager.Item item){
		bool flg;
		if (itemList [item] > 0) {
			itemList [item]--;
			flg = true;
		} else {
			flg = false;
		}
		UIUpdate ();
		return flg;
	}

	public int GetAttackPower(){
		return Att + WeaponAtt;
	}
	public int GetMagicAttackPower(){
		return Att * 2;
	}

	public int GetDefence(){
		return Def + WeaponDef;
	}

	public void AllRecover(){
		HP = maxHP;
		MP = maxMP;
	}

	void LevelUp(){
		Lv++;
		ParamCalc (Lv);
		AllRecover ();
	}

	void ParamCalc(int level){
		if (level == 1) {
			maxHP = 20;
			maxMP = 10;
			Att = 5;
			Def = 5;
			maxEx = 10;
			return;
		}

		maxHP = 20 + level * 5;
		maxMP = 10 + level * 3;
		Att = 5 + level * 4;
		Def = 5 + level * 3;
		maxEx = 10 + level * 20;
	}

	string TimeToString(float time){
		int second = (int)time;
		if (time > 60) {
			return string.Format ("{0:0}\'{1:00}\"", second / 60, second % 60);
		} else {
			return string.Format ("{0:00}\"{1:00}", second, (int)((time - second) * 100));
		}
	}
}
