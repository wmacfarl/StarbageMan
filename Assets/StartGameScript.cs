using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameScript : MonoBehaviour {
	
	public GameObject menuBackground;
	public GameObject starbageMan;
	public GameObject backStars;
	public GameObject frontStars;
	
	public GameObject buttonsGroup;
	public GameObject creditsGroup;
	
	float backgroundStart = 0;
	float backgroundEnd = 11.16f;
	float backgroundMove = 0;
	float backgroundAccelerate;
	
	float textStart = 0;
	float textEnd = 30;
	float textMove = 0;
	float textAccelerate;
	
	float starsBackStart = 0;
	float starsBackEnd = 1.5f;
	float starsBackMove = 0;
	float starsBackAccelerate;
	
	float starsFrontStart = -11;
	float starsFrontEnd = -5;
	float starsFrontMove = 0;
	float starsFrontAccelerate;
	
	float secondsToTransition = 3;
	float frameRate = 60;
	float moveChunks;
	
	MoveState moveState;
	MoveDirection moveDirection;
	
	int framesMoved;
	int accelerateFrames = 45;
	
	PersistantDataScript persistantDataScript;
	
	enum MoveState {
		STILL = 0,
		ACCELERATING = 1,
		MOVING = 2,
		DECELERATING = 3
	};		
	
	enum MoveDirection {
		TOCREDITS = 0,
		TOMAIN = 1
		
	}

	// Use this for initialization
	void Start () {
		moveState = MoveState.STILL;
		moveDirection = MoveDirection.TOMAIN;
		framesMoved = 0;
		
		moveChunks = secondsToTransition * frameRate;
		backgroundAccelerate = calculateAccelerate(backgroundStart, backgroundEnd);
		textAccelerate = calculateAccelerate(textStart, textEnd);
		starsBackAccelerate = calculateAccelerate(starsBackStart, starsBackEnd);
		starsFrontAccelerate = calculateAccelerate(starsFrontStart, starsFrontEnd);
		persistantDataScript = GameObject.Find("PersistantData").GetComponent<PersistantDataScript>();
	}
	
	float calculateAccelerate(float start, float end)
	{
		float accelerate = (end - start) / (moveChunks * accelerateFrames)  ;
		return accelerate;
	}
	
	// Update is called once per frame
	void Update () {
		
		switch (moveState)
			{
				case MoveState.STILL:
					//framesMoved = 0;
					break;
				case MoveState.ACCELERATING:
					framesMoved++;
					
					backgroundMove += backgroundAccelerate;
					textMove += textAccelerate;
					starsBackMove += starsBackAccelerate;
					starsFrontMove += starsFrontAccelerate;
					
					moveBackground();
					if (framesMoved >= accelerateFrames)
					{
						moveState = MoveState.MOVING;
					}
					
					break;
				case MoveState.MOVING:
					framesMoved++;
					moveBackground();
					
					if ( (framesMoved >= (secondsToTransition * frameRate) - accelerateFrames) )
					{
						moveState = MoveState.DECELERATING;
					}
					break;
				case MoveState.DECELERATING:
					framesMoved++;
					
					backgroundMove -= backgroundAccelerate;
					textMove -= textAccelerate;
					starsBackMove -= starsBackAccelerate;
					starsFrontMove -= starsFrontAccelerate;
					
					moveBackground();
					
					if (framesMoved >= secondsToTransition * frameRate)
					{
						framesMoved = 0;
						moveState = MoveState.STILL;
						
						if (moveDirection == MoveDirection.TOCREDITS)
						{
							creditsGroup.SetActive(true);
						} else {
							if (moveDirection == MoveDirection.TOMAIN)
								{
									buttonsGroup.SetActive(true);
								}	
						}
						
					}
					
					break;
			}
		
		if (moveState == MoveState.STILL){
			
		}
		
	}
	


    public void StartGame()
    {
				persistantDataScript.startAfterTutorial();
				
        SceneManager.LoadScene("startGameScene");
    }

    public void StartTutorial()
    {
				persistantDataScript.startFromBeginning();
        SceneManager.LoadScene("tutorialScene");

    }
		
		public void ToCredits()
		{
			if (moveState == MoveState.STILL){
				moveState = MoveState.ACCELERATING;
				moveDirection = MoveDirection.TOCREDITS;
				buttonsGroup.SetActive(false);
				
				
				
			}
		}
		
		public void toMainMenu()
		{
			if (moveState == MoveState.STILL){
				moveState = MoveState.ACCELERATING;
				moveDirection = MoveDirection.TOMAIN;
				creditsGroup.SetActive(false);
				
				
			}
		}
		
		void moveBackground(){
			if (moveDirection == MoveDirection.TOCREDITS)
			{
				menuBackground.transform.Translate(new Vector3(backgroundMove, 0, 0) );
				starbageMan.transform.Translate(new Vector3(textMove, 0, 0) );
				backStars.transform.Translate(new Vector3(starsBackMove, 0, 0) );
				frontStars.transform.Translate(new Vector3(starsFrontMove, 0, 0) );
				} else {
					if (moveDirection == MoveDirection.TOMAIN)
					{
						menuBackground.transform.Translate(new Vector3(-backgroundMove, 0, 0) );
						starbageMan.transform.Translate(new Vector3(-textMove, 0, 0) );
						backStars.transform.Translate(new Vector3(-starsBackMove, 0, 0) );
						frontStars.transform.Translate(new Vector3(-starsFrontMove, 0, 0) );
					}
				}
		}
}
