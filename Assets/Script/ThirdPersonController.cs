using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
	Rigidbody rb;
    public Camera cam;
	Transform camTransform;
	public UIScript UI;

	public float speed = 16f;
	public float maxVelocityChange = 10f;
	Vector3 moveDir;

	public float turnSmoothTime = 0.1f;
	float turnSmoothVelocity;

    public float boosting = 2f;
	public float liftingpSpeed = 2f;
	public float maxLiftingSpeed = 10f;

	public float boostingCost = 5f;
	public float liftingpCost = 10f;

    public LayerMask ignoredLayer;

    public GameObject focus;

    public float maxEnergy = 600f;
    public float currentEnergy;

    public int currentResource = 0; 

    public bool pathFinding = false;
	

    // Start is called before the first frame update
    void Start()
    {
    	rb = GetComponent<Rigidbody>();

        camTransform = cam.GetComponent<Transform>();

        currentEnergy = maxEnergy;
    }

    // Update is called once per frame
    void Update()
    {

    	// rotation 
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        moveDir = Vector3.zero;

        if (direction.magnitude >= 0.1f) 
        {
        	float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camTransform.eulerAngles.y;
        	float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        	transform.rotation = Quaternion.Euler(0f, angle, 0f);

        	moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        	//controller.Move(moveDir.normalized * speed * Time.deltaTime);

        }

        // interact with object by pressing F 

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, ~ignoredLayer)) {

            if (hit.transform != null 
                && hit.transform.gameObject.layer == LayerMask.NameToLayer("Interactable")) {

                GameObject interactable = hit.transform.gameObject;
                bool closeToInteract = interactable.GetComponent<Interactable>().distanceCheck(transform);

                if (Input.GetKeyDown(KeyCode.F) && closeToInteract) {

                    SetFocus(interactable);
                }                
            }
            
        }

        Debug.DrawLine(camTransform.position, hit.point,Color.red);
        
        if (Input.anyKey && !Input.GetKey(KeyCode.F)){
            //remove focus for any other key input(for now, maybe better way next time)
            RemoveFocus();
        }

        if (focus != null) {
            FaceTarget();
        }

        // interaction part end

        // minimap on off

        if (Input.GetKeyDown(KeyCode.M)) {
        	UI.AffectMinimap();
        }


        // reduce engery overtime, every second coust 1 energy
        currentEnergy -= Time.deltaTime;
        currentEnergy = Mathf.Max(currentEnergy, 0); 
    }


    void SetFocus(GameObject newFocus)
    {
        if (newFocus != focus) 
        {
            if (focus != null)
            {
                focus.GetComponent<Interactable>().OnDefocused();
            }

            focus = newFocus;
        }
        
        newFocus.GetComponent<Interactable>().OnFocused(gameObject);
    }

    void RemoveFocus() {

        if (focus != null)
        {
            focus.GetComponent<Interactable>().OnDefocused();
        }
        
        focus = null;
    }

    void FaceTarget() {
        Vector3 direction = (focus.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void FixedUpdate()
    {	

    	// if (Input.GetKey(KeyCode.LeftShift)) {
    	// 	rb.MovePosition(rb.position + moveDir * speed * boosting * Time.fixedDeltaTime);
    	// }
    	// else {
    	// 	rb.MovePosition(rb.position + moveDir * speed * Time.fixedDeltaTime);
    	// }

    	// Apply a force that attempts to reach our target velocity
        Vector3 velocity = rb.velocity;

        Vector3 velocityChange = (moveDir * speed - velocity);
        if (Input.GetKey(KeyCode.LeftShift) && currentEnergy > 0) {
    		velocityChange = (moveDir * speed * boosting - velocity);
    		currentEnergy -= Time.deltaTime * (boostingCost - 1f);
    	}

        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        rb.AddForce(velocityChange, ForceMode.VelocityChange);


        // apply lifing force when pressing space
        if (Input.GetKey(KeyCode.Space) && currentEnergy > 0)
        {
        	// Debug.Log("spaced hited");
        	rb.AddForce(Vector3.up * (liftingpSpeed * (1 - (rb.velocity.y / maxLiftingSpeed))), ForceMode.Acceleration);
        	currentEnergy -= Time.deltaTime * (liftingpCost - 1f);
        }

    }
}
