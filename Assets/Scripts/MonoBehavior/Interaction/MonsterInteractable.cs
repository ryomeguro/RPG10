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

	public SkinnedMeshRenderer mesh;

	int hp;
	bool death = false;

	Transform player;
	PlayerMove playerMove;
	float attackTime = 0;

	ParticleSystem particle;
	AudioSource voice;

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
		particle = GetComponent<ParticleSystem> ();
		voice = GetComponent<AudioSource> ();
		ReactionCollections [0].MonsterInit (money, ex);
	}

	void Update(){
		if (death) {
			return;
		}
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
		if (death) {
			return;
		}
		playerMove.AttackAnimation ();

		int attackPower = StageManager.Instance.GetAttackPower ();
		int damage = CalcDamage (attackPower, def);
		if (Random.value > 0.7f) {//for Metal
			damage++;
		}
			
		//Debug.Log ("attack" + damage + ":" + "HP=" + (hp - damage));
		Damage (damage);
		StageManager.Instance.AddRecord (new AttackRecord (ID, damage));
	}

	void MagicAttacked(){
		if (death) {
			return;
		}
		if (!StageManager.Instance.MagicAttack ()) {
			return;
		}
		playerMove.MagicAttackAnimation ();

		int attackPower = StageManager.Instance.GetMagicAttackPower ();
		int damage = CalcDamage (attackPower, def);
		Damage (damage);
		StageManager.Instance.AddRecord (new AttackRecord (ID, damage));
	}

	public void Damage(int damage){
		if (death) {
			return;
		}

		if (damage > 0) {
			voice.PlayOneShot (SoundUtility.Instance.slash);
		} else {
			voice.PlayOneShot (SoundUtility.Instance.glass);
		}

		hp -= damage;
		if (hp <= 0) {
			death = true;
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

		voice.PlayOneShot (SoundUtility.Instance.monsterDeath);
		playerMove.NullInteractable (this);
		playerMove.TextReset ();

		//gameObject.SetActive (false);
		CloseText ();

		Collider[] cs = GetComponents<Collider> ();
		foreach (Collider c in cs) {
			c.enabled = false;
		}

		StartCoroutine (DeathAnimation ());

	}
	IEnumerator DeathAnimation(){
		particle.Play ();
		//yield return new WaitForSeconds (0.5f);
		mesh.enabled = false;
		yield return new WaitForSeconds (6f);
		gameObject.SetActive (false);
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

	public override void DisplayText ()
	{
		if (hp > 0) {
			base.DisplayText ();
		}
	}


}
