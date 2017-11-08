using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
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
	}
}
