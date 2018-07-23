using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	[SerializeField] private int health;
	[SerializeField] private float speed = 5;
	[SerializeField] private GameObject[] items;
	[SerializeField] private Transform spawnPos;
	[SerializeField] private Transform repairSpawnPos;
	[SerializeField] private AudioSource soundMng;

	[SerializeField] private AudioClip[] fx;

	private Transform target;
	private Transform targetDroid;
	private SpriteRenderer sprite;
	private Animator anim;
	private BoxCollider2D boxCollider;
	private Rigidbody2D rigid;

	private int forceAmount = 8;
	private bool enemyKilled = false;
	private int randm;
	private string currentTarget = "Player";

	void Start () {
		anim = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();
		boxCollider = GetComponent<BoxCollider2D>();
		rigid = GetComponent<Rigidbody2D>();
		target = GameObject.Find("Player").transform;
		currentTarget = "Player";
		boxCollider.enabled = false;

		StartCoroutine(EnableCollision());

		if (this.tag == "Enemy"){
			speed = Random.Range(6, 9);
		}
		else if (this.tag == "Enemy1"){
			speed = Random.Range(4, 6);
		}
		else if (this.tag == "Monster"){
			speed = Random.Range(3, 8);
		}
	}
	IEnumerator EnableCollision(){
		yield return new WaitForSeconds(2);
		boxCollider.enabled = true;
	}
	
	void FixedUpdate ()	{
		if (!enemyKilled){
			if (GameManager.Instance.DroidIsSpawned){
				target = GameObject.Find("Droid(Clone)").transform;
				currentTarget = "Droid";
				Movement();
				rigid.Sleep();
			}
			else if (!GameManager.Instance.DroidIsSpawned){
				if (currentTarget == "Mech"){
					target = GameObject.Find("Mech").transform;
					Movement();	
				}
				else {
					target = GameObject.Find("Player").transform;
					currentTarget = "Player";
					Movement();	
					rigid.WakeUp();
				}
			}
		}
	}

	//________________________________________Movement

	void Movement(){
		if (transform.position.y >= -3.8f){
			transform.position = new Vector3 (transform.position.x, -3.8f, 30);
		}
		if (transform.position.y <= -5.8f){
			transform.position = new Vector3 (transform.position.x, -5.8f, 30);
		}
		
		transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
		Flip();	
	}

	void Flip(){
		if (target.transform.position.x > transform.position.x){
			sprite.flipX = false;
		}
		else if (target.transform.position.x < transform.position.x){
			sprite.flipX = true;
		}
	}

	//________________________________________Collider

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player" && currentTarget == "Player"){
			anim.SetBool("Attack", true);
			soundMng.PlayOneShot(fx[2]);
			target = GameObject.Find("Player").transform;
		}
		if (other.tag == "Bullet"){
			Destroy(other.gameObject);
			if (this.health <= 0 ){
				boxCollider.enabled = false;
				Death();	
			}
			else {
				forceAmount = 8;
				Damage(1);
				soundMng.PlayOneShot(fx[0]);
			}
		}
		if (other.tag == "ShotGun"){
			Destroy(other.gameObject);
			if (this.health <= 0 ){
				boxCollider.enabled = false;
				Death();	
			}
			else {
				forceAmount = 12;
				Damage(15);
				soundMng.PlayOneShot(fx[0]);
			}
		}
		if (other.tag == "Sword"){
			if (this.health <= 0 ){
				rigid.Sleep();
				boxCollider.enabled = false;
				Death();	
			}
			else {
				forceAmount = 30;
				Damage(10);
				soundMng.PlayOneShot(fx[0]);
			}
		}
		if (other.tag == "Grenade"){
			Death();
			forceAmount = 90;
			if (sprite.flipX){
				rigid.AddForce(Vector2.right * forceAmount, ForceMode2D.Impulse);
				StartCoroutine(StopForce());
			}
			else if (!sprite.flipX){
				rigid.AddForce(Vector2.left * forceAmount, ForceMode2D.Impulse);
				StartCoroutine(StopForce());
			}
			
		}
		if (other.tag == "Mech" && currentTarget == "Player"){
			target = GameObject.Find("Mech").transform;
			anim.SetBool("Attack", true);
			soundMng.PlayOneShot(fx[2]);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		anim.SetBool("Attack", false);
	}

	IEnumerator IsDead(){
		yield return new WaitForSeconds(1.0f);
		Destroy(this.gameObject);
	}

	//________________________________________Death/ Damage

	void Death(){
		soundMng.PlayOneShot(fx[1]);
		randm = Random.Range(0, 3);
		if (randm == 1 || randm == 2){
			int newRandom = Random.Range(0,7);
			if (newRandom == 5 || newRandom == 6){
				if (GameManager.Instance.PlayerHasWrench == false){
					Instantiate(items[newRandom], repairSpawnPos.position , Quaternion.identity);
				}
				else if (GameManager.Instance.PlayerHasWrench == true){
					Instantiate(items[3], spawnPos.position , Quaternion.identity);
				}
			}
			else if (newRandom != 5 || newRandom != 6){
				Instantiate(items[newRandom], spawnPos.position , Quaternion.identity );
			}
		}
		boxCollider.enabled = false;
		rigid.Sleep();
		anim.SetTrigger("Death");
		StartCoroutine(IsDead());
		enemyKilled = true;
		GameManager.Instance.TotalEnemies -= 1;
	}

	void Damage(int amount){
		this.health -= amount;
		if (health >= 0){
			if (sprite.flipX){
				rigid.AddForce(Vector2.right * forceAmount, ForceMode2D.Impulse);
				StartCoroutine(StopForce());
			}
			else if (!sprite.flipX){
				rigid.AddForce(Vector2.left * forceAmount, ForceMode2D.Impulse);
				StartCoroutine(StopForce());
			}
		}
		else {
			Death();
		}
	}

	IEnumerator StopForce(){
		yield return new WaitForSeconds(0.2f);
			rigid.Sleep();
	}


}
