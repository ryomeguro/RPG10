using UnityEngine;
using UnityEngine.UI;

public class TextReaction : Reaction
{
    public string message;
	//public Color textColor = Color.white;
	//public float delay;


    private TextManager textManager;

	private Text text;

	public string getText(){
		return message;
	}


    protected override void SpecificInit()
    {
		//textManager = FindObjectOfType<TextManager> ();

    }


    protected override void ImmediateReaction()
    {
		//textManager.DisplayMessage (message, textColor, delay);

    }
}