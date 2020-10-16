using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapturedAnimal))]

public class Animal : MonoBehaviour
{
	[System.NonSerialized] public bool isStuned = false;
	[System.NonSerialized] public bool isCaptured = false;
	[System.NonSerialized] public bool isLured = false;
	public Collider interactCollider;

	[System.NonSerialized] public float stunedTimer;

	protected Animator animator;

	protected int isStunedParam = Animator.StringToHash("isStuned");
	protected int isCapturedParam = Animator.StringToHash("isCaptured");
	protected int isLuredParam = Animator.StringToHash("isLured");

    public GameObject lure;
    public float lureDistance = 5f;
    [System.NonSerialized] public float lureTimer = -999;

    public bool seePlayer = false;
    public Transform playerlocation;

    Transform camLocation;
    public GameObject statusSprite;

    public Sprite angerSprite, capturedSprite, hungrySprite, stunedSprite, surprisedSprite;

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

                    moveAgain();
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

    protected virtual void stop() {
        gameObject.GetComponent<AutoMoveRotate>().enabled = false;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    protected virtual void moveAgain() {
        gameObject.GetComponent<AutoMoveRotate>().rotation = new Vector3(0 , Random.Range(-.9f, .9f) , 0);
        gameObject.GetComponent<AutoMoveRotate>().enabled = true;
    }

    protected virtual void toLocation(Transform targetLocation) {
        Vector3 direction = (targetLocation.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        gameObject.GetComponent<Transform>().rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    protected virtual void luredAction() {
        // override action if get lured


        float distance = Vector3.Distance(lure.transform.position, transform.position);

        if (distance > lureDistance) {
            toLocation(lure.transform);
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

            stop();            

            gameObject.GetComponent<Interactable>().enabled = true;
            gameObject.layer = LayerMask.NameToLayer("Interactable");

            animator.SetBool(isCapturedParam, isCaptured);

            interactCollider.enabled = true;

            statusSprite.GetComponent<SpriteRenderer>().sprite = capturedSprite;
        }
    }

    public void GetStuned(float f) {
        if (!isCaptured) {
            isStuned = true;
            ExitLure(lure);

            stop();

            gameObject.GetComponent<AutoMoveRotate>().enabled = false;
            

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

            statusSprite.GetComponent<SpriteRenderer>().sprite = surprisedSprite;
        }    
    }

    public void ExitLure(GameObject inputLure) {
        if (lure != null && lure == inputLure) {
            isLured = false;
            lure = null;

            if (!isCaptured && !isStuned) {

                moveAgain();
                
                animator.SetBool(isLuredParam, isLured);
                statusSprite.GetComponent<SpriteRenderer>().sprite = null;
            }
        }
        
        
    }
}
