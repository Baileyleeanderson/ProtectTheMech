using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour {

	private static SceneControl instance;

		public static SceneControl Instance{
		get {
			if(instance == null){

			}
			return instance;
		}
	}

	private void Awake(){
		instance = this;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SceneToLoad(string scene){
		SceneManager.LoadScene(scene);
	}
}

