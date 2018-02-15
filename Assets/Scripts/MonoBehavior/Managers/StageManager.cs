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

	List<Record> tmpRecordFolder = new List<Record>();

	int maxHP,maxMP;
	int maxEx;
	int powerDragEffect = 0;
	TextMeshProUGUI informationText;
	StageInfo stageInfo;
	Record nowRecord;

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
			hp = Mathf.Min(maxHP, value);
			if (hp <= 0) {
				hp = 0;
				Death ();
			}
			UIUpdate ();
		}
	}

	int mp;
	public int MP {
		get {
			return mp;
		}
		set{ 
			mp = Mathf.Min (maxMP, value);
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
			while (value >= maxEx) {
				value -= maxEx;
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

		if (nowRecord.time > time) {
			nowRecord.RecordAction ();
			nowRecord = folder.GetRecord ();
		}
	}

	public void StageInit(){
		//stageInfo = GameObject.Find ("StageInfo").GetComponent<StageInfo> ();
		//informationText = GameObject.Find ("InformationText").GetComponent<TextMeshProUGUI> ();
		turnNumber = 1;

		folder.Reset ();

		TurnInit ();

		isStart = true;
	}

	public void TurnInit(){
		stageInfo = GameObject.Find ("StageInfo").GetComponent<StageInfo> ();
		informationText = GameObject.Find ("InformationText").GetComponent<TextMeshProUGUI> ();

		AllConditions.Instance.Reset ();
		ItemReset ();
		Lv = stageInfo.startLv;
		ParamCalc (Lv);
		time = stageInfo.turnDuration;
		AllRecover ();
		Money = stageInfo.startMoney;
		WeaponAtt = 0;
		WeaponDef = 0;
		Ex = 0;
		powerDragEffect = 0;

		playerMove = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMove> ();
		playerMove.folder = moveFolders [turnNumber - 1];

		nowRecord = folder.GetRecord ();

		for (int i = 0; i < turnNumber - 1; i++) {
			Transform initPosition = stageInfo.initPosition;
			PlayerGhost pg = Instantiate (playerGhost, initPosition.position, initPosition.rotation).GetComponent<PlayerGhost> ();
			pg.folder = moveFolders [i];
		}

		playerMove.MoveStart ();
		isStart = true;
	}

	public void TurnEnd(){
		if (turnNumber >= stageInfo.turnNumber) {
			StageFailed ();
			return;
		}

		isStart = false;
		playerMove.isStart = false;

		foreach (Record rec in tmpRecordFolder) {
			folder.AddRecord (rec);
		}
		folder.Sort ();
		tmpRecordFolder.Clear ();

		turnNumber++;
		SceneController.Instance.TurnEnd ();
	}

	public void StageClear(){
		Debug.Log ("CLEAR!!!!");
		isStart = false;
	}

	void StageFailed(){
		Debug.Log ("Failed...");
		isStart = false;
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
		int i = 4;
		foreach (ItemManager.Item key in itemList.Keys) {
			str += TextUtility.NumberSprite(i++) + ItemManager.ItemName(key) + "x" + itemList[key] + "\n";
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

			switch (item) {
			case ItemManager.Item.herb:
				HP += 30;
				break;
			case ItemManager.Item.herb2:
				HP += 100;
				break;
			case ItemManager.Item.powerDrag:
				StartCoroutine (PowerDrug ());
				break;
			}
		} else {
			flg = false;
		}
		UIUpdate ();
		return flg;
	}
	void ItemReset(){
		foreach(ItemManager.Item key in itemList.Keys){
			itemList [key] = 0;
		}
	}

	public IEnumerator PowerDrug(){
		AddRecord (new PowerDragRecord ());
		powerDragEffect++;
		yield return new WaitForSeconds (2f);
		if (powerDragEffect > 0) {
			powerDragEffect--;
		}
	}

	public void AddRecord(Record record){
		record.time = time;
		tmpRecordFolder.Add (record);
	}

	public int GetAttackPower(){
		return (int)((Att + WeaponAtt) * Mathf.Pow (1.25f, powerDragEffect));
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

	void Death(){
		if (isStart) {
			TurnEnd ();
		}
	}

	void LevelUp(){
		Lv++;
		ParamCalc (Lv);
		AllRecover ();
	}

	void ParamCalc(int level){
		int initMaxHP = 20;
		int initMaxMP = 10;
		int initAtt = 10;
		int initDef = 5;
		int initmaxEx = 10;

		if (level == 1) {
			maxHP = initMaxHP;
			maxMP = initMaxMP;
			Att = initAtt;
			Def = initDef;
			maxEx = initmaxEx;
			return;
		}

		maxHP = initMaxHP + level * 5;
		maxMP = initMaxMP + level * 3;
		Att = initAtt + level * 4;
		Def = initDef + level * 3;
		maxEx = initmaxEx + level * 20;
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
