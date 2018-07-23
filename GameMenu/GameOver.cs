using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public void PlayMainMenu(){
		SceneManager.LoadScene("MainMenu");
		Debug.Log("playmainmenu");
	}

	public void PlayGame(){
		SceneManager.LoadScene("Game");
		Debug.Log("play again");
	}
}
