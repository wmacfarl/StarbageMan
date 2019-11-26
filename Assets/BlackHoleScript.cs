using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleScript : MonoBehaviour {

    public GameObject gameModeManager;
    public GameObject planetSystem;

    // Use this for initialization
    void Start () {
        planetSystem = GameObject.Find("Planet System");
        PlanetSystemScript pss = planetSystem.GetComponent<PlanetSystemScript>();
        transform.position = Random.insideUnitCircle * pss.systemRadius*.1f;
        gameModeManager = GameObject.Find("Game Mode Manager");
    }
	
	// Update is called once per frame
	void Update () {
        if (gameModeManager.GetComponent<GameModeManagerScript>().currentMode == GameModeManagerScript.GameMode.STARMAP)
        {
            transform.localScale = new Vector3(4, 4, 1);
            transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
