using UnityEngine;
using System.Collections;

public class ConstantLaser : MonoBehaviour {
	
	public float maxDistance = 100;
	public int maxReflections = 10;
	public GameObject laserEnd;
	public GameObject particles;
	[HideInInspector]
	public RaycastHit hit;
	
	private LineRenderer line;
	private Light laserLight;
	private ParticleSystem particleSystem;
	private int vertices = 1; //segment handler don't touch.
	private bool active;
	private Vector3 currot;
	private Vector3 curpos;
	private GameObject laserEndObject;
	
	// Use this for initialization
	void Start () {
		line = gameObject.GetComponent<LineRenderer> ();
		line.enabled = true;
		laserLight = gameObject.GetComponent<Light> ();
		laserLight.enabled = true;
		/* Get particle system and initially clear and stop it */
		particleSystem = particles.GetComponent<ParticleSystem>();
		if (gameObject.tag == "Off") {
			gameObject.SetActive (false);
		} else if (gameObject.tag == "On") {
			gameObject.SetActive (true);
		}
		/* Used to help with constant switches */
		laserEndObject = Instantiate (laserEnd, new Vector3 (15, -11, 16), Quaternion.identity) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		DrawLaser();
	}
	
	void DrawLaser()
	{
		vertices = 1;
		active = true;
		currot = transform.forward;
		curpos = transform.position;
		line.SetVertexCount (1);
		line.SetPosition (0, transform.position);
		while (active) {
			vertices++;
			RaycastHit hit;
			line.SetVertexCount (vertices);
			// Check if the laser hits anything
			if (Physics.Raycast (curpos, currot, out hit, maxDistance)) {
				curpos = hit.point;
				currot = Vector3.Reflect (currot, hit.normal);
				line.SetPosition (vertices - 1, hit.point);
				laserEndObject.transform.position = hit.point;
				/* particles appear at laser end point */
				particleSystem.transform.position = hit.point;
				if (hit.transform.gameObject.tag != "Reflect") {
					active = false;
				}
				if (hit.rigidbody) {
					/* we may want to comment this out depending on our gameplay mechanics; this is for moving around objects with force */
					//hit.rigidbody.AddForceAtPosition(transform.forward * 100, hit.point);
					if (hit.rigidbody.tag == "AnimationSwitch") {
						SwitchEffect switchScript = hit.rigidbody.gameObject.GetComponent<SwitchEffect> ();
						switchScript.TurnOn ();
					} else if (hit.rigidbody.tag == "TurretSwitch") {
						SwitchEffect switchScript = hit.rigidbody.gameObject.GetComponent<SwitchEffect> ();
						switchScript.door.SetActive (true);
						hit.rigidbody.audio.Play ();
					}
				}
			} else {
				active = false;
				line.SetPosition (vertices - 1, curpos + 100 * currot);
				laserEndObject.transform.position = curpos + 100 * currot;
			}
			if (vertices > maxReflections) {
				active = false;
			}
		}
	}
}
