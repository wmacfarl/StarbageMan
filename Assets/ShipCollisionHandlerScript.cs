using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCollisionHandlerScript : MonoBehaviour {

    public GameObject planetShipIsOrbiting;
    public int ticksOrbitingPlanet;
    public int ticksToSuccessfulOrbit;
    public bool successfulOrbit;

	// Use this for initialization
	void Start () {
        planetShipIsOrbiting = null;
        ticksOrbitingPlanet = 0;
        successfulOrbit = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (planetShipIsOrbiting != null)
        {
            ticksOrbitingPlanet++;
            if (ticksOrbitingPlanet >= ticksToSuccessfulOrbit)
            {
                successfulOrbit = true;
            }
        }
	}

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.tag == "orbit trigger")
        {
            triggerOrbit(trigger.transform.parent.gameObject);
        }
        if (trigger.tag == "planet")
        {
            crashIntoPlanet(trigger.transform.gameObject);
        }
    }

    void crashIntoPlanet(GameObject planet)
    {
        Debug.Log("crashed into planet: " + planet.name);
    }

    void triggerOrbit(GameObject planet)
    {
        planetShipIsOrbiting = planet;
    }

    private void OnTriggerExit2D(Collider2D trigger)
    {
        if (planetShipIsOrbiting != null)
        {
            if (trigger.gameObject.transform.parent == planetShipIsOrbiting.transform)
            {
                planetShipIsOrbiting = null;
                ticksOrbitingPlanet = 0;
                successfulOrbit = false;
            }
        }
    }
}
