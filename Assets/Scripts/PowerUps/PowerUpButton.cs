using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpButton : MonoBehaviour {

	public int ID;
	public PowerupManager PowerMan;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		//if the powerup is unlocked highlight image 
		if (PowerMan.getTime(ID) < PowerMan.getUnlockTime()) {
			GetComponent<SpriteRenderer> ().color = new Color (0.4f, 0.4f, 0.4f);
		} else {
			GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f);
		}
	}
}
