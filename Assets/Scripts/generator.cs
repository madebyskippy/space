using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generator : MonoBehaviour {

	[SerializeField] GameObject floor;
	[SerializeField] GameObject building; //container

	//limits
	float minSize, maxSize;
	float minPeopleSize, maxPeopleSize;
	float minFurnitureSize, maxFurnitureSize;
	float minTreeSize, maxTreeSize;

	//keep track of 
	List<GameObject> floors = new List <GameObject> ();
	List <GameObject> peopleList = new List <GameObject> ();
	List <GameObject> treesList = new List <GameObject> ();
//	List <GameObject> roofStuffList = new List<GameObject> ();
	List <GameObject> furnitureList = new List <GameObject> ();

	//for random gen parameters
	[SerializeField] Dictionary<string, float> paraValues;

	//loaded from resources
	GameObject[] people;
	GameObject[] trees;
//	GameObject[] roofStuff;
	GameObject[] furnitures;

	float floorHeight = 3f;

	// Use this for initialization
	void Start () {
		people = Resources.LoadAll<GameObject> ("people");
		trees = Resources.LoadAll<GameObject> ("trees");
//		roofStuff = Resources.LoadAll<GameObject> ("roofStuff");
		furnitures = Resources.LoadAll<GameObject> ("furniture");

//		parameters = new Dictionary<string, float> ();
//		parameters.Add ("floors", 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			delete ();
			clear ();
			generate ();
		}
	}

	void delete(){
		foreach (GameObject g in floors) {
			Destroy (g);
		}
		foreach (GameObject g in peopleList) {
			Destroy (g);
		}
		foreach (GameObject g in treesList) {
			Destroy (g);
		}
		foreach (GameObject g in furnitureList) {
			Destroy (g);
		}
	}

	void clear(){
		floors.Clear ();
		peopleList.Clear ();
		treesList.Clear ();
		furnitureList.Clear ();
	}

	void generate(){

		// size  -- - - - - - - - - - - - - - - -- 
		int numFloors = 5;
		int numPpl = 10;
		int numTrees = 10;
		int numFurn = 10;

		float sizeX = 7.5f;
		float sizeZ = 7.5f;

		for (int i = 0; i < numFloors; i++) {
			GameObject f = Instantiate(floor, Vector3.zero, Quaternion.identity);
			f.transform.localScale = new Vector3 (sizeX, 0.1f, sizeZ);
			f.transform.position += new Vector3 (0f, i*floorHeight, 0f);
			f.transform.parent = building.transform;
			floors.Add (f);
		}

		for (int i = 0; i < numPpl; i++) {
			GameObject p = Instantiate(people[Random.Range(0,people.Length)], Vector3.zero, Quaternion.identity);
			p.transform.position += new Vector3 (sizeX*0.5f - Random.Range(0f,sizeX), Random.Range(0,numFloors)*floorHeight+1.0f, sizeZ*0.5f - Random.Range(0f,sizeZ));
			p.transform.parent = building.transform;
			peopleList.Add (p);
		}

		for (int i = 0; i < numTrees; i++) {
			GameObject t = Instantiate(trees[Random.Range(0,trees.Length)], Vector3.zero, Quaternion.identity);
			t.transform.position += new Vector3 (sizeX*0.5f -Random.Range(0f,sizeX), Random.Range(0,numFloors)*floorHeight+2.0f, sizeZ*0.5f - Random.Range(0f,sizeZ));
			t.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			t.transform.parent = building.transform;
			treesList.Add (t);
		}

		for (int i = 0; i < numFurn; i++) {
			GameObject f = Instantiate(furnitures[Random.Range(0,furnitures.Length)], Vector3.zero, Quaternion.identity);
			f.transform.position += new Vector3 (sizeX*0.5f -Random.Range(0f,sizeX), Random.Range(0,numFloors)*floorHeight+1.0f, sizeZ*0.5f - Random.Range(0f,sizeZ));
			f.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			f.transform.parent = building.transform;
			furnitureList.Add (f);
		}

	}
}
