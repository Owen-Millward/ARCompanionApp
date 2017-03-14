using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class UnlockBar : MonoBehaviour {

	[Tooltip("The Loading bar (this)")]
	public Transform LoadingBar;		//Loading bar
	[Tooltip("PowerUpManager (Managers)")]
	public PowerupManager PowerMan;		//Power up manager
	[Tooltip("ID of the associated powerup")]
	public int ID;						//ID of the powerup being represented 

	/// <summary>
	/// Display the cooldown of a powerup in the form of a filled horizontal bar
	/// </summary>
	void Update () {
		
		LoadingBar.GetComponent<Image> ().fillAmount = 1.0f * PowerMan.powerUps[ID].coolDownPercent();
	}
}
