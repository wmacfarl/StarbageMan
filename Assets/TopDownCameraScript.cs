using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCameraScript : MonoBehaviour {
	public GameObject playerPerson;
	
	public float cameraShakeAmount = 0;
	
	Vector3 cameraShakeVector;
	
	int heatShakeTicks = 0;
	// Use this for initialization
	void Start () {
		figureOutCameraShake();
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 positionForCamera = new Vector3(playerPerson.transform.position.x, playerPerson.transform.position.y, transform.position.z);
		figureOutCameraShake();
		transform.position = positionForCamera + cameraShakeVector;
	
		
		
	}
	
	void figureOutCameraShake()
	{
		if (cameraShakeAmount != 0)
		{
			heatShakeTicks++;
			int heatTicksToChangePos = 3;
			
			if (heatShakeTicks > heatTicksToChangePos)
			{
				cameraShakeVector = new Vector3 (Random.Range(-cameraShakeAmount, cameraShakeAmount), Random.Range(-cameraShakeAmount, cameraShakeAmount), 0 );
				heatShakeTicks = 0;
			}
		}	
	}
	
	public void setCameraShakeAmount(float newAmount)
	{
		cameraShakeAmount = newAmount;
	}
}
