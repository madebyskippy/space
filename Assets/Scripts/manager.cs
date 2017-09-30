using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manager : MonoBehaviour {

	[SerializeField] fitter roomfitter;
	[SerializeField] populate populator; 
	[SerializeField] outlines outliner;
	[SerializeField] structure structurer;


	[SerializeField] GameObject building; 


	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			roomfitter.create ();
			populator.setRooms (roomfitter.getRooms ());
		}if (Input.GetKeyDown (KeyCode.P)) {
			populator.Populate (building);
		}if (Input.GetKeyDown (KeyCode.O)) {
			outliner.CreateOutlines ();
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			structurer.CreateStructures ();
		}

		// temp function to delete everything
		if (Input.GetKeyDown (KeyCode.C)) {
			ClearEverything ();
		}

		//temp ugly rotating
		building.transform.Rotate (new Vector3 (Input.GetAxis ("Vertical"), Input.GetAxis ("Horizontal"), 0));
	}

	void ClearEverything () {
		outliner.ClearOutlines ();
		structurer.ClearStructures ();
	}
}
