using UnityEngine;
using System.Collections;

public class Carry : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown ("Fire2")) {
			PickUp ();
		}
	
	}

	void PickUp() {
		Camera mainCamera = FindCamera ();
	}

	Camera FindCamera() {
		if (camera) {
			return camera;
		} 
		else {
			return Camera.main;
		}
	}
}
