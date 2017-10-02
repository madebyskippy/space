using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class room : MonoBehaviour {

	/*
	 * keeps track of stuff and generates stuff for each room.
	 * 
	 * room size
	 * room location
	 * 
	 */

	Vector3 dimensions; //width length height
	float level; //how high it is relative to whole building height. this is a %
	Vector2 location; //the xy of the room on the level

	int greenToCreate;
	int peopleToCreate;

	[SerializeField] GameObject greenPrefab;
	[SerializeField] GameObject peoplePrefab;

	// Use this for initialization
	void Start () {
		
	}

	public void Init(Vector3 d, float l, Vector2 loc){
		dimensions = d;
		level = l;
		location = loc;

		/***	EDIT THESE TO ADJUST HOW MUCH STUFF GENERATES
		 * 		ACCORDING TO THE SPECS OF THE ROOM  
		 * 		right now it's just like a filler example ***/
		greenToCreate = (int)(level * 5); //more trees higher up
		peopleToCreate = (int)(5 - level * 5); //more people lower down

		//generate stuff
		greens ();
		peoples ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void greens(){
		for (int i = 0; i < greenToCreate; i++) {
			GameObject newGreen = Instantiate (greenPrefab);
			newGreen.transform.localScale = Vector3.one * 0.5f; //TEMPORARY
			newGreen.transform.parent = this.transform;
			float posy = -0.5f;
			float posx = Random.Range(-0.5f,0.5f);
			float posz = Random.Range(-0.5f,0.5f);
			newGreen.transform.localPosition = new Vector3(posx,posy,posz);
		}
	}

	void peoples(){
		for (int i = 0; i < peopleToCreate; i++) {
			GameObject newPpl = Instantiate (peoplePrefab);
			newPpl.transform.localScale = Vector3.one * 0.5f; //TEMPORARY
			newPpl.transform.parent = this.transform;
			float posy = -0.5f;
			float posx = Random.Range(-0.5f,0.5f);
			float posz = Random.Range(-0.5f,0.5f);
			newPpl.transform.localPosition = new Vector3(posx,posy,posz);
		}
	}

	void furnitures(){
	}
}
