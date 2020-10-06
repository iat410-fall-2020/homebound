using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableWeapon : Weapons
{

	public Transform aimReference;
	bool isAiming = false;


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

    public void PlaceReference (Vector3 position) {
    	if (currentMag > 0) {
    		if (aimReference == null) {
    			aimReference = Instantiate(bullet).GetComponent<Transform>();

    		}

    		aimReference.position = position;

    		isAiming = true;
    	}
    }
}
