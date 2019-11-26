using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceGarbageScript : MonoBehaviour {

    public Vector3 velocity;
    public GameObject blackHole;
    public float gravity;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(velocity);
				if (blackHole != null){
        	Vector3 dif = blackHole.transform.position - transform.position;
        	dif.z = 0;
        	float distance = dif.sqrMagnitude;
        	if (distance < 1)
        	{
						TutorialManagerScript tManScript = GameObject.Find("TutorialManager").GetComponent<TutorialManagerScript>();
						if (tManScript.tutorialProgression == tManScript.ORBITINGBLACKHOLE)
						{
							tManScript.promoteToRegionalManager();
						}
            GameObject.Destroy(gameObject);
        	}
        	if (distance < 5)
        	{
            distance = 5;
        	}
        	dif.Normalize();
        	velocity += (gravity / distance) * dif;
        	velocity *= .95f;
				} else {
						GameObject.Destroy(gameObject);
				}


    }
}
