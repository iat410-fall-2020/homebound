using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableWeapon : Weapons
{

	public Transform aimReference;
	bool isAiming = false;

	public Material placedDown;
	public Material aiming;


    protected override void constantUpdate() 
    {

    	if (aimReference != null) {
    		if (isAiming) {
    		aimReference.gameObject.SetActive(true);
	    	}
	    	else {
	    		aimReference.gameObject.SetActive(false);
	    	}
    	} 


    	isAiming = false;
    }

    public void PlaceReference (RaycastHit hit) {
    	if (currentMag > 0) {
    		if (aimReference == null) {
    			aimReference = Instantiate(bullet).GetComponent<Transform>();

    			aimReference.GetComponent<Renderer>().material = aiming;
    		}

    		aimReference.position = hit.point;
    		aimReference.up = hit.normal;

    		customePlaceRefernce();

    		isAiming = true;
    	}
    }

    protected virtual void customePlaceRefernce() {

    }
}
