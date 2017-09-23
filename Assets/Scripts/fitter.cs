using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fitter : MonoBehaviour {

	/*
	 * need a class to keep track of the "tetronimos" ? 
	 * -try as rect prism first (randomly generate on the spot)
	 * start from layer 1, place a block, place a block next to it, etc
	 * or randomly try to place a block until you can't anymore
	 */

	[SerializeField] GameObject cube;
	[SerializeField] GameObject group; //container
	[SerializeField] GameObject basegrid; //temp

	//eventually parameters that user controls
	int size_max=5;
	int size_min=1;

	bool[,,] full; //whether or not that grid space is full
	int[] full_floor; //num of empty spaces on the floor

	int r=10;
	int c=10;
	int h=10;

	List<GameObject> rooms;

	// Use this for initialization
	void Start () {
		full = new bool[r, c, h];
		full_floor = new int[h];
		rooms = new List<GameObject> ();
		//so i can see what's happening
		for (int i=0; i<r; i++){
			for (int j = 0; j < c; j++) {
				GameObject t = Instantiate (cube, new Vector3 ((r*-0.5f)+i,-1,(c*-0.5f)+j), Quaternion.identity);
				t.transform.parent = basegrid.transform;
				for (int k = 0; k < h; k++) {
					full_floor [k] = 0;
					full [i, j, k] = false;
				}
			}
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			delete ();
			clear ();
			for (int i = 0; i < 10; i++) {
				randomPlacement (i);
			}
		}

		group.transform.Rotate (new Vector3 (Input.GetAxis ("Vertical"), Input.GetAxis ("Horizontal"), 0));
	}

	void delete(){
		for (int i = 0; i < rooms.Count; i++) {
			Destroy (rooms [i]);
		}
	}
	void clear(){
		rooms.Clear ();
		for (int i = 0; i < r; i++) {
			for (int j = 0; j < c; j++) {
				for (int k = 0; k < h; k++) {
					full_floor [k] = 0;
					full [i, j, k] = false;
				}
			}
		}
	}

	bool step(int level){
		bool placed = true;
		int sizex = Random.Range (size_min, size_max);
		int sizez = Random.Range (size_min, size_max);
		int posx = Random.Range (0, r - sizex+1);
		int posz = Random.Range (0, c - sizez+1);

		for (int i = 0; i < sizex; i++) {
			for (int j = 0; j < sizez; j++) {
				if (full [posx+i, level, posz+j]) {
					placed = false;
					Debug.Log ("overlap");
					return placed;
				}
			}
		}
			
		GameObject t = Instantiate (cube, new Vector3 (0,0,0), Quaternion.identity);
		GameObject p = new GameObject ();
		p.transform.position = new Vector3 (-1 * r*0.5f, 0, -1 * c*0.5f);
		t.transform.parent = p.transform;
		t.transform.localPosition = new Vector3 (sizex * 0.5f-0.5f+posx, level, sizez * 0.5f-0.5f+posz);
		p.transform.parent = group.transform;
		t.transform.localScale = new Vector3 (t.transform.localScale.x*sizex,t.transform.localScale.y,t.transform.localScale.z*sizez);
		rooms.Add (p);
		for (int i = 0; i < sizex; i++) {
			for (int j = 0; j < sizez; j++) {
				full [posx+i, level, posz+j] = true;
				full_floor [level]++;
			}
		}
		Debug.Log (level+": "+full_floor [level]);
		return placed;
	}

	void randomPlacement(int level){
		//		while (full_floor [level] < (r * c) - 1) {
//			step (level);
//		}
		for (int i = 0; i < r * c; i++) {
			step (level);
		}
	}

	void sequentialPlacement(){
	}
}
