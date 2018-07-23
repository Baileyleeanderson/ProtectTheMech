using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	private float speed = 100;
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Shoot();
	}

	void OnBecameInvisible() {
		Destroy(gameObject);
	}

	void Shoot(){
		transform.Translate(Vector3.right * speed * Time.deltaTime);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Bat" || other.tag == "Enemy1" || other.tag == "Monster" || other.tag == "Enemy" ){
			Destroy(gameObject);
		}
	}

}
