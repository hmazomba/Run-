
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionControl : MonoBehaviour {
	public float hor;
	public float ver;
	public Vector3 cameraHorizontalForward;
	public Transform cameraTransform;
	public Vector3 desiredMoveDirection;
	public float direction;
	public float speed;
	public float runSpeed = 1.1f;
	public float walkSpeed = 0.2f;
	public Animator anim;
	public bool running;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		hor = Input.GetAxis("Horizontal");
		ver = Input.GetAxis("Vertical");
		cameraHorizontalForward = new Vector3(cameraTransform.forward.x, 0f, cameraTransform.forward.z).normalized;
		desiredMoveDirection = ver * cameraHorizontalForward + hor * cameraTransform.right;
		direction = Vector3.Angle(transform.forward, desiredMoveDirection) * Mathf.Sign(Vector3.Dot(desiredMoveDirection, transform.right));
		speed = desiredMoveDirection.magnitude;
		anim.SetFloat("Speed", speed, 0.2f, Time.deltaTime);
		anim.SetFloat("Direction", direction, 0.2f, Time.deltaTime);
		
		
		

	}
}
