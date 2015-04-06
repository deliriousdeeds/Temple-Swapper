using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public float speed = 80f;
	
	private Rigidbody2D rigidBody2D;
	private Animator animController;
	private float h;
	private float v;

	void Awake()
	{
		rigidBody2D = GetComponent<Rigidbody2D> ();
		animController = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		h = Input.GetAxis ("Horizontal");
		v = Input.GetAxis ("Vertical");

		Move (rigidBody2D, speed, h, v);
		AnimationController (h, v);
	}


	private void Move(Rigidbody2D rb2D, float speed, float xAxis, float yAxis)
	{
		//Managing the horizontal movement
		if (Mathf.Abs(xAxis) > 0) {
			rb2D.AddForce(transform.right * speed * xAxis);
		}
		//Managing the vertical movement
		if (Mathf.Abs (yAxis) > 0) {
			rb2D.AddForce(transform.up * speed * yAxis);

		}
	}

	//Animate the walking animation relative to where the arm is pointing (north or south)
	private void AnimationController(float xAxis, float yAxis)
	{
		if (!animController.GetBool("jumpToFront") && Mathf.Abs(xAxis) >= 0.1f || !animController.GetBool("jumpToFront") && Mathf.Abs(yAxis) >= 0.1f) {
			animController.SetBool("WalkBack", true);
			animController.SetBool("WalkFront", false);
		}else if (!animController.GetBool("jumpToFront") && Mathf.Abs(xAxis) <= 0.1f || !animController.GetBool("jumpToFront") && Mathf.Abs(yAxis) <= 0.1f) {
			animController.SetBool("WalkBack", false);
			animController.SetBool("WalkFront", false);
		}else if (animController.GetBool("jumpToFront") && Mathf.Abs(xAxis) >= 0.1f || animController.GetBool("jumpToFront") && Mathf.Abs(yAxis) >= 0.1f) {
			animController.SetBool("WalkFront", true);
			animController.SetBool("WalkBack", false);
		}else if (animController.GetBool("jumpToFront") && Mathf.Abs(xAxis) <= 0.1f || animController.GetBool("jumpToFront") && Mathf.Abs(yAxis) <= 0.1f) {
			animController.SetBool("WalkFront", false);
			animController.SetBool("WalkBack", false);
		}
	}
}
