using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapHoldable : PlaceableWeapon
{
	Transform firstTrap;

	public float MaxDistance;

    protected override void WeaponBehavior ()
    {
    	if (isAiming && fire) {
    		aimReference.GetComponent<Renderer>().material = placedDown;	


			if (firstTrap == null) {
				firstTrap = aimReference;
			}

			else {
				firstTrap.GetComponent<Trap>().Place(aimReference.GetComponent<Trap>().selfLocation , MaxDistance);
				aimReference.GetComponent<Trap>().Place(firstTrap.GetComponent<Trap>().selfLocation , MaxDistance);

				firstTrap = null;
			}

			aimReference = null;

			fire = false;
			--currentMag;	
    	}
		
    }

    protected override void customePlaceRefernce() {

		if (firstTrap != null) {
			float distanceCheck = Vector3.Distance(firstTrap.position, aimReference.position);

			if (distanceCheck >= MaxDistance) {
				aimReference.GetComponent<Renderer>().material = warning;

				fire = false;
			}
			else {
				RaycastHit hit;

				Vector3 direction = (firstTrap.GetComponent<Trap>().selfLocation.position
				 - aimReference.GetComponent<Trap>().selfLocation.position).normalized;

        		if (Physics.Raycast(aimReference.GetComponent<Trap>().selfLocation.position, direction, out hit, MaxDistance) &&
					hit.collider != firstTrap.GetComponent<Collider>()) {


    				aimReference.GetComponent<Renderer>().material = warning;
					fire = false;

        		}

        		else {
        			aimReference.GetComponent<Renderer>().material = aiming;

					fire = true;
        		}
				
			}
		}
    }
}
