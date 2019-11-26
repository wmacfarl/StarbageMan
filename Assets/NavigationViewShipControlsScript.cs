using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationViewShipControlsScript : MonoBehaviour {

    public float maxSpeed;
    public Vector3 velocity;
    public Vector2 heading;
    public float rotationSpeed;
    public float thrusterPower;
    public GameObject gameModeManager;
    ShipCollisionHandlerScript collisionHandler;
    public float orbitAngle;
    public float orbitSpeed;
    public float orbitRadius;
    public GameObject planetSystem;
    public float gravity = .1f;
    public float minGravityDistance;
    public float tangentMultiplier;
    public float orbitDamping;
    public int garbagePickupRate;
    int ticksUntilNextPickup;
    int garbageCount;
    public GameObject planetShipIsOrbiting;
    public GameObject spaceGarbage;
    public int[] garbageHoldStatus;
    float garbageListFactor = .0001f;
    public GameObject topDownShipObject;
    float ticksThrustHeldInOrbit = 0;
    float baseListingFactor = .001f;
		
		public bool shipStill = true;
		
		public Sprite fire1;
		public Sprite fire2;
		public Sprite fire3;
		public Sprite fire4;
		
		List<Sprite> fires;
		
		int currentFireInt = 0;
		public Sprite currentFire;
		
		GameObject fireObject;
		SpriteRenderer fireSpriteRenderer;
		
		int fireFrames = 0;
		

    void Start () {
        collisionHandler = GetComponent<ShipCollisionHandlerScript>();
        ticksUntilNextPickup = 0;
        garbageCount = 0;
        garbageHoldStatus = new int[3];

        garbageHoldStatus[0] = 0;
        garbageHoldStatus[1] = 0;
        garbageHoldStatus[2] = 0;
				
				//This should not be here.
        float startingGarbageCount = 0;
        if (GameObject.Find("TutorialManager").GetComponent<TutorialManagerScript>().tutorialProgression < 3)
        {
            startingGarbageCount = 5;
        }
        
        //generate some starting garbage
        for (int i = 0; i < startingGarbageCount    ; i++)
        {
            garbageHoldStatus[1]++;
            topDownShipObject.GetComponent<GarbageHoldTopDownScript>().getGarbage(1);
        }
				
				placeShipNotOnPlanet();
				
				fires = new List<Sprite>();
				
				fires.Add(fire1);
                fires.Add(fire2);
				fires.Add(fire3);
				fires.Add(fire4);
				
				fireObject = GameObject.Find("ShipFire");
				//Debug.Log(fireObject);
				currentFire = fires[0];
				fireSpriteRenderer = fireObject.GetComponent<SpriteRenderer>();
				fireSpriteRenderer.sprite = currentFire;
				
				//GetComponent<Image>().sprite =

    }
		
		void placeShipNotOnPlanet()
		{
			PlanetSystemScript planetSystemScript = planetSystem.GetComponent<PlanetSystemScript>();
			if (planetSystemScript.planets.Count > 0)
			{
				transform.position = planetSystemScript.planets[0].transform.position + (Vector3) Random.insideUnitCircle*35f;
			} else {
				transform.position = Vector3.zero;
			}
			if ( collisionHandler.planetShipIsOrbiting != null) 
			{
				placeShipNotOnPlanet();
			}
		}

    // Update is called once per frame
    void Update()
    {
        bool breakOutOfOrbitThisFrame = false;
        planetShipIsOrbiting = null;
        if (collisionHandler.successfulOrbit)
        {
            if (Input.GetKey("w") || (Input.GetKey(KeyCode.UpArrow))) {
                ticksThrustHeldInOrbit++;

                if (ticksThrustHeldInOrbit > 40){
                    breakOutOfOrbitThisFrame = true;
                }
            }

            if (!breakOutOfOrbitThisFrame)
            {
                planetShipIsOrbiting = collisionHandler.planetShipIsOrbiting;
                Vector3 dif = collisionHandler.planetShipIsOrbiting.transform.position - transform.position;
                dif.z = 0;
                float distance = dif.sqrMagnitude;
                dif.Normalize();

                Vector3 tangent1 = new Vector2(-dif.y, dif.x);
                Vector3 tangent2 = new Vector2(dif.y, -dif.x);


                tangent1.z = 1;
                tangent2.z = 1;

                Vector2 tangent1Difference = tangent1 - velocity;
                Vector2 tangent2Difference = tangent2 - velocity;


                if (tangent1Difference.magnitude > tangent2Difference.magnitude)
                {
                    velocity += tangent2;
                    float angle = Mathf.Atan2(tangent2.y, tangent2.x) * Mathf.Rad2Deg - 90;
                    transform.eulerAngles = new Vector3(0, 0, angle);
                }
                else
                {
                    velocity += tangent1;
                    float angle = Mathf.Atan2(tangent1.y, tangent1.x) * Mathf.Rad2Deg - 90;
                    transform.eulerAngles = new Vector3(0, 0, angle);
                }

                if (distance < orbitRadius)
                {
                    Vector3 force = dif * 10 * -gravity / distance;
                    velocity += force;
                }

                velocity *= orbitDamping;
                ticksUntilNextPickup--;

                if (ticksUntilNextPickup < 0)
                {
                    if (planetShipIsOrbiting.GetComponent<PlanetScript>())
                    {
                        pickupGarbage();
                    }
                    else
                    {
                        if (planetShipIsOrbiting.GetComponent<BlackHoleScript>())
                        {
                            TutorialManagerScript tManScript = GameObject.Find("TutorialManager").GetComponent<TutorialManagerScript>();
                            if (tManScript.tutorialProgression == tManScript.INPROCTARISORBIT)
                            {
                                tManScript.orbitBlackHole();
                            }



                            if (Input.GetKeyDown(KeyCode.Space) && gameModeManager.GetComponent<GameModeManagerScript>().currentMode == GameModeManagerScript.GameMode.NAVIGATION)
                            {
                                shootGarbage();
                                if (tManScript.tutorialProgression == tManScript.INPROCTARISORBIT)
                                {
                                    tManScript.promoteToRegionalManager();
                                }
                            }
                        }
                    }
                }
            }
        }else
        {
            ticksThrustHeldInOrbit = 0;
        }

        float currentSpeed = velocity.magnitude;

        float percentOfMaxSpeed = 0;
        if (currentSpeed != 0)
        {
            percentOfMaxSpeed = currentSpeed / maxSpeed;
        }

        if (gameModeManager.GetComponent<GameModeManagerScript>().currentMode == GameModeManagerScript.GameMode.NAVIGATION)
            {
                transform.Rotate(Vector3.back, rotationSpeed * Input.GetAxis("Horizontal"));
                if (Input.GetAxis("Vertical") > 0)
                {
                    velocity += transform.up * thrusterPower * Input.GetAxis("Vertical");
                }
            }
						if (planetSystem.GetComponent<PlanetSystemScript>().planets.Count > 0){
       				foreach (GameObject planet in planetSystem.GetComponent<PlanetSystemScript>().planets)
      				{
								if (planet != null){
            	 Vector3 dif =  planet.transform.position - transform.position;
            	 dif.z = 0;
            	 float distance = dif.sqrMagnitude;
            	 dif.Normalize();
            	 if (distance < minGravityDistance) 
							 	{
                distance = minGravityDistance;
            		}
            		Vector3 force = dif * gravity / distance;
            		velocity += force;
        			 }	
						 }
						}
					
        if (velocity.magnitude > maxSpeed)
        {
            velocity *= maxSpeed / velocity.magnitude;
        }

        velocity += new Vector3(velocity.y, -velocity.x) * (baseListingFactor + garbageHoldStatus[2]*garbageListFactor-garbageHoldStatus[0]*garbageListFactor);
				if (!shipStill)
				{
        	transform.position += velocity;
				}
        if (Mathf.Abs(transform.position.x) > planetSystem.GetComponent<PlanetSystemScript>().systemRadius)
        {
            transform.position = new Vector3(transform.position.x * -1, transform.position.y);
        }
        if (Mathf.Abs(transform.position.y) > planetSystem.GetComponent<PlanetSystemScript>().systemRadius)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y*-1);
        }

				if (Input.GetKey("w") || (Input.GetKey(KeyCode.UpArrow)))
				{
					fireFrames++;
					if (fireFrames > 4)
					{
						fireObject.SetActive(true);
						changeFire();
						fireFrames = 0;
					}
				} else {
					fireObject.SetActive(false);
					fireFrames = 0;
				}

    }
		

		//is this going to shoot one garbage from each hold?
		//would that be a problem?
    void shootGarbage()
    {
        for(int i = 0; i < garbageHoldStatus.Length; i++)
        {
            if (garbageHoldStatus[i] > 0)
            {
                GameObject newGarbage = GameObject.Instantiate(spaceGarbage);
                newGarbage.transform.position = transform.position + transform.up;
                newGarbage.GetComponent<SpaceGarbageScript>().velocity = transform.up*.3f;
                newGarbage.GetComponent<SpaceGarbageScript>().blackHole = planetShipIsOrbiting;
                garbageHoldStatus[i]--;
                topDownShipObject.GetComponent<GarbageHoldTopDownScript>().loseGarbage(i);

                Debug.Log("garbage hold [" + i + "]: " + garbageHoldStatus[i]);
								
								

                break;
								
								
            }
        }
    }
		
		void emptyGarbage()
		{
			topDownShipObject.GetComponent<GarbageHoldTopDownScript>().emptyGarbage();
		}

    void pickupGarbage()
    {
        ticksUntilNextPickup = garbagePickupRate;
        if (collisionHandler.planetShipIsOrbiting.GetComponent<PlanetScript>().removeGarbage())
        {
            garbageHoldStatus[1]++;
            topDownShipObject.GetComponent<GarbageHoldTopDownScript>().getGarbage(1);
						
						TutorialManagerScript tManScript = GameObject.Find("TutorialManager").GetComponent<TutorialManagerScript>();
						if (tManScript.tutorialProgression == tManScript.TOPROCTARIS)
						{
							tManScript.orbitProctaris();
						}
        }
    }
		
		void changeFire()
		{
			currentFireInt++;
			if (currentFireInt >= 4)
			{
				currentFireInt = 0;
			}
			currentFire = fires[currentFireInt];
			fireSpriteRenderer.sprite = currentFire;
			
		}
}
