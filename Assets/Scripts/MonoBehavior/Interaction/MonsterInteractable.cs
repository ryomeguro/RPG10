using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInteractable : SelectableInteractable {

	public int hp;
	public int att;
	public int def;
	public float attackDuration;
	public float radius;

	Transform player;
	float time = 0;

	public override void Interact(int index){
		if (index == 0) {
			Attacked ();
		} else if (index == 1) {
			MagicAttacked ();
		}
	}

	void Start(){
		base.Start ();
		player = GameObject.Find ("Player").transform;
	}

	void Update(){
		if (Vector3.SqrMagnitude (player.position - transform.transform.position) < radius * radius) {
			time += Time.deltaTime;
		}

		if (time > attackDuration) {
			Attack ();
			time = 0;
		}
	}

	void Attack(){
		StageManager sm = StageManager.Instance;
		int damage = CalcDamage (att, sm.GetDefence ());
		sm.HP -= damage;
	}

	void Attacked(){
		int attackPower = StageManager.Instance.GetAttackPower ();
		int damage = CalcDamage (attackPower, def);
		if (Random.value > 0.7f) {//for Metal
			damage++;
		}
		Damage (damage);

		Debug.Log (attackPower);
	}

	void MagicAttacked(){
		int attackPower = StageManager.Instance.GetMagicAttackPower ();
		int damage = CalcDamage (attackPower, def);
		Damage (damage);
	}

	void Damage(int damage){
		hp -= damage;
		if (hp <= 0) {
			Death ();
			return;
		}
		TextReset ();

	}

	void Death(){
		Debug.Log ("Death");
		for (int i = 0; i < ReactionCollections.Length; i++) {
			ReactionCollections [i].React ();
		}
		Destroy (gameObject);
	}

	int CalcDamage(int attack,int defence){
		int damage = (int)((attack * 4 - defence * 2) * Random.Range (0.8f, 1.2f));
		return Mathf.Max (0, damage);
	}

	public override void TextReset ()
	{
		string str = interactName + "\n";
		str += "<indent=10%>HP:" + hp + "</indent>";
		textMesh.text = str;
	}


}
