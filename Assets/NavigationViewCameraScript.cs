using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationViewCameraScript : MonoBehaviour {

    public GameObject shipToFollow;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = new Vector3(shipToFollow.transform.position.x, shipToFollow.transform.position.y, -1);
	}
}
