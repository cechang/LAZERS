using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Player_Controller : MonoBehaviour {
	
	public float movementSpeed;
	public float mouseSensitivity;
	public float upDownRange = 60;
	public float jumpSpeed;
	public float pushPower = 2.0F;

	private GameObject myLaserGun;

	float verticalVelocity = 0;
	float verticalRotation = 0;
	CharacterController cc;

	// Use this for initialization
	void Start () {
		Screen.lockCursor = true;
		cc = GetComponent<CharacterController> ();
		myLaserGun = GameObject.Find ("LaserGun");
	}
	
	// Update is called once per frame
	void Update () {


		//Looking
		float rotLeftRight = Input.GetAxis ("Mouse X") * mouseSensitivity;
		transform.Rotate (0, rotLeftRight, 0);
		
		verticalRotation -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
		verticalRotation = Mathf.Clamp (verticalRotation, -upDownRange, upDownRange);
		Camera.main.transform.localRotation = Quaternion.Euler (verticalRotation, 0, 0);
		
		//Movement
		float forwardSpeed = Input.GetAxis ("Vertical") * movementSpeed;
		float sideSpeed = Input.GetAxis ("Horizontal") * movementSpeed;
		if (!cc.isGrounded) verticalVelocity += -18 * Time.deltaTime;

		if (cc.isGrounded && Input.GetButtonDown ("Jump")) {
			verticalVelocity = jumpSpeed;
		}

		Vector3 speed = new Vector3 (sideSpeed, verticalVelocity, forwardSpeed);

		speed = transform.rotation * speed;

		cc.Move(speed * Time.deltaTime);


	}
	public void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "PickUp") {
			audio.Play ();
			other.gameObject.SetActive (false);
			myLaserGun.SetActive (true);
		} 
		else if (other.gameObject.tag == "Finish") {
			if (Application.loadedLevel < (Application.levelCount - 1)) {
				Application.LoadLevel (Application.loadedLevel + 1);
			}
		}
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		Rigidbody body = hit.collider.attachedRigidbody;
		if (body == null || body.isKinematic)
			return;
		
		if (hit.moveDirection.y < -0.3F)
			return;
		
		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
		body.velocity = pushDir * pushPower;
	}

}
