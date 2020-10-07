using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lure : MonoBehaviour
{

	public LayerMask animalLayer;
	public SphereCollider lureCollider;

	public float lureRange;

	bool placed = false;

    // Start is called before the first frame update
    void Start()
    {
        lureCollider.radius = lureRange;
    }

    // Update is called once per frame
    void Update()
    {
        if (placed) {

        }


    }

    public void Place() {
    	placed = true;
    }

    void OnTriggerStay(Collider collider) {
    	if (placed && (animalLayer & 1 << collider.gameObject.layer) == 1 << collider.gameObject.layer) {
            
    		collider.transform.root.gameObject.GetComponent<Animal>().GetLured(gameObject);
    	}
    }


}
