using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float walkSpeed = 2;
	public float runSpeed = 6;
	public float gravity =-12;
	public float turnSmoothTime = 0.2f;	
	public float speedSmoothTime = 0.1f;
	public float jumpHeight = 1f;
	[Range(0,1)]
	public float airControl;

	float turnSmoothVelocity;	
	float speedSmoothVelocity;
	float currentSpeed;
	float velocityY;

	Animator animator;
	Transform cameraTransform;
	public bool lockedCursor;
	CharacterController controller;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		cameraTransform = Camera.main.transform;
		controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		LockCursor(lockedCursor);
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		Vector2 inputDir = input.normalized;
		bool running = Input.GetKey(KeyCode.LeftShift);
		Move(inputDir, running);
		Animate(running);
		if(Input.GetKeyDown(KeyCode.Space)){
			Jump();
		}
		//Animator
		/* float animationSpeed = ((running) ? currentSpeed/runSpeed : currentSpeed/walkSpeed * 0.5f);
		animator.SetFloat("Vertical", animationSpeed, speedSmoothTime, Time.deltaTime);
 */
		
	}

	void Move(Vector2 inputDir, bool running){
		if(inputDir != Vector2.zero){
			float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y)* Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
		}
		
		float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
		currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));
		velocityY  += Time.deltaTime * gravity;
		Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

		controller.Move(velocity * Time.deltaTime);
		currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude; 
		if(controller.isGrounded){
			velocityY = 0;
		}
		
	}
	void Animate(bool running){
		float animationSpeed = ((running) ? currentSpeed/runSpeed : currentSpeed/walkSpeed * 0.5f);
		animator.SetFloat("Vertical", animationSpeed, speedSmoothTime, Time.deltaTime);
	}
	public bool LockCursor(bool isLocked){
		if(isLocked){
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			isLocked = true;
			return isLocked;
		}else{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			isLocked = false;
			return isLocked;
		}
	}

	void Jump(){
		if(controller.isGrounded){
			float jumpVelocity = Mathf.Sqrt(-2* gravity * jumpHeight);
			velocityY = jumpVelocity;
		}
	}
	float GetModifiedSmoothTime(float smoothTime){
		if(controller.isGrounded){
			return smoothTime;
		}
		if(airControl == 0){
			return float.MaxValue;
		}
		return smoothTime / airControl;
	}
}
