using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Mech : MonoBehaviour {

	[SerializeField] private int health;
	[SerializeField] private GameObject explode;
	[SerializeField] private Text healthText;

	[SerializeField] private SoundManager soundMng;
	[SerializeField] private AudioClip[] hurt;
	[SerializeField] private AudioClip death;
	[SerializeField] private AudioClip deathExplode;

	private Animator anim;
	private int healthNum;

	void Start () {
		anim = GetComponent<Animator>();
	}
	
	void FixedUpdate () {
		CheckHealth();
	}

	private void CheckHealth(){
		if (GameManager.Instance.PlayerIsRepairing){
			health += 10;
			GameManager.Instance.PlayerIsRepairing = false;
		}
		if (health <= 0){
			anim.Play("MechDead");
			soundMng.PlaySfx(death);
			soundMng.PlaySfx(deathExplode);
			health = 0;
			healthText.text = health + " %";
			GameManager.Instance.MechIsAlive = false;
			StartCoroutine(Exploding());
		}
		else {
			healthText.text = health + " %";
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		int randomDamage = Random.Range(4, 8);

		if (other.tag == "EnemyAttack"){
			health -= randomDamage;
			Debug.Log("health" + health);
			int randFx = Random.Range(0,2);
			soundMng.PlaySfx(hurt[randFx]);
		}
		if (other.tag == "Bat"){
			health -= 1;
			int randFx = Random.Range(0,2);
			soundMng.PlaySfx(hurt[randFx]);
		}
		
	}

	IEnumerator Exploding(){
		yield return (3);
		SceneManager.LoadScene("GameOver");
		
	}
	
}
