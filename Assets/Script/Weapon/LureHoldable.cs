using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LureHoldable : PlaceableWeapon
{
    protected override void WeaponBehavior ()
    {
		aimReference.GetComponent<Lure>().Place();
		aimReference = null;


		--currentMag;	
    }
}
