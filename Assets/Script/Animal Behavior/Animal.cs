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

    GameObject lure;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCaptured) {
            if (isStuned) {
                stunedTimer -= Time.deltaTime;

                if (stunedTimer <= 0) {
                    isStuned = false;

                    gameObject.GetComponent<AutoMoveRotate>().enabled = true;
                    animator.SetBool(isStunedParam, isStuned);
                        
                }
            }
            else if (isLured) {
                float distance = Vector3.Distance(lure.transform.position, transform.position);

                if (distance > 7f) {
                    Vector3 direction = (lure.transform.position - transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
                    gameObject.GetComponent<Transform>().rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
                }
                else {
                    gameObject.GetComponent<AutoMoveRotate>().enabled = false;
                    animator.SetBool(isLuredParam, isLured);
                }

                
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

    public void GetLured(GameObject lurer) {
        isLured = true;
        lure = lurer;

        gameObject.GetComponent<AutoMoveRotate>().rotation = new Vector3();
    }
}
