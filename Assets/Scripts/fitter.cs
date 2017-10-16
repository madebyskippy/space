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
	int room_size_max=5; //for room size
	int room_size_min=1; //for room size
	int room_height_max=5; //for room size
	int room_height_min=1; //for room size
	int r=5;		//for building
	int c=5;
	int h=10;
	int room_try_max = 150; //how many times it tries to fit a room
	int density = 150;

	[SerializeField] int height_max;
	[SerializeField] int row_max;
	[SerializeField] int col_max;

	//temp controls for adjusting some parameters

	[Range(0.1f, 0.5f)]
	[SerializeField] float floorThickness = 0.3f;
	[Range(-1f, 1f)]
	[SerializeField] float floorOffsetSize = 0f;


	bool[,,] full; //whether or not that grid space is full
	int[] numRooms; //num of rooms on the floor 				-- currently not in use

	List<GameObject> rooms;

	// Use this for initialization
	void Start () {
		initialize ();
	}

	void initialize(){
		full = new bool[r, c, h];
		numRooms = new int[h];
		rooms = new List<GameObject> ();
		for (int i=0; i<r; i++){
			for (int j = 0; j < c; j++) {
				for (int k = 0; k < h; k++) {
					numRooms [k] = 0;
					full [i, j, k] = false;
				}
			}
		}
	}

	public void create(){
		delete ();
		//height
		h = Mathf.Max((int) (height_max * globalpara.Instance.getValue ("height")),1); //mathf.max so it'll never be 0

		//bounds
		r = Mathf.Max((int) (row_max * globalpara.Instance.getValue ("bounds")),1); //mathf.max so it'll never be 0
		c = Mathf.Max((int) (col_max * globalpara.Instance.getValue ("bounds")),1); //mathf.max so it'll never be 0

		//fidelity
		float fidelity = globalpara.Instance.getValue("fidelity");
		room_size_max = Mathf.Max((int) (fidelity * Mathf.Min (r, c)),1);

		//chaos
		//variation in room size/dimensions
		float chaos = globalpara.Instance.getValue("chaos");
		room_height_max = room_height_min + (int) (chaos * height_max);
		room_size_min = Mathf.Max(room_size_max - (int)(room_size_max * chaos),1);

		//density
		density = Mathf.Max((int) (room_try_max * globalpara.Instance.getValue ("density")),1);

//		Debug.Log("height: "+h+"\nbounds, rows&cols: "+r+", "+c+"\n"+"fidelity, max room size: "+room_size_max+
//					"\nchaos, room height min, max: "+room_height_min+","+room_height_max+
//					", min room size: "+room_size_min+
//					"\ndensity: "+density);
		clear ();
		for (int i = 0; i < h; i++) {
			randomPlacement (i);
		}
	}

	//deletes actual game objects
	void delete(){
		for (int i = 0; i < rooms.Count; i++) {
			Destroy (rooms [i].transform.parent.gameObject);
		}
	}

	//clears lists and status lists
	void clear(){
		initialize();
		rooms.Clear ();
//		for (int i = 0; i < r; i++) {
//			for (int j = 0; j < c; j++) {
//				for (int k = 0; k < h; k++) {
//					numRooms [k] = 0;
//					full [i, j, k] = false;
//				}
//			}
//		}
	}

	bool place(int level){
		bool placed = true;
		int sizex = Random.Range (room_size_min, room_size_max+1);
		int sizez = Random.Range (room_size_min, room_size_max+1);
		int sizey = Random.Range (room_height_min, room_height_max);
		int posx = Random.Range (0, r - sizex+1);
		int posz = Random.Range (0, c - sizez+1);

		//checks if this spot is empty. if it isn't, cancel this
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
		t.GetComponent<room> ().Init (new Vector3(sizex,sizez,sizey),(1.0f*level)/(h*1.0f),new Vector2(posx,posz),floorThickness,floorOffsetSize);
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
		for (int i = 0; i < density; i++) {
			place (level);
		}
	}

	public List<GameObject> getRooms(){
		return rooms;
	}
}
