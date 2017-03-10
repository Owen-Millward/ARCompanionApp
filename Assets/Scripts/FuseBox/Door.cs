using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	public enum DoorState {open,closed};
	public DoorState doorState;

	private float rotationValue;
	public float rotationTime;

	// Use this for initialization
	void Start () {
		
		doorState = DoorState.closed;
	}
	
	// Update is called once per frame
	void Update () {
		rotateDoor ();
	}

	private void rotateDoor(){
		Debug.Log (transform.rotation.eulerAngles.y);
		if (doorState == DoorState.closed) {
			if (transform.rotation.eulerAngles.y < 0|| transform.rotation.eulerAngles.y > 230) {

			transform.Rotate (new Vector3 (0, rotationTime * 120 * Time.deltaTime , 0));
			}
		}
		if (doorState == DoorState.open) {
			if (transform.rotation.eulerAngles.y < 10  || transform.rotation.eulerAngles.y > 240) {

				transform.Rotate (new Vector3 (0,-rotationTime * 120 * Time.deltaTime , 0));
			}
		}
	}

	public void useDoor(){
		
		if (doorState == DoorState.open) {
			doorState = DoorState.closed;
		}

		else if (doorState == DoorState.closed) {
			doorState = DoorState.open;

		}
	}


}
