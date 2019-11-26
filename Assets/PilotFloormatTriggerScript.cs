using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotFloormatTriggerScript : MonoBehaviour {
	public GameObject gameModeManager;
	GameModeManagerScript gmManagerScript;

	// Use this for initialization
	void Start () {
		gmManagerScript = gameModeManager.GetComponent<GameModeManagerScript>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	/*
	public void OnTriggerEnter(Collider other){
		gmManagerScript.switchModeTo(GameModeManagerScript.GameMode.NAVIGATION);
		
	}
	*/
}
