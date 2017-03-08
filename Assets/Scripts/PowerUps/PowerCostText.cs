using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCostText : MonoBehaviour {

	private int cost;
	public int ID;
	public PowerupManager PowerMan;

	// Use this for initialization
	void Start () {
		cost = PowerMan.getPowerLimit (ID);
		GetComponent<TextMesh> ().text = cost.ToString() + '%';
	}
	
	// Update is called once per frame
	void Update () {
		if (PowerMan.getCharge () < cost) {
			GetComponent<TextMesh> ().color = new Color (0.5f, 0.5f, 0.5f);
		} else {
			GetComponent<TextMesh> ().color = new Color (1.0f, 1.0f, 1.0f);
		}
	}
}
