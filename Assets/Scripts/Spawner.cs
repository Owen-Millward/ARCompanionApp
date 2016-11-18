using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public GameObject Projectile; //object to be fired

	public float counter;		  //initial time between spawns
	public float radius;		  //distance from player for spawning objects

	private float setTime;		  //varied reset timer for spawn

	// Use this for initialization
	void Start(){

		//set timer to max time seed random
		setTime = counter;
	    Random.InitState((int)System.DateTime.Now.Ticks);
	}

	// Update is called once per frame
	void Update(){

		//set the timer to count in real time when it reaches 0 spawn a projectile and reset to new time
		counter -= Time.deltaTime;
		if (counter < 0) {
			Spawn();
			counter = setTime;
		}
	}

	void Spawn(){

		//calculate a random angle and use to find random spawn location on a circle around the player
		//instantiate projectile prefab at this location
		float rand = Random.Range (0.0f, 2.0f * Mathf.PI);
		float tempX = 0 +  Mathf.Cos (rand) * radius;
		float tempZ = 0 +  Mathf.Sin (rand) * radius;
		transform.position.Set (tempX, 1, tempZ);

		Instantiate(Projectile, transform.position, Quaternion.identity);
	}
}
