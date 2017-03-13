using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour {

	private float charge = 0.0f;
	public bool unlockingPowerUP;
	public bool usingPowerUp;
	public bool[] unlocks;
	public bool[] unlockSoundPlayed;
	public int PowerLimitOne;
	public int PowerLimitTwo;
	public int PowerLimitThree;
	public float timeModifier;
	private float time;
	public float[] cooldowns;
	public float unlockTime;
	private int unlockID;
	private int useID;
	public FuseBox fuseBox;
	public AudioClip[] audioClips;
	private AudioSource audioSource;

	/// <summary>
	/// Start the game with no power ups being used or unlocked and all powerups being set to locked.
	/// </summary>
	void Start () {
		unlockingPowerUP = false;
		usingPowerUp = false;
		for (int i = 0; i < 3; i++) {
			unlocks [i] = false;
		}
	}


	void Update () {

		//if the fusebox is working
		if (fuseBox.getState() == FuseBox.FuseState.working) {

			//Play sound when enough charge has been reached to unlock a powerup
			if ((int)charge == getPowerLimit (1) ) {
				audioSource.clip = audioClips [1];
				audioSource.Play ();
			}

			//flagged that powerup is unlocking
			if (unlockingPowerUP) {
				//increment timer waiting for powerup
				cooldowns [unlockID - 1] += Time.deltaTime;

				//when unlocked
				if (cooldowns [unlockID - 1] > unlockTime) {

					//power up has finished unlocking and player can charge again
					unlockingPowerUP = false;

					//unlock correct powerup
					unlocks [unlockID - 1] = true;


				}
			} else {

				//not currently unlocking but powerup is being used
				if (usingPowerUp) {
					//reset timer
					cooldowns [useID - 1] = 0;
					usingPowerUp = false;
				}

				//manage timers for cooldowns after use
				for (int i = 1; i < 4; i++) {
					if (isUnlocked (i) && getTime (i) < unlockTime) {
						cooldowns [i - 1] += Time.deltaTime;
					}
				}

			}


		}
			


	}

	public void charging(){

		//if the fuse box isnt broken
		if (fuseBox.getState () == FuseBox.FuseState.working) {
			//if a power up isnt being unlocked and charge is lower than 100% increase charge
			//time modifier adjusts how fast it charges (timeModifier charge% increase per second);
			if (!unlockingPowerUP) {
				if (charge < 100.0f) {
					charge += Time.deltaTime * timeModifier;
				}
				//deals error showing percentage above 100
				if (charge > 100.0f) {
					charge = 100.0f;
					audioSource.clip = audioClips [1];
					audioSource.Play ();
				}
			}
		}

	}

	public float getCharge(){
		return charge;
	}
		
	public void usePowerUp( int powerUpID ){
			
			//UNLOCK ABILITY
			//check for enough charge and that another ability isnt currently being unlocked
			if (charge >= getPowerLimit (powerUpID) && !unlockingPowerUP && isUnlocked (powerUpID) == false) {

				//set unlocking to true
				unlockingPowerUP = true;
				unlockID = powerUpID;

			}

			//USE ABILITY
			else if (isUnlocked (powerUpID) && usingPowerUp == false && getTime (powerUpID) > unlockTime) {
				usingPowerUp = true;
				useID = powerUpID;
			}
	}

	//return the power limit for selected power up
	public int getPowerLimit(int limitID){
		switch (limitID) {
		case 0:
			break;
		case 1:
			return PowerLimitOne;
			break;
		case 2: 
			return PowerLimitTwo;
			break;
		case 3:
			return PowerLimitThree;
			break;
		}
		return 0;
	}

	//return if power up is unlocked
	public bool isUnlocked(int powerUpID){
		return unlocks [powerUpID - 1];
	}

	//returns current cool down 
	public float getTime(int powerUpID){
		return cooldowns[powerUpID-1];
	}

	//return time limit on cooldown
	public float getUnlockTime(){
		return unlockTime;
	}


}
