using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageHoldPanelScript : MonoBehaviour {

    public float[] chamberHeats;
    GameObject[] chamberDials;
    float maxHeat = 360;
    public GameObject playerShip;
    public float radiationHeatingFactor = .01f;
    public float emptyChamberCoolingFactor = .08f;
    public float radiantCoolingFactor = .005f;
    float shakeFactor = .001f;
    float totalHeat = 0;
		bool eyeOnStorageMessage = false;
		


	// Use this for initialization
	void Start () {
        chamberHeats = new float[3];
        chamberDials = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            chamberHeats[i] = 0;
            chamberDials[i] = transform.GetChild(i).gameObject;
        }
    }
	
	// Update is called once per frame
	void Update () {
		float highestHeat = 0;
    float totalHeat = 0;
		for(int i = 0; i < 3; i++)
        {
            GameObject dial = chamberDials[i];
            dial.transform.GetChild(0).transform.localRotation = Quaternion.AngleAxis(90+chamberHeats[i], Vector3.back);
            int garbageCount = playerShip.GetComponent<NavigationViewShipControlsScript>().garbageHoldStatus[i];
            if (garbageCount > 0)
            {
                chamberHeats[i] += radiationHeatingFactor * garbageCount;
            }else
            {
                chamberHeats[i] -= emptyChamberCoolingFactor;
            }
            chamberHeats[i] -= radiantCoolingFactor;
            if (chamberHeats[i] > maxHeat)
            {
                chamberHeats[i] = maxHeat;
								GameModeManagerScript gManager = GameObject.Find("Game Mode Manager").GetComponent<GameModeManagerScript>();
                //gManager.areDead = true;
								gManager.dieByShip();
								//GameObject.Find("TutorialManager").GetComponent<TutorialManagerScript>().triggerBossMessage("deathFromShip");
            }
						if (chamberHeats[i] > (maxHeat * 3 /4) && !eyeOnStorageMessage)
						{
							eyeOnStorageMessage = true;
							GameObject.Find("TutorialManager").GetComponent<TutorialManagerScript>().triggerBossMessage("eyeOnStorage");
						}
            if (chamberHeats[i] < 0)
            {
                chamberHeats[i] = 0;
            }
            totalHeat += chamberHeats[i];
						if (chamberHeats[i] > highestHeat)
						{
							highestHeat = chamberHeats[i];
						}
        }


        screenShake(highestHeat);
    }

    void screenShake(float totalHeat)
    {
        GameModeManagerScript gm = FindObjectOfType<GameModeManagerScript>();
        if (gm.currentMode == GameModeManagerScript.GameMode.TOPDOWN)
        {
					TopDownCameraScript camScript = GameObject.Find("Top Down View Camera").GetComponent<TopDownCameraScript>();
					float minHeatForShake = 5 * maxHeat / 8;
					float maxCameraShake = 0.7f;
					
					if (totalHeat > minHeatForShake)
					{

						float heatInRange = totalHeat - minHeatForShake;
						float camShake = (heatInRange / (maxHeat - minHeatForShake)  ) * maxCameraShake;
						camScript.setCameraShakeAmount(camShake);
					} else {
						camScript.setCameraShakeAmount(0);	
					}
					
					
					
          Camera cam = GameObject.Find("Top Down View Camera").GetComponent<Camera>();
//        cam.transform.localPosition = Random.insideUnitCircle * shakeFactor * totalHeat;
        }

    }
}
