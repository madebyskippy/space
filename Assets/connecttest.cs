using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class connecttest : MonoBehaviour {

	[SerializeField] GameObject[] floors;
	[SerializeField] GameObject bridge;
	[SerializeField] GameObject bridge2;

	// Use this for initialization
	void Start () {
		connect ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			randomizescale ();
			randomizepos ();
			connect ();
		}
	}

	void randomizepos(){
		floors [0].transform.position = new Vector3 (Random.Range (-5f, 5f), 0f, Random.Range (-5f, 5f));
	}
	void randomizescale(){
		floors [0].transform.localScale = new Vector3 (Random.Range (1f, 5f), 1f, Random.Range (1f, 5f));
	}

	void connect(){
		float x1 = floors[0].transform.position.x;
		float x2 = floors[1].transform.position.x;
		float w1 = floors[0].transform.localScale.x*0.5f;
		float w2 = floors[1].transform.localScale.x*0.5f;
		float xdir = 1;
		if (x1 > x2)
			xdir = -1;

		float xedge1 = x1 + w1 * xdir;
		float xedge2 = x2 - w2 * xdir;
		float xscale = Mathf.Abs (xedge2 - xedge1);
		float xpos = xedge1 + xdir * 0.5f * Mathf.Abs (xedge2 - xedge1);

		bridge.transform.position = new Vector3 (xpos,floors [0].transform.position.y, floors [0].transform.position.z);
		bridge.transform.localScale = new Vector3 (xscale, 0.1f, 0.1f);

		float z1 = floors[0].transform.position.z;
		float z2 = floors[1].transform.position.z;
		float d1 = floors[0].transform.localScale.z*0.5f;
		float d2 = floors[1].transform.localScale.z*0.5f;
		float zdir = 1;
		if (z1 > z2)
			zdir = -1;

		float zedge1 = z1 + d1 * zdir;
		float zedge2 = z2 - d2 * zdir;
		float zscale = Mathf.Abs (zedge2 - zedge1);
		float zpos = zedge1 + zdir * 0.5f * Mathf.Abs (zedge2 - zedge1);

		bridge2.transform.position = new Vector3 (floors [1].transform.position.x, floors [0].transform.position.y,zpos);
		bridge2.transform.localScale = new Vector3 (0.1f,0.1f,zscale);
	}
}
