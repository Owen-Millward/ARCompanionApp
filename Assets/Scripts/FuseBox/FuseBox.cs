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
		

	/// <summary>
	/// Start the game with the fuse box working, prevent a roll on 0% and set the rotation of the handle to the working position.
	/// </summary>
	void Start () {
		fuseState = FuseState.working;
		lastRoll = 0;
		targetRotation = 35;
	}
		
	/// <summary>
	/// When the charge percentage reacher a multiple of "Multiple" a roll is made to test if the fuse box breaks or not.
	/// CurrentRoll and LastRoll are used to hold information on the last and current charge % a roll was carried out on to prevent multiple rolls at the same %.
	/// If the roll fails and the fuse breaks the handles rotation is updates.
	/// The handles rotation is also updates if the fuse is fixed.
	/// </summary>
	void Update () {

		if ((((int)PowerMan.getCharge ()) % multiple) == 0) {
			
			currentRoll = (int)PowerMan.getCharge();

			if (currentRoll != lastRoll) {
				Roll ();
			}
				
			lastRoll = currentRoll;
		}

		rotateHandle ();

	}

	/// <summary>
	/// Rolls a dice with a 1 in 5 chance of breaking the fuse.
	/// </summary>
	private void Roll(){

		if (Random.Range (0, 100) < 20) {
			fuseBreak ();
		} 


	}

	/// <summary>
	/// Changes the state to broken.
	///	Changes the target rotation for the handle to up.
	/// </summary>
	public void fuseBreak(){
		fuseState = FuseState.broken;
		targetRotation = -35;
	}

	/// <summary>
	/// Changes the state to working.
	/// Changes the target rotation for the handle to down.
	/// </summary>
	public void fuseFix(){
		fuseState = FuseState.working;
		targetRotation = 35;
	}

	/// <summary>
	/// Rotates the handle to the target rotation over rotation time dependant on the current state of the fusebox.
	/// </summary>
	private void rotateHandle(){

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

	/// <summary>
	/// Returns if the fuse is working or is broken.
	/// </summary>
	/// <returns>Returns the current state of the fuse.</returns>
	public FuseState getState(){
		return fuseState;
	}
}
