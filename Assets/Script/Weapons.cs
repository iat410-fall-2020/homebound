using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
	public int magazineSize;
	public int currentMag;
	public float reloadTime;
	public float reloadCountDown;
	public int totalAmmo;
	public float power;
	public GameObject bullet;
	public Transform barrelPivot;
	protected bool reloading = false;

    // Start is called before the first frame update
    void Start()
    {
        currentMag = magazineSize;
    }

    // Update is called once per frame
    void Update()
    {
    	

        if (reloading) {
        	reloadCountDown -= Time.deltaTime;

        	if (reloadCountDown <= 0) {
        		reloading = false;

        		currentMag = Mathf.Min(magazineSize, totalAmmo);
        		totalAmmo -= magazineSize;
        		totalAmmo = Mathf.Max(0, totalAmmo);
        	}
        }
    }

    public void Fire() 
    {
    	if (!reloading) {
    		WeaponBehavior();

    		if (currentMag <= 0) {
	    		reloading = true;

	    		reloadCountDown = reloadTime;
	    	}
    	}
    	
    }

    protected virtual void WeaponBehavior () {
    	// this method is meant to override
    	Debug.Log("Fire " + transform.name);
    }
}
