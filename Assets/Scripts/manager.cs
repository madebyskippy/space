using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manager : MonoBehaviour {

	[SerializeField] fitter roomfitter;



	[SerializeField] GameObject building; 


	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			roomfitter.create ();

		}
			
	}


}
