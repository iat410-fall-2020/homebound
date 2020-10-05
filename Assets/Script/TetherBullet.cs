using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherBullet : MonoBehaviour
{
	public float selfDestroyTimer;
	bool hit = false;

	public LayerMask animalLayer;
	public LayerMask ignoreLayer;
	public float stunTime;
	public float criticalStun;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
    	if (!hit && 
            ((ignoreLayer & 1 << collision.collider.gameObject.layer) != 1 << collision.collider.gameObject.layer)) {

    		hit = true;

    		Rigidbody rb = GetComponent<Rigidbody>();
    		rb.velocity = Vector3.zero;

 			rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
    		rb.isKinematic = true;

    		Destroy(gameObject, selfDestroyTimer);


    		if ((animalLayer & 1 << collision.collider.gameObject.layer) == 1 << collision.collider.gameObject.layer){

	    		GameObject hitAnimal = collision.collider.transform.root.gameObject;

	    		if (GetComponent<Collider>().GetComponent<Collider>().gameObject.tag == "Head") {
	    			hitAnimal.GetComponent<Animal>().GetStuned(criticalStun);

	    			Debug.Log("head get hit");
	    		}
	    		else {
	    			hitAnimal.GetComponent<Animal>().GetStuned(stunTime);
	    		}

	    		
	    	}
    	}
    }
}
