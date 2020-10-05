using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Animal : MonoBehaviour
{
	public bool isStuned = false;
	public bool isCaptured = false;
	public bool isLured = false;
	public Collider interactCollider;

	public float stunedTimer;

	Animator animator;

	int isStunedParam = Animator.StringToHash("isStuned");
	int isCapturedParam = Animator.StringToHash("isCaptured");
	int isLuredParam = Animator.StringToHash("isLured");

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStuned) {
        	stunedTimer -= Time.deltaTime;

        	if (stunedTimer <= 0) {
        		isStuned = false;

        		gameObject.GetComponent<AutoMoveRotate>().enabled = true;
				animator.SetBool(isStunedParam, isStuned);
        	}
        }
    }

    public void GetCaptured() {

    	isCaptured = true;

    	gameObject.GetComponent<AutoMoveRotate>().enabled = false;

    	gameObject.GetComponent<Interactable>().enabled = true;
		gameObject.layer = LayerMask.NameToLayer("Interactable");
		gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

		animator.SetBool(isCapturedParam, isCaptured);

		interactCollider.enabled = true;
    }

    public void GetStuned(float f) {
    	isStuned = true;

    	gameObject.GetComponent<AutoMoveRotate>().enabled = false;
		gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

		stunedTimer = f;

		animator.SetBool(isStunedParam, isStuned);
    }
}
