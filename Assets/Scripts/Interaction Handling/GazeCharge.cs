using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeCharge : MonoBehaviour {

	private bool gazing;
	[Tooltip("PowerUpManager (Managers)")]
	public PowerupManager PowerMan;
	[Tooltip("FuseBox (Handle)")]
	public FuseBox fuseBox;

	void Start () {
		gazing = false;
	}

	/// <summary>
	/// Handles Charging
	/// </summary>
	void Update () {

		//If the player is looking at the terminal charge 
		if (gazing == true) {
			PowerMan.charging ();
		}
	

		if (GetComponent<TextMesh> () != null) {
			
			//if working change color to percent charge and display charge 
			if (fuseBox.getState () == FuseBox.FuseState.working) {
				GetComponent<Renderer> ().material.color = new Color (1.0f-(PowerMan.getCharge()/100.0f), PowerMan.getCharge()/100.0f, 0);
				GetComponent<TextMesh> ().text = ((int)PowerMan.getCharge ()).ToString () + '%';
			} 
			else 
			{
				//if not working change color to red and diaply text offline
				GetComponent<TextMesh> ().text = "OFFLINE";
				GetComponent<TextMesh> ().color = Color.red;
			}

		}
	}

	/// <summary>
	/// Response to GazeEntered Message
	/// </summary>
	void GazeEntered()
	{
		gazing = true;
	}

	/// <summary>
	/// Response to GazeExited Message
	/// </summary>
	void GazeExited()
	{
		gazing = false;
	}

}
