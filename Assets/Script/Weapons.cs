using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
	public int magazineSize;
	public int currentMag;
	public float reloadTime;
	float reloadCountDown;
	public int totalAmmo;
	public float power;
	public GameObject bullet;
	public Transform shootArea;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (reloadCountDown >= 0) {
        	reloadCountDown -= Time.deltaTime;
        }
    }

    public virtual void Fire() 
    {
    	// this method is meant to override
    	Debug.Log("Fire " + transform.name);
    }
}
