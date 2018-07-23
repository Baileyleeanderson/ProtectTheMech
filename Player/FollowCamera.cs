using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

	public Transform player;
	
	void Start () {
		transform.position = new Vector3(player.transform.position.x, transform.position.y, 10);
    }
    
    void LateUpdate () 
    {   
        transform.position = new Vector3(player.transform.position.x, transform.position.y, 1.7f);
    }

}
