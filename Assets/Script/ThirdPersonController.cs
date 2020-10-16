using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonController : MonoBehaviour
{
	Rigidbody rb;

    public Camera cam;
    Transform camTransform;

    public CinemachineVirtualCamera cinemachines;

    public bool displayWeaponWhenWalking = false;

	public UIScript UI;

	public float speed = 16f;
	public float maxVelocityChange = 10f;
	Vector3 moveDir;

	public float turnSmoothTime = 0.1f;
	float turnSmoothVelocity;

    public float sprinting = 32f;
    public float sprintingCost = 5f;

	public float liftingpSpeed = 500f;
	public float maxLiftingSpeed = 10f;
    public float liftingpCost = 10f;

    public ParticleSystem liftingParticle;

    public float maxOverHeatTime = 30f;
    public float overHeatTimer = 30f;	
    public float liftingCoolDown = 5f;
    public float sprintCoolDown = 1f;


    public LayerMask targetLayer;

    public GameObject focus;

    public float maxEnergy = 600f;
    public float currentEnergy;

    public int currentResource = 0; 

    public bool pathFinding = false;

    public Transform weaponAttactedPoint;	
	public List<GameObject> weapons = new List<GameObject>();
	public int currentWeapon = 0;

    CharacterAiming characterAiming;




    // Start is called before the first frame update
    void Start()
    {
    	rb = GetComponent<Rigidbody>();

        camTransform = cam.GetComponent<Transform>();

        currentEnergy = maxEnergy;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        characterAiming = GetComponent<CharacterAiming>();

        overHeatTimer = maxOverHeatTime;

        liftingParticle.Pause();

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

        	if (!characterAiming.isAiming) {
	        	float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
	        	transform.rotation = Quaternion.Euler(0f, angle, 0f);

                if (!displayWeaponWhenWalking) {
                    weapons[currentWeapon].GetComponent<Renderer>().enabled = characterAiming.isAiming;
                }        
        	}
        	

        	moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        	//controller.Move(moveDir.normalized * speed * Time.deltaTime);

        }

        // interact with object by pressing F 
        handleInteraction();

        // minimap on off

        if (Input.GetKeyDown(KeyCode.M)) {
        	UI.AffectMinimap();
        }


        // weapon reload
        if (Input.GetKeyDown(KeyCode.R) && weapons.Count > 0) {
            weapons[currentWeapon].GetComponent<Weapons>().Reload();
        }

        // check if display weapon
        if (Input.GetKeyDown(KeyCode.T)){
            displayWeaponWhenWalking = !displayWeaponWhenWalking;

            weapons[currentWeapon].GetComponent<Renderer>().enabled = displayWeaponWhenWalking;
        }

        // esc to exit game 
        

        


    	// check mouse scrolling
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0) {
            weapons[currentWeapon].GetComponent<Renderer>().enabled = false;

            if(Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            {
                //wheel goes up
                --currentWeapon;

                if (currentWeapon < 0) {
                    currentWeapon = weapons.Count - 1;
                }
            }
            else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            {
             //wheel goes down

                ++currentWeapon;

                if (currentWeapon >= weapons.Count) {
                    currentWeapon = 0;
                }
            }

            UI.ChangeWeapon();

            if (displayWeaponWhenWalking) {
                weapons[currentWeapon].GetComponent<Renderer>().enabled = true;
            }  

        }	



        // reduce engery overtime, every second coust 1 energy
        currentEnergy -= Time.deltaTime;
        currentEnergy = Mathf.Max(currentEnergy, 0); 
    }


    void handleInteraction() {
        // interact with object by pressing F 

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, targetLayer)) {

            // if (hit.transform != null 
            //     && hit.transform.gameObject.layer == LayerMask.NameToLayer("Interactable")) {

            if (hit.transform != null) {

                GameObject interactable = hit.transform.gameObject;
                bool closeToInteract = interactable.GetComponent<Interactable>().distanceCheck(transform, camTransform);

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

        Vector3 velocity = rb.velocity;
        Vector3 velocityChange;

        if (characterAiming.isAiming) {
        	velocityChange = (moveDir * (speed / 2f) - velocity);
        }
        else if (Input.GetKey(KeyCode.LeftShift) && currentEnergy > 0 && overHeatTimer > 0) {
    		velocityChange = (moveDir * sprinting - velocity);
    		currentEnergy -= Time.fixedDeltaTime * (sprintingCost - 1f);

            overHeatTimer -= Time.fixedDeltaTime * sprintCoolDown; 
    	}
    	else {
    		velocityChange = (moveDir * speed - velocity);
    	}

        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        rb.AddForce(velocityChange, ForceMode.VelocityChange);


        // apply lifing force when pressing space
        if (Input.GetKey(KeyCode.Space) && currentEnergy > 0 && overHeatTimer > 0)
        {   
            // Debug.Log("spaced hited");
            rb.AddForce(Vector3.up * (liftingpSpeed * (1 - (rb.velocity.y / maxLiftingSpeed))), ForceMode.Acceleration);
            currentEnergy -= Time.fixedDeltaTime * (liftingpCost - 1f);

            overHeatTimer -= Time.fixedDeltaTime * liftingCoolDown;   

            liftingParticle.Play();      
        }
        else {
            liftingParticle.Stop();
        }


        if (overHeatTimer < maxOverHeatTime && !Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.LeftShift)) {
            overHeatTimer += Time.fixedDeltaTime;
        }

    }
}
