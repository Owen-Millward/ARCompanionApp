using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Player : MonoBehaviour {

    public float speed = 5.0f;
    public UIStick joystick;
    private Rigidbody body;

    void Start(){

        body = GetComponent<Rigidbody>();
    }
	
	void Update () {

        Vector3 MoveVector = Vector3.zero;

        MoveVector.x = joystick.getHorizontal();
        MoveVector.z = joystick.getVertical();

        transform.Translate(MoveVector * speed * Time.deltaTime);

    }

}