using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetScript : MonoBehaviour
{

    public float chanceToProduceGarbage;

    int garbageCount;
    public int maxGarbage;
    public GameObject planetSystem;
    public GameObject planetGarbage;
    List<GameObject> myGarbage;
    int ticksToProduceGarbage;
    public int garbageFrequency;
    public GameObject gameModeManager;

    // Use this for initialization
    void Awake()
    {
        planetSystem = GameObject.Find("Planet System");
        PlanetSystemScript pss = planetSystem.GetComponent<PlanetSystemScript>();
        transform.position = Random.insideUnitCircle * pss.systemRadius*.8f;
        garbageCount = 0;
        myGarbage = new List<GameObject>();
        gameModeManager = GameObject.Find("Game Mode Manager");
    }

    // Update is called once per frame
    void Update()
    {
        ticksToProduceGarbage--;
        if (ticksToProduceGarbage < 0)
        {
            if (Random.value < chanceToProduceGarbage)
            {
                garbageCount++;
                GameObject newGarbage = GameObject.Instantiate(planetGarbage);
                myGarbage.Add(newGarbage);
                newGarbage.transform.parent = transform;
                newGarbage.transform.localPosition = Random.insideUnitCircle * 2;
                ticksToProduceGarbage = garbageFrequency;
                if (garbageCount > maxGarbage)
                {
                    explode();
                }
            }
        }

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

    public bool removeGarbage()
    {
        if (myGarbage.Count > 0)
        {
            GameObject removedGarbage = myGarbage[myGarbage.Count - 1];
            myGarbage.Remove(removedGarbage);
            GameObject.Destroy(removedGarbage);
            garbageCount--;
            return true;
        }else
        {
            return false;
        }
    }

    void explode()
    {
        foreach (GameObject g in myGarbage)
        {
            GameObject.Destroy(g);
        }
        planetSystem.GetComponent<PlanetSystemScript>().planets.Remove(gameObject);
        if (planetSystem.GetComponent<PlanetSystemScript>().planets.Count < 2)
        {
					GameModeManagerScript gManager = GameObject.Find("Game Mode Manager").GetComponent<GameModeManagerScript>();
          //gManager.areDead = true;
					gManager.dieByPlanet();
            //gameModeManager.GetComponent<GameModeManagerScript>().areDead = true;
						//GameObject.Find("TutorialManager").GetComponent<TutorialManagerScript>().triggerBossMessage("deathFromPlanet");
        }
        GameObject.Destroy(gameObject);
    }
}
