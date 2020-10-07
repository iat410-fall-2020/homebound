using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LureHoldable : PlaceableWeapon
{
	

	public Material warning;
	float angle;
	public float warningAngle;

    protected override void WeaponBehavior ()
    {
    	if (angle < warningAngle) {
    		aimReference.GetComponent<Renderer>().material = placedDown;
    		aimReference.GetComponent<Lure>().Place();
			aimReference = null;

			--currentMag;
    	}		
    }

    protected override void customePlaceRefernce() {
    	angle = Mathf.Abs(Vector3.SignedAngle(new Vector3(0, 1, 0), aimReference.up, aimReference.forward));
    		
		if (angle >= warningAngle) {
			aimReference.GetComponent<Renderer>().material = warning;
		}
		else {
			aimReference.GetComponent<Renderer>().material = aiming;
		}
    }

    
}
