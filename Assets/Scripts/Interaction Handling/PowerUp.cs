using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

	[Tooltip("ID for this powerup.")]
	public int ID;					//ID for the powerup
	[Tooltip("Is the powerup Locked?")]
	public bool locked;				//Has the powerup been unlocked yet?
	[Tooltip("At what percentage the power up be unlocked.")]
	public int chargeRequired;		//Charge % required to unlock the powerup
	[Tooltip("The current value of the timer for the powerups cooldown")]
	public float coolDownTimer;		//timer for the powerups cooldown
	[Tooltip("How long it takes to unlock the power up. This is also the cool down length.")]
	public float unlockTime;		//limit for the powerups cooldown timer
	[Tooltip("Is the power up on cooldown?")]
	public bool onCooldown;         //is the powerup onCoolDown
	[Tooltip("has the sound has been played or not?")]
	public bool soundPlayed = false; //if the sound has been played or not

		/// <summary>
		/// Initializes a new instance of the <see cref="PowerupManager+PowerUp"/> struct.
		/// </summary>
		/// <param name="Id">Identifier.</param>
		/// <param name="ChargeRequired">Charge required to unlock.</param>
		/// <param name="UnlockTime">Length of Cooldown.</param>
		public PowerUp(int Id, int ChargeRequired, float UnlockTime){
			ID = Id;							
			locked = true;						
			chargeRequired = ChargeRequired;
			coolDownTimer = 0.0f;
			unlockTime = UnlockTime;
			onCooldown = true;
			soundPlayed = false;
		}

		/// <summary>
		/// Unlock this power-up.
		/// </summary>
		public void Unlock(){

			locked = false;
		}

		/// <summary>
		/// Returns if the power-up is locked.
		/// </summary>
		/// <returns><c>true</c>, if locked, <c>false</c> otherwise.</returns>
		public bool isLocked(){

			return locked;
		}

		/// <summary>
		/// Gets the Charge % required to use or unlock a powerup.
		/// </summary>
		/// <returns>Charge required to unlock power up.</returns>
		public int getChargeRequired(){
			
			return chargeRequired;
		}
		
		/// <summary>
		/// play the be unlocked sound.
		/// </summary>
		public void unlockedSoundPlayed(){

			soundPlayed = true;
		}
		
		/// <summary>
		/// has the unlock sound been played or not?
		/// </summary>
		/// <returns><c>true</c>, if can be unlocked sound was played, <c>false</c> otherwise.</returns>
		public bool getSoundPlayed(){
			
			return soundPlayed;
		}

		/// <summary>
		/// Check timers to see if the powerup is on cooldown or not.
		/// </summary>
		public void coolDown(){
			if (coolDownTimer < unlockTime) {
				coolDownTimer += Time.deltaTime;
				onCooldown = true;
			} else {
				onCooldown = false;
			}
		}

		/// <summary>
		/// Return if the powerup is on cooldown or not.
		/// </summary>
		/// <returns>If the powerup is on cooldown or not</returns>
		public bool OnCoolDown(){
			return onCooldown;
		}

		/// <summary>
		/// Resets the cool down timer.
		/// </summary>
		public void resetCoolDown(){
			coolDownTimer = 0.0f;
			onCooldown = true;
		}

		/// <summary>
		/// Returns the cooldown progress as a percentage
		/// </summary>
		/// <returns>The cooldown progress as a percentage.</returns>
		public float coolDownPercent(){

			float percent;

			if (coolDownTimer == 0.0f) 
			{

				percent = 0;
			} 
			else 
			{

				percent = coolDownTimer / unlockTime;
			}

			return percent;
		}
			

}

