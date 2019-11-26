using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGarbageScript : MonoBehaviour {

    public GameObject gameModeManager;
	// Use this for initialization
	void Start () {
        gameModeManager = GameObject.Find("Game Mode Manager");

    }

    // Update is called once per frame
    void Update () {
	    if (gameModeManager.GetComponent<GameModeManagerScript>().currentMode == GameModeManagerScript.GameMode.STARMAP)
        {
            transform.localScale = new Vector3(4, 4, 1);
        }else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
	}
}
