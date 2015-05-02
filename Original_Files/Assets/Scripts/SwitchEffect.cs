using UnityEngine;
using System.Collections;

public class SwitchEffect : MonoBehaviour {
	
	public GameObject door;
	public float delayStart;
	
	private Animator animator;
	private bool timing;
	private float delay;
	private int collisions;

	// Use this for initialization
	void Start () {
		animator = door.GetComponent<Animator> ();
		timing = false;
		delay = delayStart;
	}
	
	// Update is called once per frame
	void Update () {
		if (timing) {
			if (delay > 0f) {
				delay -= 1f;
			} 
			else {
				if (collisions == 0) {
					TurnOff();
				}
				timing = false;
				delay = delayStart;
			}
		}
	}

	public void TurnOn() {
		if (!animator.GetBool ("Open")) {
			animator.SetBool ("Open", true);
			audio.Play ();
		}
	}

	public void TurnOff() {
		animator.SetBool ("Open", false);
	}

	public void OnTriggerEnter(Collider other) {
		if (gameObject.tag == "ConstantSwitch") {
			if (other.gameObject.tag == "LaserEnd") {
				if (other.gameObject.activeSelf) {
					collisions++;
				}
			}
		}
	}

	public void OnTriggerStay(Collider other) {
		if (gameObject.tag == "ConstantSwitch") {
			if (other.gameObject.tag == "LaserEnd") {
				if (other.gameObject.activeSelf) {
					TurnOn ();
				}
				else {
					if (collisions == 0) {
						TurnOff ();
					}
				}
			}
		}
	}

	public void OnTriggerExit(Collider other) {
		if (gameObject.tag == "ConstantSwitch") {
			if (other.gameObject.tag == "LaserEnd") {
				collisions--;
				if (!timing) {
					timing = true;
				}
			}
		}
	}
}



