using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bounds : MonoBehaviour {

	/*
	 * as of 10/1/2017, we're not using this
	 * 
	 * this was conway game-of-life-3D attempt to build rooms
	 * 
	 */

	[SerializeField] GameObject cube;
	[SerializeField] GameObject group; //container

	[SerializeField] Text steps;

	int r = 10;
	int c = 10;
	int h = 10;

	GameObject[,,] grid;

	[SerializeField] int bottomlimit = 2;
	[SerializeField] int toplimit = 6;
		
	int stepcount;

	// Use this for initialization
	void Start () {
		stepcount = 0;
		steps.text = "steps: " + stepcount;
		grid = new GameObject[r, c, h];
		for (int i = 0; i < r; i++) {
			for (int j = 0; j < c; j++) {
				for (int k = 0; k < h; k++) {
					GameObject t = Instantiate (cube, new Vector3 ((r*-0.5f)+i,(c*-0.5f)+j,(h*-0.5f)+k), Quaternion.identity);
					t.transform.parent = group.transform;
					grid [i, j, k] = t;
					if (Random.Range (0f, 1f) < 0.8f) {
						grid [i, j, k].SetActive (false);
					}
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			step ();
		}

		group.transform.Rotate (new Vector3 (Input.GetAxis ("Vertical"), Input.GetAxis ("Horizontal"), 0));
		
	}

	void step(){
		stepcount++;
		steps.text = "steps: " + stepcount;
		int count = 0;
		for (int i = 0; i < r; i++) {
			for (int j = 0; j < c; j++) {
				for (int k = 0; k < h; k++) {
					count = 0;
					if (i > 0) {
						count += grid [i - 1, j, k].activeSelf ? 1 : 0; //converts bool to int, 1 for true 0 for false
					} if (i < r - 1) {
						count += grid [i + 1, j, k].activeSelf ? 1 : 0;
					}
					if (j > 0) {
						count += grid [i, j-1, k].activeSelf ? 1 : 0;
					} if (j < c - 1) {
						count += grid [i, j+1, k].activeSelf ? 1 : 0;
					}
					if (k > 0) {
						count += grid [i, j, k-1].activeSelf ? 1 : 0;
					}if (k < h - 1) {
						count += grid [i, j, k+1].activeSelf ? 1 : 0;
					}
					if (count >= toplimit || count <= bottomlimit) {
						//DEAD!!!!!!!!1
						Debug.Log("dead "+count);
						grid[i,j,k].SetActive(false);
					} else {
						//lives
						grid[i,j,k].SetActive(true);
					}
				}
			}
		}
	}
}
