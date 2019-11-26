using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantDataScript : MonoBehaviour {
	public string restartStatus;
	int tutorialProgression = 0;

	// Use this for initialization
	void Start () {
		restartStatus = "none";
	}
	
	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void startFromBeginning()
	{
		tutorialProgression = 0;
		restartStatus = "tutorial";
	}
	
	public void startAfterTutorial()
	{
		tutorialProgression = 50;
		restartStatus = "game";
	}
	
	public string getRestartStatus()
	{
		return restartStatus;
	}
	
	public void setTutorialProgression(int newTutorialProgression)
	{
		tutorialProgression = newTutorialProgression;
	}
	
	public int getTutorialProgression()
	{
		return tutorialProgression;
	}
}
