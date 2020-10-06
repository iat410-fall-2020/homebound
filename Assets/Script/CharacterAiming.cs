using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAiming : MonoBehaviour
{
	public Transform CameraLookAt;
	public Cinemachine.AxisState xAxis;
	public Cinemachine.AxisState yAxis;

	ThirdPersonController controller;
	Animator animator;

    public bool isAiming = false;
	int isAimingParam = Animator.StringToHash("isAiming");

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);

        CameraLookAt.eulerAngles = new Vector3(yAxis.Value , xAxis.Value, 0);


        //aim
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonUp(1)){
            isAiming = !isAiming;
            animator.SetBool(isAimingParam, isAiming);
        }

        if (isAiming) {
            float targetAngle = controller.cam.GetComponent<Transform>().eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            controller.weaponAttactedPoint.rotation = controller.cam.GetComponent<Transform>().rotation;

            controller.weapons[controller.currentWeapon].GetComponent<Renderer>().enabled = true;

            


            if (controller.weapons[controller.currentWeapon].tag == "Trap" ||
                controller.weapons[controller.currentWeapon].tag == "Lure") {


                Ray ray = controller.cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 15, LayerMask.GetMask("Ground"))) {

                    if (hit.transform != null) {

                        controller.weapons[controller.currentWeapon].GetComponent<PlaceableWeapon>().PlaceReference(hit.point);
                    }
                    
                }
            }


            // Weapon firing
            if (Input.GetMouseButtonDown(0)) {
                if (controller.weapons[controller.currentWeapon] != null) {
                    controller.weapons[controller.currentWeapon].GetComponent<Weapons>().Fire();
                }
            }
        }
    }
}
