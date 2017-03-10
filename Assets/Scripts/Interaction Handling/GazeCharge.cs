using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeCharge : MonoBehaviour {

	private bool gazing;
	public PowerupManager PowerMan;
	public FuseBox fuseBox;

	// Use this for initialization
	void Start () {
		//GetComponent<Renderer> ().material.color = Color.red;
		gazing = false;
	}
	
	// Update is called once per frame
	void Update () {
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

	void GazeEntered()
	{
		//GetComponent<Renderer> ().material.color = Color.green;
		gazing = true;
	}

	void GazeExited()
	{
		gazing = false;
		//GetComponent<Renderer> ().material.color = Color.red;
	}

}
