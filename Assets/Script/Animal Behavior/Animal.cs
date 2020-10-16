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

    public GameObject lure;
    public float lureDistance = 5f;
    public float lureTimer = -999;

    public bool seePlayer = false;
    public Transform playerlocation;

    Transform camLocation;
    public GameObject statusSprite;

    public Sprite angerSprite;
    public Sprite capturedSprite;
    public Sprite hungrySprite;
    public Sprite stunedSprite;
    public Sprite surprisedSprite;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        animator.SetBool(isStunedParam, isStuned);
        animator.SetBool(isCapturedParam, isCaptured);
        animator.SetBool(isLuredParam, isLured);

        camLocation = Camera.main.GetComponent<Transform>();
        statusSprite.GetComponent<SpriteRenderer>().sprite = null;
    }

    // Update is called once per frame
    void Update()
    {   
        Vector3 direction = (statusSprite.transform.position - camLocation.position).normalized;
        statusSprite.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));

        if (!isCaptured) {
            if (isStuned) {
                stunedTimer -= Time.deltaTime;

                if (stunedTimer <= 0) {
                    isStuned = false;

                    gameObject.GetComponent<AutoMoveRotate>().enabled = true;
                    animator.SetBool(isStunedParam, isStuned);
                    
                    statusSprite.GetComponent<SpriteRenderer>().sprite = null;
                }
            }
            else if (isLured) {
                luredAction();   

                // if reaches lured location
                if (animator.GetBool(isLuredParam))
                {          
                    lureTimer -= Time.deltaTime;

                    if (lureTimer < 0) {
                        ExitLure(lure);
                    } 
                }                     
            }
        }
        
    }

    protected virtual void luredAction() {
        // override action if get lured


        float distance = Vector3.Distance(lure.transform.position, transform.position);

        if (distance > lureDistance) {
            Vector3 direction = (lure.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            gameObject.GetComponent<Transform>().rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        else { // reaches lured location
            gameObject.GetComponent<AutoMoveRotate>().enabled = false;
            animator.SetBool(isLuredParam, isLured);

            statusSprite.GetComponent<SpriteRenderer>().sprite = hungrySprite;
        }
    }

    public void GetCaptured() {

        if (!isCaptured) {
            isCaptured = true;

            gameObject.GetComponent<AutoMoveRotate>().enabled = false;

            gameObject.GetComponent<Interactable>().enabled = true;
            gameObject.layer = LayerMask.NameToLayer("Interactable");
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

            animator.SetBool(isCapturedParam, isCaptured);

            interactCollider.enabled = true;

            statusSprite.GetComponent<SpriteRenderer>().sprite = capturedSprite;
        }
    }

    public void GetStuned(float f) {
        if (!isCaptured) {
            isStuned = true;
            ExitLure(lure);



            gameObject.GetComponent<AutoMoveRotate>().enabled = false;
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

            stunedTimer = f;

            animator.SetBool(isStunedParam, isStuned);

            statusSprite.GetComponent<SpriteRenderer>().sprite = stunedSprite; 
        }
    }

    public void GetLured(GameObject lurer, float luredTime) {

        if (!isCaptured && !isLured) {
            isLured = true;
            lure = lurer;
            lureTimer = luredTime;

            gameObject.GetComponent<AutoMoveRotate>().rotation = new Vector3();

            statusSprite.GetComponent<SpriteRenderer>().sprite = surprisedSprite;
        }    
    }

    public void ExitLure(GameObject inputLure) {
        if (lure != null && lure == inputLure) {
            isLured = false;
            lure = null;

            gameObject.GetComponent<AutoMoveRotate>().rotation = new Vector3(0 , Random.Range(-.9f, .9f) , 0);

            if (!isCaptured && !isStuned) {
                
                animator.SetBool(isLuredParam, isLured);
                gameObject.GetComponent<AutoMoveRotate>().enabled = true;
                statusSprite.GetComponent<SpriteRenderer>().sprite = null;
            }
        }
        
        
    }
}
