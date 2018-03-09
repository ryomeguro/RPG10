using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class StageManager : MonoBehaviour {

	public RecordFolder folder;
	public MoveRecordFolder[] moveFolders;
	public GameObject playerGhost;
	public float recordDuration;
	public AudioClip [] BGMs;

	public static StageManager Instance;

	public int Att, WeaponAtt, Def, WeaponDef, Lv;

	SortedDictionary<ItemManager.Item,int> itemList = new SortedDictionary<ItemManager.Item, int>();

	List<Record> tmpRecordFolder = new List<Record>();

	int maxHP,maxMP;
	int maxEx;
	int powerDragEffect = 0;
	int magicMp = 10;
	int magicLv = 1;
	TextMeshProUGUI informationText;
	TextMeshProUGUI parametaText;
	TextMeshProUGUI turnText;
	Image informationImage;
	Image turnImage;
	Light sunLight;
	StageInfo stageInfo;
	Record nowRecord;
	AudioSource bgmSource;

	PlayerMove playerMove;

	float time;
	int turnNumber;

	bool isStart = false;
	public int stageNumber;

	int hp = 0;
	public int HP {
		get {
			return hp;
		}
		set{ 
			if (!isStart) {
				return;
			}
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

		bgmSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isStart) {
			return;
		}

		time -= Time.deltaTime;
		if (time <= 10) {
			bgmSource.pitch = 1.2f;
		}

		if (time <= 0) {
			time = 0;
			UIUpdate ();

			LightToRed ();
			TurnEnd (false);
			return;
		}
		UIUpdate ();

		if (nowRecord.time > time) {
			nowRecord.RecordAction ();
			nowRecord = folder.GetRecord ();
			playerMove.TextReset ();
		}
	}

	public void StageInit(){
		turnNumber = 1;

		bgmSource.clip = BGMs [stageNumber - 1];

		folder.Reset ();

		Reset ();

		StartCoroutine (StageInitCoroutine ());
	}

	IEnumerator StageInitCoroutine(){
		GameObject startPanel = GameObject.Find ("StartPanel");
		yield return new WaitForSeconds (1f);
		iTween.ScaleTo (startPanel, iTween.Hash ("scale", Vector3.one, "time", 0.7f, "easetype", iTween.EaseType.easeOutBack));
		yield return new WaitForSeconds (3f);
		iTween.ScaleTo (startPanel, iTween.Hash ("scale", Vector3.zero, "time", 0.7f, "easetype", iTween.EaseType.easeInBack));
		TurnInit ();
	}

	public void TurnInit(){
		if (turnNumber != 1) {
			Reset ();
		}

		nowRecord = folder.GetRecord ();

		StartCoroutine (TurnInitCoroutine ());
	}

	IEnumerator TurnInitCoroutine(){
		//GameObject turnPanel = GameObject.Find ("TurnPanel");
		//TextMeshProUGUI textMesh = turnPanel.transform.Find ("TurnText").GetComponent<TextMeshProUGUI> ();

		turnText.text = turnNumber + "/" + stageInfo.turnNumber + "回目\n"
		                + "スタート!";
		turnImage.color = new Color (1, 1, 1, 0.83f);

		yield return new WaitForSeconds (1f);
		iTween.ScaleTo (turnImage.gameObject, iTween.Hash ("scale", Vector3.one, "time", 0.7f, "easetype", iTween.EaseType.easeOutBack));
		yield return new WaitForSeconds (2f);
		iTween.ScaleTo (turnImage.gameObject, iTween.Hash ("scale", Vector3.zero, "time", 0.4f, "easetype", iTween.EaseType.easeInBack));

		for (int i = 0; i < turnNumber - 1; i++) {
			Transform initPosition = stageInfo.initPosition;
			PlayerGhost pg = Instantiate (playerGhost, initPosition.position, initPosition.rotation).GetComponent<PlayerGhost> ();
			pg.folder = moveFolders [i];
		}

		playerMove.MoveStart ();
		bgmSource.pitch = 1f;
		bgmSource.Play ();
		isStart = true;
	}
		
	void Reset(){
		stageInfo = GameObject.Find ("StageInfo").GetComponent<StageInfo> ();
		informationText = GameObject.Find ("InformationText").GetComponent<TextMeshProUGUI> ();
		parametaText = GameObject.Find ("ParametaText").GetComponent<TextMeshProUGUI> ();
		turnText = GameObject.Find ("TurnText").GetComponent<TextMeshProUGUI> ();
		informationImage = GameObject.Find ("InformationPanel").GetComponent<Image> ();
		turnImage = GameObject.Find ("TurnPanel").GetComponent<Image> ();
		sunLight = GameObject.Find ("Directional Light").GetComponent<Light> ();

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

		UIUpdate ();
	}

	public void TurnEnd(bool death){
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

		//turnNumber++;
		//SceneController.Instance.TurnEnd ();
		StartCoroutine (TurnEndCoroutine (death));
	}

	IEnumerator TurnEndCoroutine(bool death){
		bgmSource.Stop ();

		if (death) {
			turnText.text = "ちからつきた\n"
			+ turnNumber + "/" + stageInfo.turnNumber + "回目終了";
			turnImage.color = new Color (1, 0, 0, 0.83f);
			turnText.color = Color.red;
		} else {
			turnText.text = "じかんぎれ\n"
			+ turnNumber + "/" + stageInfo.turnNumber + "回目終了";
			turnImage.color = new Color (1, 1, 1, 0.83f);
		}

		//yield return new WaitForSeconds (1f);
		iTween.ScaleTo (turnImage.gameObject, iTween.Hash ("scale", Vector3.one, "time", 0.4f, "easetype", iTween.EaseType.easeOutBack));
		yield return new WaitForSeconds (2f);
		iTween.ScaleTo (turnImage.gameObject, iTween.Hash ("scale", Vector3.zero, "time", 0.7f, "easetype", iTween.EaseType.easeInBack));

		turnNumber++;
		SceneController.Instance.TurnEnd ();
	}

	public void StageClear(){
		Debug.Log ("CLEAR!!!!");
		isStart = false;
		StartCoroutine (StageEndCoroutine(true));
	}

	void StageFailed(){
		Debug.Log ("Failed...");
		isStart = false;
		StartCoroutine (StageEndCoroutine(false));
	}
	IEnumerator StageEndCoroutine(bool clear){
		bgmSource.Stop ();

		if (clear) {
			turnText.text = "ステージ" + stageNumber + "\n"
			                + "クリアー!";
			turnImage.color = new Color (1, 1, 1, 0.83f);
			turnText.color = Color.white;
		} else {
			turnText.text = "ステージ" + stageNumber + "\n"
			                + "しっぱい...";
			turnImage.color = new Color (1, 0, 0, 0.83f);
			turnText.color = Color.red;
		}

		//yield return new WaitForSeconds (1f);
		iTween.ScaleTo (turnImage.gameObject, iTween.Hash ("scale", Vector3.one, "time", 0.4f, "easetype", iTween.EaseType.easeOutBack));
		yield return new WaitForSeconds (4f);
		//iTween.ScaleTo (turnImage.gameObject, iTween.Hash ("scale", Vector3.zero, "time", 0.7f, "easetype", iTween.EaseType.easeInBack));

		SceneController.Instance.StageEnd ();
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
				SoundUtility.Instance.PlayOneShot (SoundUtility.Instance.herb, 0.1f);
				playerMove.HerbParticle.Play ();
				HP += 30;
				break;
			case ItemManager.Item.herb2:
				HP += 100;
				break;
			case ItemManager.Item.powerDrag:
				SoundUtility.Instance.PlayOneShot (SoundUtility.Instance.powerDrag, 0.1f);
				AddRecord (new PowerDragRecord ());
				PowerDrug ();
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

	public void PowerDrug(){
		//AddRecord (new PowerDragRecord ());
		StartCoroutine (PowerDrugCoroutine ());
	}
	IEnumerator PowerDrugCoroutine(){
		powerDragEffect++;
		playerMove.PowerDragParticle.Play ();
		yield return new WaitForSeconds (10f);
		if (powerDragEffect > 0) {
			powerDragEffect--;
		}

		if (powerDragEffect == 0) {
			playerMove.PowerDragParticle.Stop ();
		}
	}

	public bool MagicAttack(){
		if (MP >= magicMp) {
			MP -= magicMp;
			//Debug.Log ("magicAttack");
			return true;
		} else {
			return false;
		}
	}

	public void AddRecord(Record record){
		record.time = time;
		tmpRecordFolder.Add (record);
		//Debug.Log ("addrecord:" + record);
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
		hp = maxHP;
		MP = maxMP;
	}

	void Death(){
		if (isStart) {
			TurnEnd (true);
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

		magicLv = level / 5 + 1;
		magicMp = 10 + (magicLv - 1) * 5; 
	}

	void LightToRed(){
		iTween.ValueTo (gameObject, iTween.Hash ("from", sunLight.color, "to", Color.red, "time", 0.3f, "onupdate", "LightColorChange", "onupdatetarget", gameObject));
	}
	void LightColorChange(Color c){
		sunLight.color = c;
	}

	public void UIUpdate(){
		string str = turnNumber + "/" + stageInfo.turnNumber + "回目\n" +
		             "残り" + TimeToString (time) + "\n" +
		             "<size=70%>HP:" + hp + "/" + maxHP + "\n" +
		             "MP:" + mp + "/" + maxMP + "</size>\n" +
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
		HPColorChange ();

		str = "<size=70%>攻撃力:" + (Att + WeaponAtt);
		if (powerDragEffect > 0) {
			str += "<size=50%>x<color=#f00>"+Mathf.Pow (1.25f, powerDragEffect).ToString("#.##") +"</color></size>";
		}
		str += "\n防御力:" + (Def + WeaponDef) + "\n" +
		       "<line-height=40%>\n"+
		       "</line-height>魔法:フレア" + IntToRoma(magicLv) + "\n" +
		       "消費MP:" + magicMp + "\n" +
		       "いりょく:" + GetMagicAttackPower();

		parametaText.text = str;
	}

	void HPColorChange(){
		Color c;
		string colorString;
		if (HP == 0) {
			c = Color.red;
			colorString = "#f00";
		} else if ((HP * 1.0f) / (maxHP * 1.0f) < 0.3f) {
			c = new Color (1f, 0.359f, 0f, 0.8f);
			colorString = "#ff5b00";
		} else {
			c = new Color (1, 1, 1, 0.8f);
			colorString = "#fff";
		}

		informationImage.color = c;
		informationText.text = "<color=" + colorString + ">" + informationText.text + "</color>";
	}

	string TimeToString(float time){
		int second = (int)time;
		if (time > 60) {
			return string.Format ("{0:0}\'{1:00}\"", second / 60, second % 60);
		} else {
			return string.Format ("{0:00}\"{1:00}", second, (int)((time - second) * 100));
		}
	}

	string IntToRoma(int value){
		string[] r = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
		int[] a = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
		string str = "";

		while (value > 0) {
			int count = 0;
			while (value < a [count]) {
				count++;
			}
			value -= a [count];
			str += r [count];
		}

		return str;
	}
}
