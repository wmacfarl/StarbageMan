using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageHoldTopDownScript : MonoBehaviour {

    public List<List<GameObject>> GarbageHoldContents;
    public GameObject shipGarbagePrefab;
    public List<GameObject> garbageHolds;
    public List<GameObject> hold0;
    public List<GameObject> hold1;
    public List<GameObject> hold2;


    // Use this for initialization
    void Start () {


        if (GarbageHoldContents == null)
        {
            generateGarbageHolds();
        }

		}
	
		// Update is called once per frame
		void Update () {
		
		}
    void generateGarbageHolds()
    {
        garbageHolds.Add(GameObject.Find("RadiationStation (1)"));
        garbageHolds.Add(GameObject.Find("RadiationStation"));
        garbageHolds.Add(GameObject.Find("RadiationStation (2)"));

        GarbageHoldContents = new List<List<GameObject>>();
        for (int i = 0; i < 3; i++)
        {
            GarbageHoldContents.Add(new List<GameObject>());
        }
        hold0 = GarbageHoldContents[0];
        hold1 = GarbageHoldContents[1];
        hold2 = GarbageHoldContents[2];
    }
    public void getGarbage(int holdNumber)
    {
        if (GarbageHoldContents == null)
        {
            generateGarbageHolds();
        }
        GameObject newGarbage = GameObject.Instantiate(shipGarbagePrefab);
     //   Debug.Log("holdNumber = " + holdNumber);
       // Debug.Log("newGarbage = " + newGarbage);
       // Debug.Log("GarbageHoldContents = " + GarbageHoldContents);
        //Debug.Log("GarbageHoldContents[holdNumber] = " + GarbageHoldContents[holdNumber]);

        GarbageHoldContents[holdNumber].Add(newGarbage);
        newGarbage.transform.parent = garbageHolds[holdNumber].transform;
        newGarbage.transform.localPosition = Random.insideUnitCircle * 2;
    }

    public void loseGarbage(int holdNumber)
    {
        Debug.Log("holdNumber = " + holdNumber);
				if (GarbageHoldContents[holdNumber].Count > 0 )
				{
        	GameObject garbage = GarbageHoldContents[holdNumber][GarbageHoldContents[holdNumber].Count - 1];
        	GarbageHoldContents[holdNumber].Remove(garbage);
        	GameObject.Destroy(garbage);
				}
    }
		
		public void emptyGarbage(){
			for (int i = 0; i < garbageHolds.Count; i++)
			{
				for (int j=GarbageHoldContents[i].Count; j>=0; j--)
				{
					loseGarbage(i);
				}
            GameObject.Find("PlayerShip").GetComponent<NavigationViewShipControlsScript>().garbageHoldStatus[i] = 0;
        }
			
		}
}
