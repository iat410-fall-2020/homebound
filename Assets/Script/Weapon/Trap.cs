using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
	bool placed = false;
	public float checkDistance;
	public Transform pairTrap;
	public Transform selfLocation;
	public GameObject partical;

	public LayerMask animalLayer;

    public float selfDestoryTimer;

    // Start is called before the first frame update
    void Start()
    {
        partical.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (placed) {

        	RaycastHit hit;

            Vector3 direction = (pairTrap.position - partical.GetComponent<Transform>().position).normalized;

        	if (Physics.Raycast(partical.GetComponent<Transform>().position, direction, out hit, checkDistance / 2.0f , animalLayer)) {

                hit.collider.transform.root.GetComponent<Animal>().GetCaptured();
        	}
        }
    }

    public void Place(Transform otherOne, float distance) {
    	placed = true;
    	partical.SetActive(true);
    	checkDistance = distance;
    	Destroy(gameObject, selfDestoryTimer);

    	pairTrap = otherOne;

    	Vector3 direction = (otherOne.position - partical.GetComponent<Transform>().position);

        partical.GetComponent<Transform>().rotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
    }
}
