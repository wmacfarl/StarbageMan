using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiationChamberParticleSystemScript : MonoBehaviour {

    ParticleSystem particleSystem;
    public GameObject garbageHoldPanel;
    public int myHoldIndex;
	// Use this for initialization
	void Start () {
        particleSystem = GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        float myHeat = garbageHoldPanel.GetComponent<GarbageHoldPanelScript>().chamberHeats[myHoldIndex];
        var emission = particleSystem.emission;
        emission.rateOverTime = myHeat*2;
        var main = particleSystem.main;
        main.startColor = Color.Lerp(Color.green, Color.red, myHeat / 360);	
	}
}
