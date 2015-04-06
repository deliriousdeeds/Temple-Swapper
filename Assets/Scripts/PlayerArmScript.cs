using UnityEngine;
using System.Collections;

public class PlayerArmScript : MonoBehaviour {

	private Animator animController;
	private SpriteRenderer spriteRenderer;
	private RaycastHit2D hit;
	private Ray2D ray;
	private Transform laserRef;
	private LineRenderer lineRenderer;
	private BaseScepter baseScepter;
	private GameObject gauge;

	void Awake()
	{
		animController = transform.parent.GetComponent<Animator> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		lineRenderer = transform.FindChild("Laser").GetComponent<LineRenderer> ();
		laserRef = transform.FindChild ("LaserPointerRef");
		baseScepter = new BaseScepter (transform.parent);
	}

	void Update()
	{
		LookAtMouse ();
		CharacterAnimationController ();
		SwapInputManager ();
	}

	/// <summary>
	/// Control the different inputs conditions for the swap mechanic
	/// </summary>
	private void SwapInputManager(){

		//At mouse down, we create the swap gauge.
		if (Input.GetButtonDown("Fire1")) {
			if (hit && hit.transform.tag == "Pillar") {	//The scepter must be aiming first at the pillar
				CreateSwapGauge();	//Create the Swap Gauge
			}
		}

		//At mouse hold down, charge up the Gauge bar
		if (Input.GetButton("Fire1")) {
			if (gauge && !gauge.GetComponent<GaugeBar> ().hasCompleted && hit && hit.transform.tag == "Pillar") {	//The gauge and the pillar must be highlighted to do so
				
				gauge.GetComponent<GaugeBar> ().ChargeUp();	//Charge up the gauge

				//Check if the gauge is full
				if (gauge && gauge.GetComponent<GaugeBar> ().hasCompleted) {
					//print ("Completed");
					baseScepter.SwapPosition(hit.transform);	//Swap Position
					Destroy (gauge.gameObject); //Destroy the gauge
				}
			}else{	//If the scepter is not highlighting a pillar but that the mouse is held down
				Destroy (gauge.gameObject); // Destroy the gauge : We don't want it to continue where it was left off if we put again focus on the same pillar
			}
		}

		//if the mouse button is released
		if (Input.GetButtonUp("Fire1")) {
			if (gauge && !gauge.GetComponent<GaugeBar> ().hasCompleted && hit && hit.transform.tag == "Pillar") {//Check if the scepter is still aiming the pillar and see if the charge up is completed
				Destroy (gauge.gameObject); // Destroy the gauge
			}
		}
	}

	/// <summary>
	/// Creates the swap gauge.
	/// </summary>
	private void CreateSwapGauge()
	{
		//Spawn the gauge
		gauge = Instantiate(Resources.Load("Gauge_HUD", typeof(GameObject))) as GameObject;
		gauge.GetComponent<GaugeBar> ().SetPosition (Camera.main.WorldToScreenPoint(hit.transform.position));
	}
	

	//Aim the arm toward where the mouse is pointing
	private void LookAtMouse (){
		
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);	//Change the mouse position into World coordinates

		CreateRaycastAndLineRenderer (); //Create the raycast that will point from the scepter

		Quaternion rot = Quaternion.LookRotation (transform.position - mousePosition, Vector3.forward);	// Tell the Quartenion to look at the mouse position

		transform.rotation = rot; // Assign the rotation to the gameobject rotation
	
		transform.eulerAngles = new Vector3 (0,0,transform.eulerAngles.z); //Prevent the angle to go nuts on x and y

	}

	/// <summary>
	/// Creates the raycast and line renderer.
	/// </summary>
	private void CreateRaycastAndLineRenderer(){

		Vector3 laserDirection = laserRef.position - transform.position;	//Determining the laser direction

		//Calculate the Hit from the Raycast
		hit = Physics2D.Raycast(transform.position, laserDirection, Mathf.Infinity, 1 << LayerMask.NameToLayer("Pillar")); 

		//Taking Ray informations
		ray = new Ray2D(transform.position, laserDirection);

		//Set line position
		lineRenderer.SetPosition(0, ray.origin);
		lineRenderer.SetPosition(1, ray.GetPoint(500));
		
		//Set line Width
		lineRenderer.SetWidth(0.1f, 0.1f);


		//if it hits a pillar, then tell the other scripts that the raycast has hit the pillar
		if (hit) {
			//print("Pillar is touched");
			lineRenderer.SetPosition(1,  hit.point);
		}else{
			//print ("Pillar is not touched");
			lineRenderer.SetPosition(1, ray.GetPoint(500));
		}
	}


	//Will display a different sprite depending on where the MC is aiming
	private void CharacterAnimationController()
	{
		float angle = transform.eulerAngles.z; //record the angle

		//If the user aim to the south
		if (angle > 90f && angle < 270f) {
			animController.SetBool ("jumpToFront", true); // Show the front sprite
			spriteRenderer.sortingOrder = 1; //Make the arm sorting layer jump to 1
			lineRenderer.sortingOrder = 0;
		} else {
			animController.SetBool ("jumpToFront", false);// Show the back sprite
			spriteRenderer.sortingOrder = 0; //Make the arm sorting layer jump to 0
			lineRenderer.sortingOrder = -1;
		}
	}
}
