using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechIntroScene : MonoBehaviour {
	[SerializeField] private GameObject mech;

	void Start () {
		StartCoroutine(TurnOffMech());
	}
	

	void Update () {
		
	}

	IEnumerator TurnOffMech(){
		yield return new WaitForSeconds(5.9f);
		Destroy(mech);
	}
}
