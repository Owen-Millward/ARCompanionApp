using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpButton : MonoBehaviour {

	[Tooltip("ID of the associated powerup")]
	public int ID;						//Id of the associated powerup
	[Tooltip("PowerUpManager (Managers)")]
	public PowerupManager PowerMan;		//power up manager 
	
	/// <summary>
	/// Highlights the image if it is unlocked
	/// </summary>
	void Update () {
 
		if (PowerMan.powerUps[ID].isLocked()) {
			
			GetComponent<SpriteRenderer> ().color = new Color (0.4f, 0.4f, 0.4f);
		}
		else {
			
			GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f);
		}
	}
}
