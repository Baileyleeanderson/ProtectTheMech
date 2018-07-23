using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droid : MonoBehaviour {

	[SerializeField] private Vector3[] target;
	[SerializeField] private GameObject explode;
	[SerializeField] private float speed = 10;

	private SpriteRenderer sprite;
	private bool facingRight = true;
	private int randm;


	void Start () {
		sprite = GetComponent<SpriteRenderer>();
		randm = Random.Range(0, 8);
		StartCoroutine(DroidExplode());
	}
	
	// Update is called once per frame
	void Update () {
		MoveMent();
		
	}

	void MoveMent(){
		if (transform.position == target[randm]){
			randm = Random.Range(0, 8);
		}
		Flip();
		transform.position = Vector3.MoveTowards(transform.position, target[randm], speed * Time.deltaTime);

	}

	void Flip(){
		if(target[randm].x > transform.position.x){
			sprite.flipX = false;
			facingRight = true;
		}
		else if(target[randm].x < transform.position.x){
			sprite.flipX = true;
			facingRight = false;
		}
	}

	IEnumerator DroidExplode(){
		yield return new WaitForSeconds(10);
		Instantiate(explode, transform.position, Quaternion.identity);
		Destroy(gameObject);
		GameManager.Instance.DroidIsSpawned = false;
	}

}
