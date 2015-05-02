using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

	public float maxDistance = 100;
	public int maxReflections = 10;
	public GameObject laserEnd;
	public float lerpTime;
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
	private GameObject laserGun;

	// Use this for initialization
	void Start () {
		line = gameObject.GetComponent<LineRenderer> ();
		line.enabled = false;
		laserLight = gameObject.GetComponent<Light> ();
		laserLight.enabled = false;
		Screen.lockCursor = true;
		/* Get particle system and initially clear and stop it */
		particleSystem = GameObject.Find ("LaserParticles").GetComponent<ParticleSystem>();
		/* Used to help with constant switches */
		laserEndObject = Instantiate (laserEnd, new Vector3 (15, -11, 16), Quaternion.identity) as GameObject;
		laserGun = GameObject.Find ("LaserGun"); 
		if (Application.loadedLevelName == "Level1") {
			laserGun.SetActive (false); // initially can't use laser in Level 1
		}
	}
	
	// Update is called once per frame
	void Update () {
		line.enabled = Input.GetButton ("Fire1");
		laserLight.enabled = Input.GetButton ("Fire1");
		if ((Input.GetButton ("Fire1")) || (Input.GetButtonUp ("Fire1"))) {
			DrawLaser ();
			if (particleSystem.isStopped) {
				particleSystem.Play ();
			}
		} 
		else {
			particleSystem.Stop ();
			laserEndObject.transform.position = Vector3.Lerp(laserEndObject.transform.position, new Vector3(0, 0, 0), Time.deltaTime*lerpTime);
		}
	}

	void DrawLaser()
	{
		vertices = 1;
		active = true;
		currot = transform.forward;
		curpos = transform.position;
		line.SetVertexCount(1);
		line.SetPosition(0,transform.position);
		while(active)
		{
			vertices++;
			RaycastHit hit;
			line.SetVertexCount(vertices);
			// Check if the laser hits anything
			if (Physics.Raycast(curpos, currot, out hit, maxDistance))
			{
				curpos=hit.point;
				currot = Vector3.Reflect(currot,hit.normal);
				line.SetPosition(vertices-1,hit.point);
				laserEndObject.transform.position = hit.point;
				/* particles appear at laser end point */
				particleSystem.transform.position = hit.point;
				if (hit.transform.gameObject.tag != "Reflect"){
					active = false;
				}
				if (hit.rigidbody) {
					/* we may want to comment this out depending on our gameplay mechanics; this is for moving around objects with force */
					//hit.rigidbody.AddForceAtPosition(transform.forward * 100, hit.point);
					if (hit.rigidbody.tag == "AnimationSwitch") {
						SwitchEffect switchScript = hit.rigidbody.gameObject.GetComponent<SwitchEffect> ();
						switchScript.TurnOn ();
					}
					else if (hit.rigidbody.tag == "TurretSwitch") {
						SwitchEffect switchScript = hit.rigidbody.gameObject.GetComponent<SwitchEffect>();
						if (!switchScript.door.active) {
							switchScript.door.SetActive(true);
							hit.rigidbody.audio.Play ();
						}
					}
				}
			}
			else
			{
				active = false;
				line.SetPosition(vertices-1,curpos+100*currot);
				laserEndObject.transform.position = curpos+100*currot;
			}
			if (vertices > maxReflections)
			{
				active = false;
			}
		}
	}
}