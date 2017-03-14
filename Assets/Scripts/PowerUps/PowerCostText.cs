using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCostText : MonoBehaviour {

	private int cost; 					//Cost to unlock powerup
	[Tooltip("ID of this powerup represented")]
	public int ID;						//Powerups ID
	[Tooltip("PowerUpManager (Managers)")]
	public PowerupManager PowerMan;		//Powermanager storing powerups

	/// <summary>
	/// Get the charge required from the powermanager and display in game
	/// </summary>
	void Start () {
		cost = PowerMan.powerUps[ID].getChargeRequired();
		GetComponent<TextMesh> ().text = cost.ToString() + '%';
	}
	
	/// <summary>
	/// If the powerup is avaliable highlight the text
	/// </summary>
	void Update () {
		if (PowerMan.powerUps[ID].isLocked()) {
			GetComponent<TextMesh> ().color = new Color (0.5f, 0.5f, 0.5f);
		} else {
			GetComponent<TextMesh> ().color = new Color (1.0f, 1.0f, 1.0f);
		}
	}
}
