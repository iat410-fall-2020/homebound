using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableWeapon : Weapons
{

	public Transform aimReference;
	protected bool isAiming = false;

	public Material placedDown;
	public Material aiming;
    public Material warning;

    protected bool fire = false;


    protected override void constantUpdate() 
    {
        if (currentMag > 0) {
            reloading = false;
        }

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

                fire = true;
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
