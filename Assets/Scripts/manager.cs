using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manager : MonoBehaviour {

	[SerializeField] fitter roomfitter;
	[SerializeField] populate populator; 
	[SerializeField] outlines outliner;


	[SerializeField] GameObject building; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			roomfitter.create ();
			populator.setRooms (roomfitter.getRooms ());
		}if (Input.GetKeyDown (KeyCode.P)) {
			populator.Populate (building);
		}if (Input.GetKeyDown (KeyCode.O)) {
			outliner.CreateOutlines ();
		}

		//temp ugly rotating
		building.transform.Rotate (new Vector3 (Input.GetAxis ("Vertical"), Input.GetAxis ("Horizontal"), 0));
	}
}
