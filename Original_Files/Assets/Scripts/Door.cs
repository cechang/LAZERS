using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	[HideInInspector]
	public bool animationStarted;
	public bool animationFinished;

	// Use this for initialization
	void Start () {
		animationStarted = false;
		animationFinished = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Triggered at the end of the animation
	public void AnimationFinished() {
		animationFinished = true;
	}

	/*  
     At the end of the frame if the animation is finished
     we update the position of the parent to the last position of the child
     and set the position of the child to zero inside the parent.
 	*/
	void LateUpdate() {
		if (animationStarted && animationFinished) {
			animationStarted = false;
			// update the parent position
			transform.parent.position = transform.position;
			// update the door position to zero inside the parent
			transform.localPosition = Vector3.zero;
		}
	}
}
