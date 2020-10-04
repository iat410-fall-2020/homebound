using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetBullet : MonoBehaviour
{
	public GameObject checker;
	public float selfDestroyTimer;
	bool selfDestory = false;

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
    	if (!selfDestory) {
    		selfDestory = true;

    		Rigidbody rb = GetComponent<Rigidbody>();
    		rb.velocity = Vector3.zero;

    		checker.SetActive(true);

    		rb.isKinematic = true;

    		Destroy(gameObject, selfDestroyTimer);
    	}
    }

    void OnTriggerEnter(Collider collider)
    {
    	if (selfDestory && collider.gameObject.layer == LayerMask.NameToLayer("Animal")){
    		Destroy(collider.gameObject.GetComponent<Animator>());
    		Destroy(collider.gameObject.GetComponent<AutoMoveRotate>());



    		collider.gameObject.GetComponent<Interactable>().enabled = true;
    		collider.gameObject.layer = LayerMask.NameToLayer("Interactable");
    		collider.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

    		//ChangeLayersRecursively(collider.gameObject.transform, "Interactable");

    		
    	}
        
        
    }

    void ChangeLayersRecursively(Transform trans, string name)
	{
	     trans.gameObject.layer = LayerMask.NameToLayer(name);
	     foreach(Transform child in trans)
	     {            
	         ChangeLayersRecursively(child, name);
	     }
	}
}
