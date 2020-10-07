using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lure : MonoBehaviour
{

	public LayerMask animalLayer;
	public SphereCollider lureCollider;

	public float lureRange;

	public float duration;

	bool placed = false;

    // Start is called before the first frame update
    void Start()
    {
        lureCollider.radius = lureRange;
        lureCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (placed) {
        	duration -= Time.deltaTime;

        	if (duration <= 0) {
        		Destroy(gameObject);
        	}
        }


    }

    public void Place() {
    	placed = true;
    	lureCollider.enabled = true;
    }

    void OnTriggerEnter(Collider collider) {
    	if (placed && (animalLayer & 1 << collider.gameObject.layer) == 1 << collider.gameObject.layer) {
            
    		collider.transform.root.gameObject.GetComponent<Animal>().GetLured(gameObject);
    	}
    }

    void OnTriggerStay(Collider collider) {
    	if (duration <= 1
    		&& (animalLayer & 1 << collider.gameObject.layer) == 1 << collider.gameObject.layer) {
    		Debug.Log("exit lure");
    		collider.transform.root.gameObject.GetComponent<Animal>().ExitLure();
    	}
    }


}
