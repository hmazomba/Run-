using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
	public bool isGrounded;
	public bool isCrouching;
	private float speed;
	private float walkSpeed = 0.05f;
	private float runSpeed = 0.1f;
	private float crouchSpeed=0.025f;
	public float rotSpeed;
	public float jumpHeight;
	Rigidbody rb;
	Animator anim;
	CapsuleCollider colSize;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		colSize = GetComponent<CapsuleCollider>();
		isGrounded = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.LeftControl)){
			if(isCrouching){
				isCrouching = false;
				anim.SetBool("isCrouching", false);
				colSize.height = 2;
				colSize.center= new Vector3(0,1,0);
			}else{
				isCrouching = true;
				anim.SetBool("isCrouching", true);
				colSize.height = 1;
				colSize.center= new Vector3(0,0.5f,0);
			}
		}

		var z = Input.GetAxis("Vertical") * speed;
		var y = Input.GetAxis("Horizontal") * rotSpeed;

		transform.Translate(0, 0, z);
		transform.Rotate(0, y, 0);

		if(Input.GetKey(KeyCode.Space) && isGrounded == true){
			rb.AddForce(0, jumpHeight, 0);
			anim.SetTrigger("isJumping");  
			isCrouching = false;
			isGrounded = false;
		}
		if(isCrouching){
			if(Input.GetKey(KeyCode.W)){
				anim.SetBool("isWalking", true);
				anim.SetBool("isRunning", false);
				anim.SetBool("isIdle", false);
			}else if(Input.GetKey(KeyCode.S)){
				anim.SetBool("isWalking", true);
				anim.SetBool("isRunning", false);
				anim.SetBool("isIdle", false);
			}
			else
			{
				anim.SetBool("isWalking", false);
				anim.SetBool("isRunning", false);
				anim.SetBool("isIdle", true);
			}
		}
		else if(Input.GetKey(KeyCode.LeftShift)){
			speed= runSpeed;
			if(Input.GetKey(KeyCode.W)){
				anim.SetBool("isWalking", false);
				anim.SetBool("isRunning", true);
				anim.SetBool("isIdle", false);
			}else if(Input.GetKey(KeyCode.S)){
				anim.SetBool("isWalking", false);
				anim.SetBool("isRunning", true);
				anim.SetBool("isIdle", false);
			}
			else
			{
				anim.SetBool("isWalking", false);
				anim.SetBool("isRunning", false);
				anim.SetBool("isIdle", true);
			} 
		}
		else if(!isCrouching){
			speed= walkSpeed;
			if(Input.GetKey(KeyCode.W)){
				anim.SetBool("isWalking", true);
				anim.SetBool("isRunning", false);
				anim.SetBool("isIdle", false);
			}else if(Input.GetKey(KeyCode.S)){
				anim.SetBool("isWalking", true);
				anim.SetBool("isRunning", false);
				anim.SetBool("isIdle", false);
			}
			else
			{
				anim.SetBool("isWalking", false);
				anim.SetBool("isRunning", false);
				anim.SetBool("isIdle", true);
			} 
		}
		
	}

	void OnCollisionenter(){
		isGrounded = true;
	}
}
