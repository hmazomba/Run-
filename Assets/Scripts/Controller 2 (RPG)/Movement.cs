using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Movement : MonoBehaviour {
	Animator anim;
	bool isWalking = false;
	public const float WALK_SPEED = .25f;
	public Transform cameraT;
	Rigidbody rb;
	float turnSmoothVelocity;	
	public float turnSmoothTime = 0.4f;
	void Awake(){
		anim = GetComponent<Animator>();
		cameraT = Camera.main.transform;
		rb = GetComponent<Rigidbody>();
	}

	void Update(){
		//turning
		//Turning();
		Walking();

		//jumping
		//walking
		//moveForward

		Move();
		Jump();
		DrawDebugRays();
	}
	void TurnBasedOnCamera(){
		var targetRotation = cameraT.eulerAngles.y;
		transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime); 
	}
	void Turning(){
		anim.SetFloat("Turn", Input.GetAxis("Horizontal"));
	}
	void Walking(){
		if(Input.GetKeyDown(KeyCode.LeftShift)){
			isWalking = !isWalking;
			anim.SetBool("Walk", isWalking);
		}
	}
	void Jump(){
		if(Input.GetKeyDown(KeyCode.Space)){
			anim.SetTrigger("Jump");
		}
	}
	void Move(){
			
		if(anim.GetBool("Walk")){
			anim.SetFloat("Forward", Mathf.Clamp(Input.GetAxis("Vertical"), -WALK_SPEED, WALK_SPEED));
			anim.SetFloat("Turn", Mathf.Clamp(Input.GetAxis("Horizontal"), -WALK_SPEED, WALK_SPEED));	
		}else{
			anim.SetFloat("Forward", Input.GetAxis("Vertical"));
			anim.SetFloat("Turn", Input.GetAxis("Horizontal"));
		}
		if(rb.velocity.magnitude > 0.1f){
			TurnBasedOnCamera();
		}
		
			
	}

	void DrawDebugRays(){
		int layerMask = 1 << 9;
		layerMask = ~layerMask;
		//
		RaycastHit hit;

		Vector3 origin = transform.position;
		origin.y = 1;
		Vector3 dir = transform.forward;
		//shoot out ray in front of player
		if(Physics.Raycast(origin, dir, out hit, 10, layerMask)){
			Debug.DrawRay(origin, dir * hit.distance, Color.red);
		}
	}

	
}
