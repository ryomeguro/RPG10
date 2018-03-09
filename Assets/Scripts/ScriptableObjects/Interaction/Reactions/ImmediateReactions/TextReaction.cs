using UnityEngine;
using UnityEngine.UI;

public class TextReaction : Reaction
{
    public string message;
	//public Color textColor = Color.white;
	//public float delay;
	public AudioClip sound;
	public float volume = 1;


    private TextManager textManager;

	private Text text;

	public string GetText(){
		return message;
	}
	public AudioClip GetSound(){
		return sound;
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