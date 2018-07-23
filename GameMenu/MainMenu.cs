using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	[SerializeField] GameObject controlScreen;
	// Use this for initialization
	void Start () {
		
	}
	
	public void EnterGame(){
		SceneManager.LoadScene("FirstStartGame");
	}

	public void ShowControls(){
		controlScreen.SetActive(true);
	}
	public void HideControls(){
		Debug.Log("clicked");
		controlScreen.SetActive(false);
	}

}
