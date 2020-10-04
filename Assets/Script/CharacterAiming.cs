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
            controller.aim = !controller.aim;
            animator.SetBool(isAimingParam, controller.aim);
        }
    }
}
