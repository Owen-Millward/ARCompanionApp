using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour {

	private float charge = 0.0f;
	public int PowerLimitOne;
	public int PowerLimitTwo;
	public int PowerLimitThree;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void charging(){
		if (charge < 100.0f) {
			charge += Time.deltaTime * 3;
		}
		if (charge > 100.0f) {
			charge = 100.0f;
		}
	}

	public float getCharge(){
		return charge;
	}

	public void usePowerUp( int cost ){
		charge -= cost;
	}

	public int getPowerLimit(int limitID){
		switch (limitID) {
		case 1:
			return PowerLimitOne;
			break;
		case 2: 
			return PowerLimitTwo;
			break;
		case 3:
			return PowerLimitThree;
			break;
		}
		return 0;
	}
}
