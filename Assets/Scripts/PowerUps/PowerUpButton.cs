using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpButton : MonoBehaviour {

	private int cost;
	public int ID;
	public PowerupManager PowerMan;

	// Use this for initialization
	void Start () {
		cost = PowerMan.getPowerLimit (ID);
	}
	
	// Update is called once per frame
	void Update () {
		if (PowerMan.getCharge () < cost) {
			GetComponent<SpriteRenderer> ().color = new Color (0.4f, 0.4f, 0.4f);
		} else {
			GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f);
		}
	}
}
