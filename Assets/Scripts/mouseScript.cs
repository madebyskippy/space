using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseScript : MonoBehaviour {

	[SerializeField] GameObject allCanvas;

	Vector3 oldMousePos;
	float mouseTimer;
	float mouseStillTime = 4f; //seconds for mouse to be still until stuff fades

	// Use this for initialization
	void Start () {
		mouseTimer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit = new RaycastHit ();
		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit)) {
			GameObject objectHit = hit.transform.gameObject;
			if (objectHit.tag == "person") {
				objectHit.GetComponent<person> ().hover();
			}
		}

		Vector3 mousePos = Input.mousePosition;
		if (mousePos == oldMousePos) {
			mouseTimer += Time.deltaTime;
		} else {
			mouseTimer = 0;
			allCanvas.SetActive (true);
		}
		oldMousePos = mousePos;

		if (mouseTimer > mouseStillTime) {
			allCanvas.SetActive (false);
		}
	}
}
