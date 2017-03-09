using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class UnlockBar : MonoBehaviour {

	public Transform LoadingBar;
	public PowerupManager PowerMan;
	public int ID;

	// Update is called once per frame
	void Update () {
		if (PowerMan.getTime (ID) == 0) {
			LoadingBar.GetComponent<Image> ().fillAmount = 0;
		} 
		else {
			LoadingBar.GetComponent<Image> ().fillAmount = 1.0f * (PowerMan.getTime (ID) / PowerMan.getUnlockTime ());
		}
	}
}
