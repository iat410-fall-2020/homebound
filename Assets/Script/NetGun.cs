using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetGun : Weapons
{
    public override void Fire ()
    {
    	Rigidbody bulletrb = Instantiate(bullet, barrelPivot.position, barrelPivot.rotation).GetComponent<Rigidbody>();
    	bulletrb.velocity = barrelPivot.forward * power;
    }
}
