using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBox : MonoBehaviour {

	public PowerupManager PowerMan;
	public int multiple;
	private int lastRoll, currentRoll;
	public enum FuseState { working, broken };
	public FuseState fuseState; 
	private int targetRotation; 
	public float rotationTime;
		

	// Use this for initialization
	void Start () {
		fuseState = FuseState.working;
		lastRoll = 0;
		targetRotation = 35;
	}

	// Update is called once per frame
	void Update () {


		//if the charge has reached a multiple of "Multiple" roll
		if ((((int)PowerMan.getCharge ()) % multiple) == 0) {
			//the current charge
			currentRoll = (int)PowerMan.getCharge();

			//ensure there are no double rolls on the same number
			if (currentRoll != lastRoll) {
				Roll ();
			}

			//remember last roll
			lastRoll = currentRoll;
		}

		rotateHandle ();

	}

	//check if the machine breaks or not 
	private void Roll(){

		if (Random.Range (0, 100) < 20) {
			fuseBreak ();
		} 


	}

	public void fuseBreak(){
		fuseState = FuseState.broken;
		targetRotation = -35;
	}

	public void fuseFix(){
		fuseState = FuseState.working;
		targetRotation = 35;
	}


	private void rotateHandle(){

		//if x rotation is greater than -35 and fuse is broken rotate up to broken position

		if (fuseState == FuseState.broken  ) {
			if (transform.rotation.eulerAngles.x < 40.0f  || transform.rotation.eulerAngles.x > 325.0f) {
				
				transform.Rotate (new Vector3 (rotationTime * targetRotation * Time.deltaTime * 2, 0, 0));
			}
		}
		if (fuseState == FuseState.working ) {
			if (transform.rotation.eulerAngles.x < 35.0f  || transform.rotation.eulerAngles.x > 320.0f) {
				
				transform.Rotate (new Vector3 (rotationTime * targetRotation * Time.deltaTime, 0, 0));
			}
		}
	

	}

	public FuseState getState(){
		return fuseState;
	}
}
