using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generator : MonoBehaviour {

	/*
	 * as of 10/1/2017, we're not using this
	 * 
	 * it was the very first very quick "look we can randomly generate stuff" test
	 */

	[SerializeField] selector inputs;

	//prefab
	[SerializeField] GameObject floor;
	//loaded from resources
	GameObject[] people;
	GameObject[] trees;
	GameObject[] furnitures;

	[SerializeField] GameObject building; //container for the thing

	//limits
	float minSize, maxSize;
	float minPeopleSize, maxPeopleSize;
	float minFurnitureSize, maxFurnitureSize;
	float minTreeSize, maxTreeSize;

	//the actual objects to keep track of 
	List<List<GameObject>> objects = new List<List<GameObject>>();

	//for random gen parameters
	Dictionary<string, float> paraValues;

	float floorHeight = 3f;

	// Use this for initialization
	void Start () {
		people = Resources.LoadAll<GameObject> ("people");
		trees = Resources.LoadAll<GameObject> ("trees");
		furnitures = Resources.LoadAll<GameObject> ("furniture");

		paraValues = new Dictionary<string, float> ();
		for (int i = 0; i < globalpara.Instance.getNumPara (); i++) {
			paraValues.Add (globalpara.Instance.getParameterName(i), 0);
			objects.Add (new List<GameObject> ());
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			delete ();
			clear ();
			getValues ();
			generate ();
		}
	}

	void getValues(){
		for (int i = 0; i < paraValues.Count; i++) {
			paraValues [globalpara.Instance.getParameterName (i)] = inputs.getValue (i);
		}
	}

	void delete(){
		for (int i = 0; i < objects.Count; i++) {
			for (int j = 0; j < objects [i].Count; j++) {
				Destroy (objects [i] [j]);
			}
		}
	}

	void clear(){
		for (int i = 0; i < objects.Count; i++) {
			objects [i].Clear ();
		}
	}

	void generate(){

		// size  -- - - - - - - - - - - - - - - -- 
		int numFloors = (int)(paraValues["floors"] * 10);
		int numPpl = (int)(paraValues["people"] * 40);
		int numTrees = (int)(paraValues["trees"] * 100);
		int numFurn = (int)(paraValues["furniture"] * 50);

		float sizeX = 1f + paraValues["size"] * 10f;
		float sizeZ = 1f + paraValues["size"] * 10f;

		for (int i = 0; i < numFloors; i++) {
			GameObject f = Instantiate(floor, Vector3.zero, Quaternion.identity);
			f.transform.localScale = new Vector3 (sizeX, 0.1f, sizeZ);
			f.transform.position += new Vector3 (0f, i*floorHeight, 0f);
			f.transform.parent = building.transform;
			objects [1].Add (f);
		}

		for (int i = 0; i < numPpl; i++) {
			GameObject p = Instantiate(people[Random.Range(0,people.Length)], Vector3.zero, Quaternion.identity);
			p.transform.position += new Vector3 (sizeX*0.5f - Random.Range(0f,sizeX), Random.Range(0,numFloors)*floorHeight+1.0f, sizeZ*0.5f - Random.Range(0f,sizeZ));
			p.transform.parent = building.transform;
			objects[2].Add(p);
		}

		for (int i = 0; i < numTrees; i++) {
			GameObject t = Instantiate(trees[Random.Range(0,trees.Length)], Vector3.zero, Quaternion.identity);
			t.transform.position += new Vector3 (sizeX*0.5f -Random.Range(0f,sizeX), Random.Range(0,numFloors)*floorHeight+2.0f, sizeZ*0.5f - Random.Range(0f,sizeZ));
			t.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			t.transform.parent = building.transform;
			objects[3].Add(t);
		}

		for (int i = 0; i < numFurn; i++) {
			GameObject f = Instantiate(furnitures[Random.Range(0,furnitures.Length)], Vector3.zero, Quaternion.identity);
			f.transform.position += new Vector3 (sizeX*0.5f -Random.Range(0f,sizeX), Random.Range(0,numFloors)*floorHeight+1.0f, sizeZ*0.5f - Random.Range(0f,sizeZ));
			f.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			f.transform.parent = building.transform;
			objects[4].Add(f);
		}

	}
}
