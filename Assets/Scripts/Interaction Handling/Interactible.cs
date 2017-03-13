using UnityEngine;

/// <summary>
/// The Interactible class flags a Game Object as being "Interactible".
/// Determines what happens when an Interactible is being gazed at.
/// </summary>
public class Interactible : MonoBehaviour
{
    [Tooltip("Audio clip to play when interacting with this hologram.")]
    public AudioClip TargetFeedbackSound;
    private AudioSource audioSource;

	public AudioClip[] AudioClips;

	public enum ButtonType {MIC, SHOCK, PURGE, SHIELD, FUSE, DOOR};
	[Tooltip("Which type of button is this?")]
	public ButtonType Button;

	// Get communicator and power up manager
	private PowerUpController pwr;
	private Communicator comm;	
	public FuseBox fuseBox;
	public PowerupManager PowerMan;
	public Door door;
	private bool unlocked = false;

	private int limitOne, limitTwo, limitThree;
	private int charge;
    private Material[] defaultMaterials;


    void Start()
    {
		charge = (int)PowerMan.getCharge ();
		limitOne = PowerMan.getPowerLimit (1);
		limitTwo = PowerMan.getPowerLimit (2);
		limitThree = PowerMan.getPowerLimit (3);

        defaultMaterials = GetComponent<Renderer>().materials;

        // Add a BoxCollider if the interactible does not contain one.
        Collider collider = GetComponentInChildren<Collider>();
        if (collider == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }

		// Get Communicator for starting voice recording
		comm = GameObject.Find ("Communicator").GetComponent<Communicator> ();

		// Get power up controller for setting spawn flags
		pwr =  GameObject.Find("PowerUpAzure").GetComponent<PowerUpController>();

        EnableAudioHapticFeedback();
    }

    private void EnableAudioHapticFeedback()
    {
        // If this hologram has an audio clip, add an AudioSource with this clip.
        if (TargetFeedbackSound != null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.clip = TargetFeedbackSound;
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1;
            audioSource.dopplerLevel = 0;
        }
    }

	void Update(){
		//IF THE FUSE BLOWS WHILE RECORDING
		if (fuseBox.getState () == FuseBox.FuseState.broken) {
			if (comm.recording) {
				audioSource.clip = AudioClips [1];
				audioSource.Play ();
				comm.RecordStop ();
			}
		}
	}

    void OnSelect()
	{
		
		for (int i = 0; i < defaultMaterials.Length; i++) {
			defaultMaterials [i].SetFloat ("_Highlight", .5f);
		}
			

		charge = (int)PowerMan.getCharge ();

		// Decide what to do when pressed
		switch (Button) {
			
			//MICROPHONE BUTTON
			case ButtonType.MIC:
				
				//IF THE FUSE IS BROKEN 
				if (fuseBox.getState () == FuseBox.FuseState.broken) {
					audioSource.clip = AudioClips [2];
					audioSource.Play ();
				} 
				//IF THE FUSE IS WORKING
				else {
					//IF RECORDING
					if (comm.recording) {
						audioSource.clip = AudioClips [1];
						audioSource.Play ();
						comm.RecordStop ();
					//IF NOT RECORDING
					} else {
						audioSource.clip = AudioClips [0];
						audioSource.Play ();
						comm.Record ();
					}
				}

				break;

			//SHOCKWAVE BUTTON 
			case ButtonType.SHOCK:
				
				//IF FUSE IS BROKEN
				if (fuseBox.getState () == FuseBox.FuseState.broken) {
					audioSource.clip = AudioClips [1];
					audioSource.Play ();
				}
				//IF FUSE IS WORKING
				else if (fuseBox.getState () == FuseBox.FuseState.working) {
					//POWERUP IS OFF COOLDOWN
					if (PowerMan.getTime (1) >= PowerMan.getUnlockTime () && PowerMan.unlockingPowerUP == false) {
						pwr.ShockButton ();
						audioSource.clip = AudioClips [0];
						audioSource.Play ();
						
					//POWER UP IS ON COOLDOWN		
					} else {
						if (!unlocked) {
							audioSource.clip = AudioClips [0];
							unlocked = true;
						} else {
							audioSource.clip = AudioClips [1];
						}
						audioSource.Play ();
					}

					PowerMan.usePowerUp (1);
				}
				break;
				
			case ButtonType.PURGE:
	
			//IF FUSE IS BROKEN
			if (fuseBox.getState () == FuseBox.FuseState.broken) {
				audioSource.clip = AudioClips [1];
				audioSource.Play ();
			}
			//IF FUSE IS WORKING
			else if (fuseBox.getState () == FuseBox.FuseState.working) {
				//POWERUP IS OFF COOLDOWN
				if (PowerMan.getTime (3) >= PowerMan.getUnlockTime () && PowerMan.unlockingPowerUP == false) {
					pwr.PurgeButton ();
					audioSource.clip = AudioClips [0];
					audioSource.Play ();

					//POWER UP IS ON COOLDOWN		
				} else {
					if (!unlocked) {
						audioSource.clip = AudioClips [0];
						unlocked = true;
					} else {
						audioSource.clip = AudioClips [1];
					}
					audioSource.Play ();
				}

				PowerMan.usePowerUp (3);
			}
				break;
				
			case ButtonType.SHIELD:
		
			//IF FUSE IS BROKEN
			if (fuseBox.getState () == FuseBox.FuseState.broken) {
				audioSource.clip = AudioClips [1];
				audioSource.Play ();
			}
			//IF FUSE IS WORKING
			else if (fuseBox.getState () == FuseBox.FuseState.working) {
				//POWERUP IS OFF COOLDOWN
				if (PowerMan.getTime (2) >= PowerMan.getUnlockTime () && PowerMan.unlockingPowerUP == false) {
					pwr.SheildButton ();
					audioSource.clip = AudioClips [0];
					audioSource.Play ();

					//POWER UP IS ON COOLDOWN		
				} else {
					if (!unlocked) {
						audioSource.clip = AudioClips [0];
						unlocked = true;
					} else {
						audioSource.clip = AudioClips [1];
					}
					audioSource.Play ();
				}

				PowerMan.usePowerUp (2);
			}
				break;

			case ButtonType.DOOR:

				door.useDoor ();
				
				break;
			
		}
		
	
		if (fuseBox.getState () == FuseBox.FuseState.broken) {
			if (Button == ButtonType.FUSE) {
				fuseBox.fuseFix ();
				audioSource.Play ();
			}
		} 
		else {
		}
			

		

			
		this.SendMessage ("PerformTagAlong");
	

    }
}