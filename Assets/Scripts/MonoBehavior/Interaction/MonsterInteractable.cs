using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInteractable : SelectableInteractable {

	public int maxHP;
	public int att;
	public int def;
	public int ex;
	public int money;
	public float attackDuration;
	public float radius;
	public int ID;

	int hp;

	Transform player;
	PlayerMove playerMove;
	float attackTime = 0;

	public override void Interact(int index){
		if (index == 0) {
			Attacked ();
		} else if (index == 1) {
			MagicAttacked ();
		}
	}

	void Start(){
		base.Start ();
		Reset ();
		GameObject p = GameObject.Find ("Player");
		player = p.transform;
		playerMove = p.GetComponent<PlayerMove> ();
		ReactionCollections [0].MonsterInit (money, ex);
	}

	void Update(){
		if (Vector3.SqrMagnitude (player.position - transform.transform.position) < radius * radius) {
			attackTime += Time.deltaTime;
		}

		if (attackTime > attackDuration) {
			Attack ();
			attackTime = 0;
		}
	}

	void Attack(){
		StageManager sm = StageManager.Instance;
		int damage = CalcDamage (att, sm.GetDefence ());
		sm.HP -= damage;
		//Debug.Log (damage);
	}

	void Attacked(){
		if (hp <= 0) {
			return;
		}
		playerMove.Attack ();

		int attackPower = StageManager.Instance.GetAttackPower ();
		int damage = CalcDamage (attackPower, def);
		if (Random.value > 0.7f) {//for Metal
			damage++;
		}
		Damage (damage);
		StageManager.Instance.AddRecord (new AttackRecord (ID, damage));
		//Debug.Log (attackPower);
	}

	void MagicAttacked(){
		int attackPower = StageManager.Instance.GetMagicAttackPower ();
		int damage = CalcDamage (attackPower, def);
		Damage (damage);
	}

	public void Damage(int damage){
		hp -= damage;
		if (hp <= 0) {
			hp = 0;
			Death ();
			return;
		}
		TextReset ();

	}

	void Death(){
		for (int i = 0; i < ReactionCollections.Length; i++) {
			ReactionCollections [i].React ();
		}
		//Destroy (gameObject);
		//player.GetComponent<PlayerMove> ().NullInteractable (this);
		gameObject.SetActive (false);

		CloseText ();
	}

	void Reset(){
		hp = maxHP;
		attackTime = 0;
	}

	int CalcDamage(int attack,int defence){
		int damage = (int)((attack / 2 - defence / 4) * Random.Range (0.8f, 1.2f));
		return Mathf.Max (0, damage);
	}

	public override void TextReset ()
	{
		string str = interactName + "\n";
		str += "<indent=10%>HP:" + hp + "</indent>\n";
		str += TextUtility.NumberSprite(1) + "攻撃" + TextUtility.NumberSprite(2) + "攻撃魔法";
		textMesh.text = str;
	}


}
