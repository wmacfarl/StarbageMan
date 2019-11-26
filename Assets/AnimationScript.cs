using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour {
	SpriteRenderer spriteRenderer;
	public Sprite walk1;
	public Sprite walk2;
	
	public bool walking;
	int frame;
	int animationFrame;
	
	int framesPerState;
	
	bool startWalking;
	
	public GameObject closeArm;
	public GameObject farArm;
	
	float farRotationDirection = -1;
	float closeRotationDirection = 1;
	
	float maxArmRotation = 25;
	
	float rotationAmount;
	
	public bool holdingWaste = false;
	
	PlayerPersonControls playerControlScript;

	// Use this for initialization
	void Start () {

		spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = walk1;
		this.frame = 1;
		this.walking = false;
		
		this.startWalking = false;
		
		this.framesPerState = 10;
		
		rotationAmount = maxArmRotation/framesPerState;
		
		playerControlScript = this.gameObject.GetComponent<PlayerPersonControls>();
		
		//closeArm.transform.eulerAngles;
		//farArm.transform.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (walking)
		{
			if (!startWalking)
			{
				startWalking = true;
				this.animationFrame = 2;
				spriteRenderer.sprite = walk2;
			}
			frame++;
			

			
			if (frame > this.framesPerState){
				animate();
				if (animationFrame == 1)
				{
					this.closeRotationDirection *= -1;
					this.farRotationDirection *= -1;
				}
				frame = 0;
			}
			
			if (!holdingWaste)
			{
				closeArm.transform.Rotate(new Vector3(0,0, this.rotationAmount * closeRotationDirection) );
				farArm.transform.Rotate(new Vector3(0,0, this.rotationAmount * farRotationDirection) );
			}
		
		} else {
			frame = 0;
			spriteRenderer.sprite = walk1;
			startWalking = false;
			
			if (!holdingWaste)
			{
				closeArm.transform.rotation =  Quaternion.identity; //new Vector3(0,0,0) ;
				farArm.transform.rotation = Quaternion.identity; //new Vector3(0,0,0) ;
			}
			
		}
		

		if (holdingWaste)
		{
			//if (playerControlScript.leftRightFacing > 0)
			//{
				if (closeArm.transform.eulerAngles.z < this.maxArmRotation)
				{
					closeArm.transform.Rotate(new Vector3(0,0,this.rotationAmount * 2) );
				}
				if (farArm.transform.eulerAngles.z < this.maxArmRotation)
				{
					farArm.transform.Rotate(new Vector3(0,0,this.rotationAmount * 2) );
				}
				/*} else {
				if (-1 * closeArm.transform.eulerAngles.z > -1 * this.maxArmRotation)
				{
					closeArm.transform.Rotate(new Vector3(0,0,this.rotationAmount * 2) );
				}
				if (-1 * farArm.transform.eulerAngles.z > -1 * this.maxArmRotation)
				{
					farArm.transform.Rotate(new Vector3(0,0,this.rotationAmount * 2) );
				}
			}*/

		}//end if holding waste
	}
	
	void animate(){
		this.animationFrame++;
		if (this.animationFrame > 2){
			this.animationFrame = 1;
		}
		
		if (this.animationFrame == 1)
			{
				spriteRenderer.sprite = walk1;
			} else {
				if (this.animationFrame ==2)
					{
						spriteRenderer.sprite = walk2;
					}	
			} 
		
	}
}

/*
public class animation{
	public List<Sprite> images;
	
	animation(List<Sprite> sprites){
		
	
	}
	
	
}
*/