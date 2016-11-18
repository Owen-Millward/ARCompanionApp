using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
    
	public float speed;
    
	private GameObject target;
    private MeshRenderer rend;

	// Use this for initialization
	void Start () {

		Rigidbody body = GetComponent<Rigidbody>();
        rend = GetComponent<MeshRenderer>();
        rend.material.SetColor("_Color", Color.blue);

        // Find target object
        target = GameObject.FindWithTag("Player");

		// Calculate direction (target position - position)
		Vector3 direction = target.transform.position - transform.position;
        body.velocity = direction * speed;
    }
	
	// Update is called once per frame
	void Update () {

        float distance = Vector3.Distance(transform.position, target.transform.position);
        if(distance > 40.0f) { Destroy(this.gameObject); }
    }

    void OnTriggerEnter(Collider collider){

        if (collider.tag == "Player"){
            rend.material.SetColor("_Color", Color.red);
        }
    }
}