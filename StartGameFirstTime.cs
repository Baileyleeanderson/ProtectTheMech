using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGameFirstTime : MonoBehaviour {

	[SerializeField] private Text introText;
	[SerializeField] private AudioSource evilLaugh;
	[SerializeField] private AudioClip evilClip;
	[SerializeField] private AudioSource explosion;

	// Use this for initialization
	void Start () {
		StartCoroutine(StartIntro());
		StartCoroutine(PlayExplosion());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator PlayExplosion(){
		yield return new WaitForSeconds(5.9f);
		explosion.Play();
	}

	IEnumerator StartIntro(){
		yield return new WaitForSeconds(5.5f);
		introText.text = "Your  suit  is  linked  to  your  Mechs  Ai.";
		yield return new WaitForSeconds(3.4f);
		introText.text = "If  it  dies  you  die...";
		yield return new WaitForSeconds(3.0f);
		introText.text = "You  must  protect  your  mech  until  reinforcements  arrive.";
		yield return new WaitForSeconds(4f);
		introText.text = "Just  FYI,";
		yield return new WaitForSeconds(3f);
		introText.text = "there  are  no  reinforcements...";
		yield return new WaitForSeconds(1.2f);
		evilLaugh.clip = evilClip;
		evilLaugh.Play();
		yield return new WaitForSeconds(2.3f);
		SceneManager.LoadScene("Game");
	}
}
