using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleTransition : MonoBehaviour, ISelectHandler {

	public Selectable selectable;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		if (Input.anyKeyDown) {
//			if (Application.loadedLevel < (Application.levelCount - 1)) {
//				Application.LoadLevel (Application.loadedLevel + 1);
//			}
//		}
	}

	public void OnSelect(BaseEventData eventData) {
		switch(selectable.gameObject.name) {
			case ("Start"):
				Application.LoadLevel (Application.loadedLevel + 1);
				break;
			case ("Quit"):
				Application.Quit ();
				break;
		}
	}

}
