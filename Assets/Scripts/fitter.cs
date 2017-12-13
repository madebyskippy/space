using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fitter : MonoBehaviour {

	[SerializeField] GameObject cube;
	[SerializeField] GameObject group; //container

	// parameters that user controls
	int room_size_max=5; //for room size
	int room_size_min=1; //for room size
	int room_height_max=5; //for room size
	int room_height_min=2; //for room size
	int r=5;		//for building
	int c=5;
	int h=10;
	int room_try_max = 150; //how many times it tries to fit a room
	int density = 150;

	[SerializeField] int height_max;
	[SerializeField] int row_max;
	[SerializeField] int col_max;

	//temp controls for adjusting some parameters

//	[Range(0.1f, 0.5f)]
//	[SerializeField] float floorThickness = 0.3f;
	float floorThickness = 0.3f; //controlled by slider now
	[Range(-1f, 1f)]
	[SerializeField] float floorOffsetSize = 0f;


	bool[,,] full; //whether or not that grid space is full
	int[] numRooms; //num of rooms on the floor 				-- currently not in use

	List<GameObject> rooms;

	[SerializeField] connections connScript;

	//this is for checking for a certain event
	int peopleOnTop;

	// Use this for initialization
	void Start () {
		initialize ();
	}

	void initialize(){
		peopleOnTop = 0;
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
		//----------buildling para
		//height
		h = Mathf.Max((int) (height_max * globalpara.Instance.getValue (parameters.height)),1); //mathf.max so it'll never be 0

		//bounds
		r = Mathf.Max((int) (row_max * globalpara.Instance.getValue (parameters.width)),1); //mathf.max so it'll never be 0
		c = Mathf.Max((int) (col_max * globalpara.Instance.getValue (parameters.depth)),1); //mathf.max so it'll never be 0

		//fidelity
		float fidelity = globalpara.Instance.getValue(parameters.fidelity);
		room_size_max = Mathf.Max((int) (fidelity * Mathf.Min (r, c)),1);

		//chaos
		//variation in room size/dimensions
		float chaos = globalpara.Instance.getValue(parameters.rooms);
		room_height_max = room_height_min + (int) (chaos * h);
		room_size_min = Mathf.Max(room_size_max - (int)(room_size_max * chaos),1);

		//density
		density = Mathf.Max((int) (room_try_max * globalpara.Instance.getValue (parameters.in_out)),1);

		//---------struct para
		//floor thickness
		floorThickness = globalpara.Instance.getValue(parameters.thickness);

//		Debug.Log("height: "+h+"\nbounds, rows&cols: "+r+", "+c+"\n"+"fidelity, max room size: "+room_size_max+
//					"\nchaos, room height min, max: "+room_height_min+","+room_height_max+
//					", min room size: "+room_size_min+
//					"\ndensity: "+density);

		globalpara.Instance.resetGreen ();
		globalpara.Instance.resetPeople ();

		initialize();
		clear ();

		for (int i = 0; i < h; i++) {
			randomPlacement (i);
		}

        // NO DELAY
        connScript.InitializeConnections();
        // DELAY
        //Invoke("InitConnections", 0.1f);


		if (globalpara.Instance.getPeople () < 5) {
			globalpara.Instance.setState (events.less5people, true);
		}
		if (globalpara.Instance.getGreen () > 25) {
			globalpara.Instance.setState (events.more25trees, true);
		}
		if (peopleOnTop < 1) {
			globalpara.Instance.setState (events.nopeopletop, true);
		}
	}

	//used when loading a building
	//nothing else in this script runs but it still needs a reference to all the rooms
	//(so it can delete them later when you want)
	public void setRoomList(List<GameObject> r){
		rooms = r;
	}

	//deletes actual game objects
	public void delete(){
		for (int i = 0; i < rooms.Count; i++) {
            rooms[i].transform.parent.gameObject.SetActive(false); //first set to inactive because destroying happens too late
			Destroy (rooms [i].transform.parent.gameObject);
		}
	}

	//clears lists and status lists
	public void clear(){
		rooms.Clear ();
	}

	bool place(int level){
		bool placed = true;
		int sizex = Random.Range (room_size_min, room_size_max+1);
		int sizez = Random.Range (room_size_min, room_size_max+1);
		float sizey_distrib = Random.Range (Mathf.Pow(room_height_min,2f), Mathf.Pow(room_height_max,2f));
		int sizey = room_height_min+room_height_max-(int)Mathf.Floor(Mathf.Sqrt (sizey_distrib));
		int posx = Random.Range (0, r - sizex+1);
		int posz = Random.Range (0, c - sizez+1);

		//checks if this spot is empty. if it isn't, cancel this
		for (int i = 0; i < sizex; i++) {
			for (int j = 0; j < sizez; j++) {
				if (full [posx+i, posz+j,level]) {
					placed = false;
					return placed;
				}
			}
		}
			
		GameObject t = Instantiate (cube, new Vector3 (0,0,0), Quaternion.identity);
		GameObject p = new GameObject ();
        p.name = "Room";
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

		if (sizex * sizez > 60) {
			globalpara.Instance.setState (events.largestroom, true);
		}

		return placed;
	}

	void randomPlacement(int level){
		int roomsOnLevel = 0;
		int peopleOnThisLevel = 0;
		bool roomsexist = false;
		for (int i = 0; i < density; i++) {
			bool p = place (level);
			if (p) {
				roomsOnLevel++;
				roomsexist = true;
			}
		}
		peopleOnThisLevel = globalpara.Instance.getPeople ();
		if (roomsexist) {
			//there are rooms on this level
			//and by nature of the for loop, it's the highest level
			//so check the number of ppl on it
			peopleOnTop = peopleOnThisLevel;
		}
		if (roomsOnLevel < 2) {
			globalpara.Instance.setState (events.unevenrooms, true);
		}
	}

	public List<GameObject> getRooms(){
		return rooms;
	}

    // test for later creation of connections
    void InitConnections (){
        connScript.InitializeConnections();
    }
}
