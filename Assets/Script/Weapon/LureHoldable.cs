using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LureHoldable : PlaceableWeapon
{
	
	public float warningAngle;

    protected override void WeaponBehavior ()
    {
    	if (isAiming && fire) {
    		aimReference.GetComponent<Renderer>().material = placedDown;
    		aimReference.GetComponent<Lure>().Place();
			aimReference = null;

			--currentMag;

			fire = false;
    	}		
    }

    protected override void customePlaceRefernce() {
    	float angle = Mathf.Abs(Vector3.SignedAngle(new Vector3(0, 1, 0), aimReference.up, aimReference.forward));
    		
		if (angle >= warningAngle) {
			aimReference.GetComponent<Renderer>().material = warning;

			fire = false;
		}
		else {
			aimReference.GetComponent<Renderer>().material = aiming;

			fire = true;
		}
    }

    
}
