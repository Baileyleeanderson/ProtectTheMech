using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {

	[SerializeField] private GameObject explode;
	
	void Start () {
		
	}
	
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player" || other.tag == "Mech" || other.tag == "Bullet"){
			
		}
		else {
			Instantiate(explode, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
	
}
