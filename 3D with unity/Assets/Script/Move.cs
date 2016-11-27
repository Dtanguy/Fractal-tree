using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public float speedH = 2.0f;
	public float speedV = 2.0f;

	private float yaw = 0.0f;
	private float pitch = 0.0f;

	// Update is called once per frame
	void Update () {
		
		float x = Input.GetAxis ("Horizontal");
		float z = Input.GetAxis ("Vertical");
		transform.Translate (new Vector3 (x*5 / 2, 0.0f, z*5 / 2));

		yaw += speedH * Input.GetAxis("Mouse X");
		pitch -= speedV * Input.GetAxis("Mouse Y");

		transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
	}
}
