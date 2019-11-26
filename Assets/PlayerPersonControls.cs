using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPersonControls : MonoBehaviour {
  public float speed;
  public Vector3 velocity;
  public Vector2 raycastOffset;
    public bool inStorageChamber;
    GameObject currentGarbageHold = null;
    public int currentStorageChamberNumber;
    List<GameObject> pelletsBeingHeld;

  int garbageHoldingCount;

  public GameObject gameModeManager;
	GameModeManagerScript gmManagerScript;
	
	public List<bool> arrows;
	
	Rigidbody2D rigidBody;
	
	float xScale;
	
	public int leftRightFacing;
	
	AnimationScript aScript;
    public float raycastDistance = .7f;
	bool inPilotFloorMat = false;
	bool inUpperFloorMat = false;
	bool inLowerFloorMat = false;
	
	TutorialManagerScript tManScript;
	
	public int buttonPresses = 0;
	
	
	// Use this for initialization
	void Start () {
    inStorageChamber = false;
		arrows.Add(false); arrows.Add(false); arrows.Add(false); arrows.Add(false);
		speed = .2f;
		rigidBody = this.GetComponent<Rigidbody2D>();
		leftRightFacing = 1;
		xScale = transform.localScale.x;
		aScript = this.GetComponent<AnimationScript>();
    pelletsBeingHeld = new List<GameObject>();
		
		gmManagerScript = gameModeManager.GetComponent<GameModeManagerScript>();
		
		tManScript = GameObject.Find("TutorialManager").GetComponent<TutorialManagerScript>();
		//inPilotFloorMat = false;
	}
	
	// Update is called once per frame
	void Update () {
    if (pelletsBeingHeld.Count > 0)
    {
        GetComponent<AnimationScript>().holdingWaste = true;
    } else
    {
        GetComponent<AnimationScript>().holdingWaste = false;
    }

    if (gameModeManager.GetComponent<GameModeManagerScript>().currentMode == GameModeManagerScript.GameMode.TOPDOWN && GameObject.Find("Boss Canvas") == null){


			if ( (Input.GetKeyDown("w") ) || (Input.GetKeyDown(KeyCode.UpArrow) ) ){
				arrows[0] = true;
				if (tManScript.tutorialProgression == tManScript.BACKTOSHIPCONTROLS)
				{
					buttonPresses++;
				}
			}
			if ( (Input.GetKeyDown("a") ) || (Input.GetKeyDown(KeyCode.LeftArrow) ) ){
				arrows[1] = true;	
				if (tManScript.tutorialProgression == tManScript.BACKTOSHIPCONTROLS)
				{
					buttonPresses++;
				}
			}
			if ( (Input.GetKeyDown("s") ) || (Input.GetKeyDown(KeyCode.DownArrow) ) ){
				arrows[2] = true;
				if (tManScript.tutorialProgression == tManScript.BACKTOSHIPCONTROLS)
				{
					buttonPresses++;
				}	
			}
			if ( (Input.GetKeyDown("d") ) || (Input.GetKeyDown(KeyCode.RightArrow) ) ){
				arrows[3] = true;
				if (tManScript.tutorialProgression == tManScript.BACKTOSHIPCONTROLS)
				{
					buttonPresses++;
				}	
			}
		
			if ( (Input.GetKeyUp("w") ) || (Input.GetKeyUp(KeyCode.UpArrow) ) ){
				arrows[0] = false;	
			}
			if ( (Input.GetKeyUp("a") ) || (Input.GetKeyUp(KeyCode.LeftArrow) ) ){
				arrows[1] = false;	
			}
			if ( (Input.GetKeyUp("s") ) || (Input.GetKeyUp(KeyCode.DownArrow) ) ){
				arrows[2] = false;	
			}
			if ( (Input.GetKeyUp("d") ) || (Input.GetKeyUp(KeyCode.RightArrow) ) ){
				arrows[3] = false;	
			}
			


      if (Input.GetKeyDown(KeyCode.Space))
      {
          tryToPickUpGarbage();
      }

			float newX = 0; float newY = 0;
			
			if (arrows[0]){ //w
				newY = 1;
				//transform.position.y += speed;
			}
			if (arrows[1]){ //a
				newX = -1;
				leftRightFacing = -1;
				//transform.position.x -= speed;
			}
			if (arrows[2]){ //s
				newY = -1;
				//transform.position.y -= speed;
			}
			if (arrows[3]){ //d
				newX = 1;
				leftRightFacing = 1;
				//transform.position.x += speed;
			}
			
			rigidBody.MovePosition(new Vector2(transform.position.x + newX * speed, transform.position.y + newY * speed) );
			transform.localScale = new Vector3 (xScale * (float)leftRightFacing, transform.localScale.y, transform.localScale.z);
			//transform.Translate(newX * speed, newY*speed, 0);
		
			if ( (arrows[0]) || (arrows[1])  || (arrows[2])  || (arrows[3])  ) {
				aScript.walking = true;
				
				} else {
					aScript.walking = false;
				}
				
    	}
			if (tManScript.tutorialProgression == tManScript.BACKTOSHIPCONTROLS && gameModeManager.GetComponent<GameModeManagerScript>().currentMode == GameModeManagerScript.GameMode.STARMAP)
			{
				if ( 
					(Input.GetKeyDown("w") ) || (Input.GetKeyDown(KeyCode.UpArrow) )    ||
					(Input.GetKeyDown("a") ) || (Input.GetKeyDown(KeyCode.LeftArrow) )  ||
					(Input.GetKeyDown("s") ) || (Input.GetKeyDown(KeyCode.DownArrow) )  ||
					(Input.GetKeyDown("d") ) || (Input.GetKeyDown(KeyCode.RightArrow) ) 
				){
					buttonPresses++;
				}
				if (buttonPresses > 3)
				{
					buttonPresses = 0;
					tManScript.returnToCockpit();
				}
			}
	}

	
	public void OnTriggerEnter2D(Collider2D other){
		Debug.Log ("Entered a Trigger");
		if (other.gameObject.name == "PilotFloormat")
		{
			
			if (tManScript.tutorialProgression == tManScript.INCOCKPIT)
			{
				tManScript.becomePilot();
			}
			if (tManScript.tutorialProgression == tManScript.BACKTOSHIPCONTROLS)
			{
				tManScript.flyToPlanetInstructions();
			}
			
			if (!inPilotFloorMat)
			{
				//Debug.Log ("Floor Mat!!!!");
				inPilotFloorMat = true;
				gmManagerScript.switchModeTo(GameModeManagerScript.GameMode.NAVIGATION);
				setArrowsToZero();
			}
		}
		if (other.gameObject.name == "UpperFloormat")
		{
			if (!inPilotFloorMat)
			{
				//Debug.Log ("Floor Mat!!!!");
				inUpperFloorMat = true;

				gmManagerScript.switchModeTo(GameModeManagerScript.GameMode.GARBAGEHOLD);
				setArrowsToZero();

			}
		}
		
		if (other.gameObject.name == "LowerFloormat")
		{
			if (!inPilotFloorMat)
			{
			//	Debug.Log ("Floor Mat!!!!");
				inLowerFloorMat = true;
				setArrowsToZero();
				gmManagerScript.switchModeTo(GameModeManagerScript.GameMode.STARMAP);
			}
		}
        if (other.gameObject.name == "StorageFloor")
        {
					TutorialManagerScript tManScript = GameObject.Find("TutorialManager").GetComponent<TutorialManagerScript>();
					if (tManScript.tutorialProgression == tManScript.BEGINGAME)
					{
						tManScript.becomeNoviceGarbageMan();
					}
            if (other.gameObject.transform.parent.gameObject.name == "RadiationStation")
            {
                currentStorageChamberNumber = 1;
            }
            if (other.gameObject.transform.parent.gameObject.name == "RadiationStation (1)")
            {
                currentStorageChamberNumber = 0;
            }
            if (other.gameObject.transform.parent.gameObject.name == "RadiationStation (2)")
            {
                currentStorageChamberNumber = 2;
            }
            inStorageChamber = true;
            currentGarbageHold = other.transform.parent.gameObject;
        }
        if (other.gameObject.name == "cockpitTrigger")
        {
					TutorialManagerScript tManScript = GameObject.Find("TutorialManager").GetComponent<TutorialManagerScript>();
					if (tManScript.tutorialProgression == tManScript.PROMOTEDTOCAPTAIN)
					{
						tManScript.enterCockpit();
					}
				}
        if (other.gameObject.name == "LowerFloormat")
        {
					TutorialManagerScript tManScript = GameObject.Find("TutorialManager").GetComponent<TutorialManagerScript>();
					if (tManScript.tutorialProgression == tManScript.TONAVCOMP)
					{
						tManScript.inNavComp();
					}
				}
    }

    public void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.name == "PilotFloormat")
		{
			inPilotFloorMat = false;
		}
		if (other.gameObject.name == "UpperFloormat")
		{
			inUpperFloorMat = false;
		}
		if (other.gameObject.name == "LowerFloormat")
		{
			inLowerFloorMat = false;
		}
        if (other.gameObject.name == "StorageFloor")
        {
            inStorageChamber = false;
        }

    }

    public void setArrowsToZero()
	{
		for (int i=0; i<arrows.Count; i++)
		{
			arrows[i] = false;
		}
	}

    void tryToPickUpGarbage()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll((Vector2) transform.position+new Vector2(raycastOffset.x*leftRightFacing, raycastOffset.y), new Vector2(leftRightFacing, 0), raycastDistance);
        RaycastHit2D[] hits2 =  Physics2D.RaycastAll((Vector2)transform.position + new Vector2(raycastOffset.x * leftRightFacing, raycastOffset.y*.7f), new Vector2(leftRightFacing, 0), raycastDistance);
        RaycastHit2D[] hits3 = Physics2D.RaycastAll((Vector2)transform.position + new Vector2(raycastOffset.x * leftRightFacing, raycastOffset.y * 1.3f), new Vector2(leftRightFacing, 0), raycastDistance);

        Debug.DrawRay(transform.position+ new Vector3(raycastOffset.x * leftRightFacing, raycastOffset.y), new Vector2(leftRightFacing,0) * raycastDistance, Color.green);

        List<RaycastHit2D> allHits = new List<RaycastHit2D>();
        allHits.AddRange(hits);
        allHits.AddRange(hits3);
        allHits.AddRange(hits2);

        bool shouldPutDown = true;
        if (allHits.Count > 0)
        {
            foreach (RaycastHit2D hit in allHits)
            {
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject.tag == "ship garbage")
                {
                    if (pelletsBeingHeld.Contains(hitObject) == false)
                    {
                        int garbageHoldIndex = -1;

                        if (hitObject.transform.parent.gameObject.name == "RadiationStation (1)")
                        {
                            garbageHoldIndex = 0;
                        }
                        if (hitObject.transform.parent.gameObject.name == "RadiationStation")
                        {
                            garbageHoldIndex = 1;
                        }
                        if (hitObject.transform.parent.gameObject.name == "RadiationStation (2)")
                        {
                            garbageHoldIndex = 2;
                        }

                        hitObject.transform.parent = transform;
                        hitObject.transform.localPosition = new Vector3( 5.05f, -2.5f, 0);
                        pelletsBeingHeld.Add(hitObject);
                        shouldPutDown = false;
                 
                        Debug.Log("Garbage Hold index = " + garbageHoldIndex);
                        NavigationViewShipControlsScript shipScript = GameObject.Find("PlayerShip").GetComponent<NavigationViewShipControlsScript>();
                        shipScript.garbageHoldStatus[garbageHoldIndex]--;
                        GarbageHoldTopDownScript garbageHoldScript = transform.parent.gameObject.GetComponent<GarbageHoldTopDownScript>();
                        garbageHoldScript.GarbageHoldContents[garbageHoldIndex].Remove(hitObject);
                        debugDisplayGarbageStatus();
                        break;
                    }
                }
            }
        }
        
            if (pelletsBeingHeld.Count > 0 && shouldPutDown)
            {
                tryToPutDownGarbage();
            }
        
    }

    void tryToPutDownGarbage()
    {
        if (inStorageChamber)
        {
            GameObject garbagePellet = pelletsBeingHeld[0];

            garbagePellet.transform.parent = currentGarbageHold.transform;
            pelletsBeingHeld.Remove(garbagePellet);
            GameObject.Find("PlayerShip").GetComponent<NavigationViewShipControlsScript>().garbageHoldStatus[currentStorageChamberNumber]++;
            GarbageHoldTopDownScript garbageHoldScript = transform.parent.gameObject.GetComponent<GarbageHoldTopDownScript>();
            garbageHoldScript.GarbageHoldContents[currentStorageChamberNumber].Add(garbagePellet);

            debugDisplayGarbageStatus();
						
						
						if (tManScript.tutorialProgression == tManScript.NOVICEGARBAGEMAN && currentStorageChamberNumber != 1)
						{
							tManScript.becomeGarbageMan();
						}
						if (tManScript.tutorialProgression == tManScript.GARBAGEMAN 
							&& currentStorageChamberNumber != 1 
							&& GameObject.Find("Ship").GetComponent<GarbageHoldTopDownScript>().GarbageHoldContents[1].Count == 0) //&& StorageChamber 1 is empty
						{
							tManScript.becomeCaptain();
						}
        }


    }

    void debugDisplayGarbageStatus()
    {
        Debug.Log("Carrying: " + pelletsBeingHeld.Count);
        for (int i = 0; i < 3; i++) {
            Debug.Log("Hold[" + i + "]: " + GameObject.Find("PlayerShip").GetComponent<NavigationViewShipControlsScript>().garbageHoldStatus[i]);
    }

    }
}
