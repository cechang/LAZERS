using UnityEngine;
using System.Collections;

public class PlayerMover : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if (other.name == "Player") {
			other.transform.parent = gameObject.transform;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.name == "Player") {
			other.transform.parent = null;
		}
	}


}
