using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour {

	void Start () {
		StartCoroutine(DestroyExplode());
	}

	IEnumerator DestroyExplode(){
		yield return new WaitForSeconds(1f);
		Destroy(gameObject);
		Debug.Log("Coroutine started");
	}
}
