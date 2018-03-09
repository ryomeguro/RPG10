using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundUtility : MonoBehaviour {

	public static SoundUtility Instance;

	public AudioSource SESouce;

	public AudioClip slash;
	public AudioClip glass;
	public AudioClip monsterDeath;
	public AudioClip herb;
	public AudioClip powerDrag;
	public AudioClip register;

	// Use this for initialization
	void Start () {
		if (Instance == null) {
			Instance = this;
		}
	}
	
	public void PlayOneShot(AudioClip clip){
		SESouce.PlayOneShot (clip);
	}
	public void PlayOneShot(AudioClip clip,float volumeScale){
		SESouce.PlayOneShot (clip,volumeScale);
	}

	public void PlayDelayed(AudioClip clip,float delay){
		StartCoroutine (PlayDelayedCoroutine (clip, delay, 1));
	}
	public void PlayDelayed(AudioClip clip,float delay, float volumeScale){
		StartCoroutine (PlayDelayedCoroutine (clip, delay, volumeScale));
	}
	IEnumerator PlayDelayedCoroutine(AudioClip clip,float delay, float volumeScale){
		yield return new WaitForSeconds (delay);
		SESouce.PlayOneShot (clip, volumeScale);
	}
}
