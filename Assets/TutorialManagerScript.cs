using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManagerScript : MonoBehaviour {

    int ticks;
    public int tutorialProgression;
    //int ticksToCaptainPromotion;
		public int secondsToCaptainPromotion;
		public int secondsToPilotInstructions;
		public int secondsToLearnNav;
		public int secondsToMapExplain;
		
		//int ticksToPilotInstructions;

    public int ticksToSystemManager;
    public int ticksToSectorManager;
    public int ticksToDistrictManager;
		
		

    public int  BEGINGAME = 0;
    //do garbagey tasks for a while 
		public int NOVICEGARBAGEMAN = 1;
		public int GARBAGEMAN = 2;
    public int PROMOTEDTOCAPTAIN = 3;
		public int INCOCKPIT = 4;
		public int PILOT = 5;
		public int INFORMEDPILOT = 6;
		public int LEAVETOLEARNNAV = 7;
		public int TONAVCOMP = 8;
		public int BACKTOSHIPCONTROLS = 9;
		public int TOPROCTARIS = 10;
		public int INPROCTARISORBIT = 11;
		public int ORBITINGBLACKHOLE = 12;
		public int PROMOTEDTOREGIONALMANAGER = 13;
		
		
		
    int SYSTEMMANAGER = 20;
    int SECTORMANAGER = 30;
    int DISTRICTMANAGER = 40;
		public int GAME = 50;
    public GameObject planetSystem;
    public GameObject starmapViewCamera;
    public GameObject BossCanvas;
    BossAnimationScript bossAnimationScript;
		
		//public TextAsset bossAudioCSV;
		
		PersistantDataScript persistantDataScript;
		

    // Use this for initialization
    void Start () {
		persistantDataScript = GameObject.Find("PersistantData").GetComponent<PersistantDataScript>();
        ticks = 0;
        GameObject.Find("Game Mode Manager").GetComponent<GameModeManagerScript>().restartStatus = "tutorial";
        planetSystem.GetComponent<PlanetSystemScript>().numberOfPlanets = 0;
        planetSystem.GetComponent<PlanetSystemScript>().systemRadius = 50;
        planetSystem.GetComponent<PlanetSystemScript>().GeneratePlanets();
				GameObject.Destroy(planetSystem.GetComponent<PlanetSystemScript>().blackHole);
        starmapViewCamera.GetComponent<Camera>().orthographicSize = 50;
        bossAnimationScript = BossCanvas.GetComponent<BossAnimationScript>();
                //ticksToCaptainPromotion = secondsToCaptainPromotion * 60;
                //ticksToPilotInstructions = secondsToPilotInstructions * 60;
				tutorialProgression = persistantDataScript.getTutorialProgression();

				if (tutorialProgression > 3)
				{
          GameObject.Find("Ship").GetComponent<GarbageHoldTopDownScript>().emptyGarbage();
          Destroy(GameObject.Find("RadiationDoor"));	
				}
				
				if (tutorialProgression >= BACKTOSHIPCONTROLS)
				{
					planetSystem.GetComponent<PlanetSystemScript>().numberOfPlanets = 1;
					planetSystem.GetComponent<PlanetSystemScript>().GeneratePlanets();
					GameObject.Find("PlayerShip").GetComponent<NavigationViewShipControlsScript>().shipStill = false;
				}
				
        if (tutorialProgression > 12)
        {
            
            planetSystem.GetComponent<PlanetSystemScript>().numberOfPlanets = 5;
            planetSystem.GetComponent<PlanetSystemScript>().systemRadius = 200;
            starmapViewCamera.GetComponent<Camera>().orthographicSize = 200;
            planetSystem.GetComponent<PlanetSystemScript>().GeneratePlanets();
            GameObject.Find("PlayerShip").GetComponent<NavigationViewShipControlsScript>().shipStill = false;
        }
    }



    // Update is called once per frame
    void Update () {
        ticks++;

        if (ticks == 3 && tutorialProgression == BEGINGAME)
        {
            triggerBossMessage("intro");
        }
     
		 	 	//Promote to Captain
				/*
        if (tutorialProgression == GARBAGEMAN && ticks > ticksFromSeconds(secondsToCaptainPromotion) )
        {
            becomeCaptain();
        }
				*/
				
				if (tutorialProgression == INCOCKPIT)
				{
					//setTutorialProgression(PILOT);
				}
				
        if (tutorialProgression == PILOT && ticks > ticksFromSeconds(secondsToPilotInstructions) )
        {
					setTutorialProgression(INFORMEDPILOT);
					GameObject.Find("PlayerShip").GetComponent<NavigationViewShipControlsScript>().shipStill = false;
					triggerBossMessage("controlShip");
					ticks = 0;
				}
				
				if (tutorialProgression == INFORMEDPILOT && ticks > ticksFromSeconds(secondsToLearnNav) )
				{
					setTutorialProgression(LEAVETOLEARNNAV);
					triggerBossMessage("navigationLesson");
					
				}
				/*
        if (tutorialProgression == PROMOTEDTOCAPTAIN && ticks > ticksToSystemManager)
        {
            planetSystem.GetComponent<PlanetSystemScript>().numberOfPlanets = 2;
            planetSystem.GetComponent<PlanetSystemScript>().systemRadius = 75;
            planetSystem.GetComponent<PlanetSystemScript>().MakeNewPlanet();
            starmapViewCamera.GetComponent<Camera>().orthographicSize = 75;
            setTutorialProgression(SYSTEMMANAGER);
            ticks = 0;
        }
				*/
        if (tutorialProgression == SYSTEMMANAGER && ticks > ticksToSectorManager)
        {
            setTutorialProgression(SECTORMANAGER);
            ticks = 0;
            planetSystem.GetComponent<PlanetSystemScript>().numberOfPlanets = 3;
            planetSystem.GetComponent<PlanetSystemScript>().systemRadius = 125;
            starmapViewCamera.GetComponent<Camera>().orthographicSize = 125;
            planetSystem.GetComponent<PlanetSystemScript>().MakeNewPlanet();
        }
        if (tutorialProgression == SECTORMANAGER && ticks > ticksToDistrictManager)
        {
            planetSystem.GetComponent<PlanetSystemScript>().numberOfPlanets = 5;
            planetSystem.GetComponent<PlanetSystemScript>().systemRadius = 200;
            starmapViewCamera.GetComponent<Camera>().orthographicSize = 200;
            planetSystem.GetComponent<PlanetSystemScript>().MakeNewPlanet();
            planetSystem.GetComponent<PlanetSystemScript>().MakeNewPlanet();
            setTutorialProgression(DISTRICTMANAGER);
            ticks = 0;
        }
    }
		
    public void triggerBossMessage(string name)
    {
        BossCanvas.SetActive(true);
        bossAnimationScript.triggerBossMessage(name);
    }

		public void becomePilot()
		{
			setTutorialProgression(PILOT);
			ticks = 0;
			//GameObject.Find("PlayerShip").GetComponent<NavigationViewShipControlsScript>().shipStill = false;
		}
		
		public void becomeNoviceGarbageMan()
		{
      if (tutorialProgression == BEGINGAME)
      {
          setTutorialProgression(NOVICEGARBAGEMAN);
          //triggerBossMessage("doGarbage");
          ticks = 0;
      }
		}
		
		public void becomeGarbageMan()
		{
        if (tutorialProgression == NOVICEGARBAGEMAN)
        {
            setTutorialProgression(GARBAGEMAN);
            triggerBossMessage("howI'mTalking");
            ticks = 0;
        }
			
		}
		
		public void becomeCaptain()
		{
      if (tutorialProgression == GARBAGEMAN)
      {
	      setTutorialProgression(PROMOTEDTOCAPTAIN);
				GameObject.Find("Ship").GetComponent<GarbageHoldTopDownScript>().emptyGarbage();
				Destroy(GameObject.Find("RadiationDoor"));
				triggerBossMessage("promotionToCaptain");
	      ticks = 0;
	      Debug.Log("tutorialProgression = " + tutorialProgression);
			}
		}
		
		public void enterCockpit()
		{
			setTutorialProgression(INCOCKPIT);
			Destroy(GameObject.Find("cockpitTrigger") );
			triggerBossMessage("startAsCaptain");
			ticks = 0;
		}
		
		public void toNavComp()
		{
			triggerBossMessage("toNavComputer");
			setTutorialProgression(TONAVCOMP);
			GameObject.Find("PlayerShip").GetComponent<NavigationViewShipControlsScript>().shipStill = true;
		}

		public void inNavComp()
		{
      planetSystem.GetComponent<PlanetSystemScript>().numberOfPlanets = 1;
      planetSystem.GetComponent<PlanetSystemScript>().systemRadius = 50;
      planetSystem.GetComponent<PlanetSystemScript>().GeneratePlanets();
			triggerBossMessage("insideNavComputer");
			setTutorialProgression(BACKTOSHIPCONTROLS);
			GameObject.Find("PlayerShip").GetComponent<NavigationViewShipControlsScript>().shipStill = false;
		}

		public void returnToCockpit()
		{
			triggerBossMessage("notFromNavComputer");
		}
		
		public void flyToPlanetInstructions()
		{
			triggerBossMessage("toProctaris");
			setTutorialProgression(TOPROCTARIS);
			
		}
		
		public void orbitProctaris()
		{
			setTutorialProgression(INPROCTARISORBIT);
			triggerBossMessage("returnToBlackHole");
		}
		
		public void orbitBlackHole()
		{
			setTutorialProgression(ORBITINGBLACKHOLE);
			triggerBossMessage("shootPellet");
		}
		
		public void promoteToRegionalManager()
		{
			setTutorialProgression(GAME);
			Vector3 blackHolePos = planetSystem.GetComponent<PlanetSystemScript>().blackHole.transform.position;
      planetSystem.GetComponent<PlanetSystemScript>().numberOfPlanets = 5;
      planetSystem.GetComponent<PlanetSystemScript>().systemRadius = 200;
      planetSystem.GetComponent<PlanetSystemScript>().GeneratePlanets();
        starmapViewCamera.GetComponent<Camera>().orthographicSize = 210;
        planetSystem.GetComponent<PlanetSystemScript>().blackHole.transform.position = blackHolePos;
			
			triggerBossMessage("promotionToDistrictManager");
		}
		
		int ticksFromSeconds(int seconds)
		{
			return seconds * 60;
		}
		
		public void setTutorialProgression(int newProgression)
		{
			tutorialProgression = newProgression;
			persistantDataScript.setTutorialProgression(newProgression);
		}
}
