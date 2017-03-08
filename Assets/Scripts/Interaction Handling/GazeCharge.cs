using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeCharge : MonoBehaviour {

	private bool gazing;
	public PowerupManager PowerMan;
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
	
		GetComponent<Renderer> ().material.color = new Color (1.0f-(PowerMan.getCharge()/100.0f), PowerMan.getCharge()/100.0f, 0);
		if (GetComponent<TextMesh> () != null) {
			GetComponent<TextMesh> ().text = ((int)PowerMan.getCharge ()).ToString () + '%';
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
