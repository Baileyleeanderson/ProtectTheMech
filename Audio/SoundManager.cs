using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	[SerializeField] private AudioSource audioSrc;

	public void PlaySfx(AudioClip clip){
		audioSrc.clip = clip;
		audioSrc.Play();
	}
}
