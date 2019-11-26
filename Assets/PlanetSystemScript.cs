using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSystemScript : MonoBehaviour {

    public float systemRadius;
    public int numberOfPlanets;

    public GameObject planetPrefab;
    public GameObject blackHolePrefab;
    public List<GameObject> planets;
    public GameObject blackHole;

	// Use this for initialization
	void Awake () {
        GeneratePlanets();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GeneratePlanets()
    {   if (blackHole != null)
        {
            GameObject.Destroy(blackHole);
        }

        foreach (GameObject planet in planets)
        {
            GameObject.Destroy(planet);
        }

        planets = new List<GameObject>();
        blackHole = GameObject.Instantiate(blackHolePrefab);
        blackHole.transform.parent = transform;
        planets.Add(blackHole);
        for (int i = 1; i < numberOfPlanets + 1; i++)
        {
            MakeNewPlanet();
        }
    }

    public void MakeNewPlanet()
    {
        planets.Add(GameObject.Instantiate(planetPrefab));
        int i = planets.Count - 1;
        planets[i].transform.parent = transform;
        planets[i].name = "planet(" + i + ")";
    }
}
