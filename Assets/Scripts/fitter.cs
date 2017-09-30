using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fitter : MonoBehaviour {

	/*
	 * problems: how to control space density?
	 */

	[SerializeField] GameObject cube;
	[SerializeField] GameObject group; //container

	//eventually parameters that user controls
	int size_max=5;
	int size_min=1;
	int r=5;
	int c=5;
	int h=10;

	bool[,,] full; //whether or not that grid space is full
	int[] numRooms; //num of rooms on the floor 				-- currently not in use

	List<GameObject> rooms;

	// Use this for initialization
	void Start () {
		full = new bool[r, c, h];
		numRooms = new int[h];
		rooms = new List<GameObject> ();
		//so i can see what's happening
		for (int i=0; i<r; i++){
			for (int j = 0; j < c; j++) {
				for (int k = 0; k < h; k++) {
					numRooms [k] = 0;
					full [i, j, k] = false;
				}
			}
		}
		
	}
	
	// Update is called once per frame
	void Update () {


	}

	public void create(){
		delete ();
		clear ();
		for (int i = 0; i < h; i++) {
			randomPlacement (i);
		}
	}

	//deletes actual game objects
	void delete(){
		for (int i = 0; i < rooms.Count; i++) {
			Destroy (rooms [i]);
		}
	}

	//clears lists and status lists
	void clear(){
		rooms.Clear ();
		for (int i = 0; i < r; i++) {
			for (int j = 0; j < c; j++) {
				for (int k = 0; k < h; k++) {
					numRooms [k] = 0;
					full [i, j, k] = false;
				}
			}
		}
	}

	bool place(int level){
		bool placed = true;
		int sizex = Random.Range (size_min, size_max);
		int sizez = Random.Range (size_min, size_max);
		int sizey = Random.Range (size_min, size_max);
		int posx = Random.Range (0, r - sizex+1);
		int posz = Random.Range (0, c - sizez+1);

		for (int i = 0; i < sizex; i++) {
			for (int j = 0; j < sizez; j++) {
				if (full [posx+i, posz+j,level]) {
					placed = false;
//					Debug.Log ("overlap");
					return placed;
				}
			}
		}
			
		GameObject t = Instantiate (cube, new Vector3 (0,0,0), Quaternion.identity);
		GameObject p = new GameObject ();
		p.transform.position = new Vector3 ((-1*r*0.5f)+posx, level, (-1*c*0.5f)+posz);
		t.transform.parent = p.transform;
		t.transform.localPosition = new Vector3 (sizex*0.5f,sizey*0.5f,sizez*0.5f);
		p.transform.parent = group.transform;
		t.transform.localScale = new Vector3 (t.transform.localScale.x*sizex,t.transform.localScale.y*sizey,t.transform.localScale.z*sizez);
		rooms.Add (t);
		int heightmax = sizey;
		if (h-level < sizey){
			heightmax = h-level;
		}
		for (int i = 0; i < sizex; i++) {
			for (int j = 0; j < sizez; j++) {
				for (int k = 0; k < heightmax; k++){
					full [posx+i, posz+j, level+k ] = true;
				}
			}
		}
		numRooms [level]++;
		return placed;
	}

	void randomPlacement(int level){
		for (int i = 0; i < 100; i++) {
			place (level);
		}
	}

	public List<GameObject> getRooms(){
		return rooms;
	}
}
