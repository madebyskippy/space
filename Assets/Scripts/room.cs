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

	[SerializeField] GameObject outlinePrefab;

	[SerializeField] GameObject greenPrefab;
	[SerializeField] GameObject peoplePrefab;

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
		GenerateOutlines ();

		GenerateGreens ();
		GeneratePeoples ();
	}
	


	void GenerateGreens(){
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

	void GeneratePeoples(){
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

	void GenerateFurniture(){
	}

	void GenerateOutlines () {
		Mesh mesh = gameObject.GetComponent<MeshFilter> ().mesh;
		List<Vector3> cornerPoints = new List <Vector3> ();
		mesh.GetVertices (cornerPoints);
		for (int j = 0; j < cornerPoints.Count; j++) {
			cornerPoints [j] = gameObject.transform.TransformPoint (cornerPoints [j]);
		}

		//generate floor outlines
		GameObject floorOutlineGO = Instantiate (outlinePrefab,gameObject.transform.position, Quaternion.identity);
		floorOutlineGO.transform.parent = this.transform;
		LineRenderer floorLine = floorOutlineGO.GetComponent<LineRenderer> ();
		floorLine.positionCount = 5;
		floorLine.SetPosition (0, cornerPoints [1]);
		floorLine.SetPosition (1, cornerPoints [7]);
		floorLine.SetPosition (2, cornerPoints [6]);
		floorLine.SetPosition (3, cornerPoints [0]);
		floorLine.SetPosition (4, cornerPoints [1]);

		// generate ceiling outlines
		GameObject ceilingOutlineGO = Instantiate (outlinePrefab, gameObject.transform.position, Quaternion.identity);
		ceilingOutlineGO.transform.parent = this.transform;
		LineRenderer ceilingLine = ceilingOutlineGO.GetComponent<LineRenderer> ();
		ceilingLine.positionCount = 5;
		ceilingLine.SetPosition (0, cornerPoints [3]);
		ceilingLine.SetPosition (1, cornerPoints [5]);
		ceilingLine.SetPosition (2, cornerPoints [4]);
		ceilingLine.SetPosition (3, cornerPoints [2]);
		ceilingLine.SetPosition (4, cornerPoints [3]);

		// generate walls outlines
		GameObject wallOutline1 = Instantiate (outlinePrefab, gameObject.transform.position, Quaternion.identity);
		wallOutline1.transform.parent = this.transform;
		LineRenderer wallLine1 = wallOutline1.GetComponent<LineRenderer> ();
		wallLine1.positionCount = 2;
		wallLine1.SetPosition (0, cornerPoints [7]);
		wallLine1.SetPosition (1, cornerPoints [5]);
		GameObject wallOutline2 = Instantiate (outlinePrefab, gameObject.transform.position, Quaternion.identity);
		wallOutline2.transform.parent = this.transform;
		LineRenderer wallLine2 = wallOutline2.GetComponent<LineRenderer> ();
		wallLine2.positionCount = 2;
		wallLine2.SetPosition (0, cornerPoints [6]);
		wallLine2.SetPosition (1, cornerPoints [4]);
		GameObject wallOutline3 = Instantiate (outlinePrefab, gameObject.transform.position, Quaternion.identity);
		wallOutline3.transform.parent = this.transform;
		LineRenderer wallLine3 = wallOutline3.GetComponent<LineRenderer> ();
		wallLine3.positionCount = 2;
		wallLine3.SetPosition (0, cornerPoints [0]);
		wallLine3.SetPosition (1, cornerPoints [2]);
		GameObject wallOutline4 = Instantiate (outlinePrefab, gameObject.transform.position, Quaternion.identity);
		wallOutline4.transform.parent = this.transform;
		LineRenderer wallLine4 = wallOutline4.GetComponent<LineRenderer> ();
		wallLine4.positionCount = 2;
		wallLine4.SetPosition (0, cornerPoints [1]);
		wallLine4.SetPosition (1, cornerPoints [3]);

	}
}
