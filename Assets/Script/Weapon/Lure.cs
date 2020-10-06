using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lure : MonoBehaviour
{
	public Renderer render;

	public Material placedDown;
	public Material aiming;
	public Material collides;

	bool placed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!placed) {
        	render.material = aiming;
        }


    }

    void OnCollisionStay(Collision collisionInfo)
    {
        Debug.Log("collides");
    	render.material = collides;
    }

    public void Place() {
    	placed = true;
    	render.material = placedDown;
    }


}
