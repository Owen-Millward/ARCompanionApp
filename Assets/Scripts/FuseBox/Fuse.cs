using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuse : MonoBehaviour {


	[SerializeField]
	private Material onMaterial;
	[SerializeField]
	private Material offMaterial;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void turnOff(){
		gameObject.GetComponent<MeshRenderer> ().materials [1].color = Color.grey;
	}

	public void turnOn(){
		gameObject.GetComponent<MeshRenderer> ().materials [1].color = Color.yellow;
	}
}
