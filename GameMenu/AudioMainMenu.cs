using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMainMenu : MonoBehaviour {

	[SerializeField] private AudioSource playBtn;
	[SerializeField] private AudioSource controlBtn;

 	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayBtnFx(){
		playBtn.Play();
	}
	
	public void CtrlBtnFx(){
		controlBtn.Play();
	}
}
