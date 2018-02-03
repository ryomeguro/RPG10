using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class InteractableScript : MonoBehaviour {

	public enum Type
	{
		shop,
		monster
	}

	public GameObject window;
	public Type type;
	public string interactName;

	protected TextMeshPro textMesh;

	protected void Start(){
		textMesh = window.transform.Find ("TextMeshPro").GetComponent<TextMeshPro>();
		window.transform.localScale = Vector3.zero;
	}

	public virtual void DisplayText (){
		TextReset ();
		iTween.ScaleTo (window, Vector3.one, 0.5f);
	}

	public virtual void CloseText(){
		iTween.ScaleTo (window, Vector3.zero, 0.5f);
	}

	public abstract void TextReset ();

}
