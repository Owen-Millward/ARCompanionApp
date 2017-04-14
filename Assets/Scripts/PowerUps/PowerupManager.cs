using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour {

	//GamePlay
	private float charge = 0.0f;								//Current Charge %
	[Tooltip("Charge % increase per second")]
	public float timeModifier;

	public enum TerminalState{Idle, Unlocking, UsingPowerUp};	//States for PowerUpTerminal
	private TerminalState terminalState;						//State of PowerUpTerminal

	//PowerUps
	private int unlockID;		//PowerUp being unlocked
	private int useID;			//PowerUp being Used

	public PowerUp[] powerUps;	//Array of power ups

	//FuseBox
	[Tooltip("FuseBox(handle)")]
	public FuseBox fuseBox;				//Holds game state working or broken

	//Audio
	[Tooltip("Audiofiles associated with this object")]
	public AudioClip[] audioClips;		//List of audio files for this object
	[Tooltip("AudioSource (this)")]
	public AudioSource audioSource;	//Audio source for playing clips

	//Tool tips
	[SerializeField]
	[Tooltip("ToolTips parent object")]
	private TipManager tipManager;


	/// <summary>
	/// Start the game with terminal state in idle and all powerups being initialised
	/// Show Gaze and Communication terminal ToolTips
	/// </summary>
	void Start () {

		terminalState = TerminalState.Idle;

		if (audioSource == null) {
			audioSource = GetComponent<AudioSource> ();
		}

		tipManager.ShowTip (0);
		tipManager.ShowTip (6);
	}

	/// <summary>
	/// Powerup gameplay management
	/// 
	/// Handle unlocking powerups
	/// Using powerups
	/// Cooldowns for powerups
	/// 
	/// dismiss tooltips
	/// </summary>
	void Update () {

		//if the fusebox is working
		if (fuseBox.getState() == FuseBox.FuseState.working) {

			//Play sound when enough charge has been reached to unlock a powerup
			for(int i = 0; i<= 2; i ++){
				if ((int)charge >= powerUps[i].getChargeRequired() && powerUps[i].getSoundPlayed() == false) {
					audioSource.clip = audioClips [0];
					audioSource.Play ();
					powerUps [i].unlockedSoundPlayed ();
				}
			}

			//flagged that powerup is unlocking
			if (terminalState == TerminalState.Unlocking) {
				
				//increment timer waiting for powerup
				powerUps[unlockID].coolDown();

				//when unlocked
				if (powerUps[unlockID].OnCoolDown() == false) {

					//dismiss can unlock tool tip
					tipManager.DismissTip(2);
					tipManager.DismissTip (9);

					//show unlocked and send tip
					tipManager.ShowTip(4);
					tipManager.ShowTip(5);


					//power up has finished unlocking and player can charge again
					terminalState = TerminalState.Idle;

					//unlock correct powerup
					powerUps[unlockID].Unlock();
				}
			}

			else {

				//not currently unlocking but powerup is being used
				if (terminalState == TerminalState.UsingPowerUp) {
					//reset timer

					powerUps [useID].resetCoolDown ();
					terminalState = TerminalState.Idle;
				}

				//manage timers for cooldowns after use
				for (int i = 0; i < 3; i++) {
					
					if (powerUps [i].isLocked () == false && powerUps [i].OnCoolDown ()) {
						
						powerUps [i].coolDown ();
					} 
					else 
					{
						//dismiss cooldown tip
						tipManager.DismissTip(8);
					}
				}
			}
		}

		//dismiss gaze tip show win tip
		if ((int)charge == 10) {
			tipManager.DismissTip (0); 	//gaze
			tipManager.ShowTip (1);		//win
		}

		//show unlock tip
		if (charge >= powerUps [0].getChargeRequired ()) {
			tipManager.ShowTip (2);
			tipManager.ShowTip (3);
		}
	}

	/// <summary>
	/// Increase charge % over time.
	/// </summary>
	public void charging(){

		//if the fuse box isnt broken
		if (fuseBox.getState () == FuseBox.FuseState.working) {
			
			//if a power up isnt being unlocked and charge is lower than 100% increase charge
			//time modifier adjusts how fast it charges (timeModifier charge% increase per second);
			if (terminalState == TerminalState.Idle) {
				
				if (charge < 100.0f) {
					
					charge += Time.deltaTime * timeModifier;
				}

				//deals error showing percentage above 100 dismiss win tip
				if (charge > 100.0f) {

					tipManager.DismissAll ();
					charge = 100.0f;
					audioSource.clip = audioClips [1];
					audioSource.Play ();
				}
			}
		}
	}

	/// <summary>
	/// Gets the current charge %.
	/// </summary>
	/// <returns>The  current charge %.</returns>
	public float getCharge(){
		
		return charge;
	}

	/// <summary>
	/// Uses or Unlocks a power-up.
	/// </summary>
	/// <param name="powerUpID">The ID of the power-up being used.</param>
	public void usePowerUp( int powerUpID ){
			
		//UNLOCK ABILITY
		//check for enough charge and that another ability isnt currently being unlocked
		if (charge >= powerUps[powerUpID].getChargeRequired() && terminalState == TerminalState.Idle && powerUps[powerUpID].isLocked()) {

			//tooltip
			tipManager.DismissTip(3); // unlock button
			tipManager.ShowTip(9); //cannot charge

			//set unlocking to true
			terminalState = TerminalState.Unlocking;
			unlockID = powerUpID;
		}

		//USE ABILITY
		else if (powerUps[powerUpID].isLocked() == false && terminalState == TerminalState.Idle && powerUps[powerUpID].OnCoolDown() == false) {

			//dismiss tool tip
			tipManager.DismissTip(4);
			tipManager.DismissTip(5);
			//show cooldowntip
			tipManager.ShowTip(8);

			terminalState = TerminalState.UsingPowerUp;
			useID = powerUpID;
		}
	}
		
	/// <summary>
	/// Gets the state of the terminal.
	/// </summary>
	/// <returns>The terminal state.</returns>
	public TerminalState getTerminalState(){
		return terminalState;
	}

}
