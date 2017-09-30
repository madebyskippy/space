using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour {


	[SerializeField] float speed;

	void Update () {
		float inputH1 = Input.GetAxis ("Horizontal");
		float inputV1 = Input.GetAxis ("Vertical");

		transform.RotateAround (gameObject.transform.position, Vector3.up, -inputH1 * speed * Time.deltaTime);
		transform.RotateAround (gameObject.transform.position, gameObject.transform.right, inputV1 * speed * Time.deltaTime);
	}
}
