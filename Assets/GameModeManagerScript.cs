using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameModeManagerScript : MonoBehaviour {

    public enum GameMode
    {
        NAVIGATION,
        STARMAP,
        TOPDOWN,
        GARBAGEHOLD
    }

    public GameMode currentMode;
    public GameObject NavigationCamera;
    public GameObject TopDownCamera;
    public GameObject StarmapCamera;
    public GameObject playerShip;
    public GameObject garbageHoldCamera;
    public Sprite playerShipNavigationModeSprite;
    public Sprite playerShipStarmapModeSprite;
    public GameObject bossCanvas;
    public bool areDead;
    public string restartStatus;
    public GameObject restartButton;
    bool justDied = true;
		
		public DeathMethodType deathMethod;
		
		PersistantDataScript persistantDataScript;
		
		public enum DeathMethodType
		{
			NONE,
			SHIP,
			PLANET
		}

    // Use this for initialization
    void Start () {
        areDead = false;
        switchModeTo(GameMode.TOPDOWN);
       // restartButton = GameObject.Find("Restart Button");
				deathMethod = DeathMethodType.NONE;
				
				persistantDataScript = GameObject.Find("PersistantData").GetComponent<PersistantDataScript>();
	}
	
	// Update is called once per frame
	void Update () {
    if (areDead)
    {
        if (justDied == true)
        {
					if (deathMethod == DeathMethodType.SHIP)
					{
						GameObject.Find("TutorialManager").GetComponent<TutorialManagerScript>().triggerBossMessage("deathFromShip");
					}
					if (deathMethod == DeathMethodType.PLANET)
					{
						GameObject.Find("TutorialManager").GetComponent<TutorialManagerScript>().triggerBossMessage("deathFromPlanet");
					}
            //GameObject.Find("TutorialManager").GetComponent<TutorialManagerScript>().triggerBossMessage("death1");
            justDied = false;
        }
				
				//I'm not sure what this line does but when it's in there  the death message only plays the first clip
        //bossCanvas.GetComponent<BossAnimationScript>().interruptionStartTime = Time.time;
				
        bossCanvas.SetActive(true);
        //restartButton.SetActive(true);
    }

		//Why is this here?
    if (bossCanvas.activeSelf == false)
    {
        //Debug.Log("not active.");
        if (Random.value > .99999)
        {
            bossCanvas.SetActive(true);
            bossCanvas.GetComponent<BossAnimationScript>().popUpMessage(null);
        }
    }

    //This is for debugging/dev purposes
    if (Input.GetKeyDown(KeyCode.F1))
    {
        switchModeTo(GameMode.NAVIGATION);
    }
    if (Input.GetKeyDown(KeyCode.F2))
    {
        switchModeTo(GameMode.STARMAP);
    }
    if (Input.GetKeyDown(KeyCode.F3))
    {
        switchModeTo(GameMode.TOPDOWN);
    }
    if (Input.GetKeyDown(KeyCode.F4))
    {
        switchModeTo(GameMode.GARBAGEHOLD);
    }

    //this is the in-game interface
    if (Input.GetKeyDown(KeyCode.E))
    {
        
				TutorialManagerScript tManScript = GameObject.Find("TutorialManager").GetComponent<TutorialManagerScript>();
				if (tManScript.tutorialProgression == tManScript.LEAVETOLEARNNAV)
				{
					tManScript.toNavComp();
				}
				if (tManScript.tutorialProgression == tManScript.BACKTOSHIPCONTROLS)
				{
					tManScript.flyToPlanetInstructions();
				}
				switchModeTo(GameMode.TOPDOWN);
    }
	}

    public void switchModeTo(GameMode mode)
    {
        if (currentMode != mode)
        {
            currentMode = mode;
            NavigationCamera.gameObject.SetActive(false);
            TopDownCamera.gameObject.SetActive(false);
            StarmapCamera.gameObject.SetActive(false);
            garbageHoldCamera.SetActive(false);
            switch (currentMode)
            {
                case (GameMode.NAVIGATION):
                    NavigationCamera.SetActive(true);
                    playerShip.GetComponent<SpriteRenderer>().sprite = playerShipNavigationModeSprite;
                    break;
                case (GameMode.STARMAP):
                    StarmapCamera.SetActive(true);
                    playerShip.GetComponent<SpriteRenderer>().sprite = playerShipStarmapModeSprite;
                    break;
                case (GameMode.TOPDOWN):
                    TopDownCamera.SetActive(true);
                    break;
                case (GameMode.GARBAGEHOLD):
                    garbageHoldCamera.SetActive(true);
                    break;
            }
        }
    }

		public void dieByPlanet()
		{
			deathMethod = DeathMethodType.PLANET;
			this.areDead = true;
		}
		
		public void dieByShip()
		{
			deathMethod = DeathMethodType.SHIP;
			this.areDead = true;
		}

    public void restart()
    {
			restartStatus = persistantDataScript.getRestartStatus();
			
			Debug.Log("restart button clicked");
        if (restartStatus == "tutorial")
        {
            SceneManager.LoadScene("tutorialScene");
        }
        if (restartStatus == "game")
        {
            SceneManager.LoadScene("startGameScene");
        }

    }

}

