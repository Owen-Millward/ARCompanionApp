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

	public enum ButtonType {MIC, SHOCK, PURGE, SHIELD};
	[Tooltip("Which type of button is this?")]
	public ButtonType Button;

	// Get communicator and power up manager
	private PowerUpController pwr;
	private Communicator comm;		
	public PowerupManager PowerMan;

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

    void GazeEntered()
    {
        for (int i = 0; i < defaultMaterials.Length; i++)
        {
            defaultMaterials[i].SetFloat("_Highlight", .25f);
        }
    }

    void GazeExited()
    {
        for (int i = 0; i < defaultMaterials.Length; i++)
        {
            defaultMaterials[i].SetFloat("_Highlight", 0f);
        }
    }

    void OnSelect()
    {
        for (int i = 0; i < defaultMaterials.Length; i++)
        {
            defaultMaterials[i].SetFloat("_Highlight", .5f);
        }

        // Play the audioSource feedback when we gaze and select a hologram.
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
		
        }

		charge = (int)PowerMan.getCharge ();

		// Decide what to do when pressed
		switch (Button) 
		{
		case ButtonType.MIC:
			if (comm.recording)
				comm.RecordStop ();			
			else 
				comm.Record ();
			break;
			
		case ButtonType.SHOCK:
			if (charge >= limitOne) {
				pwr.ShockButton ();
				PowerMan.usePowerUp ((int)PowerMan.getPowerLimit (1));
			} 
			break;
			
		case ButtonType.PURGE:
			if (charge >= limitThree) {
				pwr.PurgeButton ();
				PowerMan.usePowerUp ((int)PowerMan.getPowerLimit (3));
			}
			break;
			
		case ButtonType.SHIELD:
			if (charge >= limitTwo) {
				pwr.SheildButton ();
				PowerMan.usePowerUp ((int)PowerMan.getPowerLimit (2));
			}
			break;
		}

        this.SendMessage("PerformTagAlong");
    }
}