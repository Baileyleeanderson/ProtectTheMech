using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour {

	private Transform target;
	private int speed;
	private int health = 2;
	private SpriteRenderer sprite;	
	[SerializeField] private AudioSource soundMng;

	[SerializeField] AudioClip[] fx;

	void Start () {
		target = GameObject.Find("Player").transform;
		speed = Random.Range(15,18);
		sprite = GetComponent<SpriteRenderer>();
		soundMng.PlayOneShot(fx[0]);
	}
	
	void Update () {
		Movement();
	}

	private void Movement (){
		if (transform.position.y >= -3.0f){
			transform.position = new Vector3( transform.position.x, -3.0f, 30);
		}
		transform.position = Vector3.MoveTowards(transform.position, target.position,speed * Time.deltaTime);
		Flip();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player" || other.tag == "Mech" || other.tag == "Grenade"){
			soundMng.PlayOneShot(fx[1]);
			Destroy(this.gameObject);
		}
		if (other.tag == "Bullet" || other.tag == "ShotGun" ){
			health -= 1;
			if (health <= 0){
				soundMng.PlayOneShot(fx[1]);
				Destroy(this.gameObject);
			}
		}
	}

	void Flip(){
		if (target.transform.position.x > transform.position.x){
			sprite.flipX = false;
		}
		else if (target.transform.position.x < transform.position.x){
			sprite.flipX = true;
		}
	}

}
