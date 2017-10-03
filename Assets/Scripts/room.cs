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

	// structure parameters
	float floorThickness;
	float floorOffsetSize;

	float columnThickness = 0.05f;
	float columnDistance = 1f;

	int greenToCreate;
	int peopleToCreate;

	List<GameObject> columns = new List <GameObject> ();

	//outlines
	[SerializeField] GameObject outlinePrefab;
	//population stuff
	[SerializeField] GameObject greenPrefab;
	[SerializeField] GameObject peoplePrefab;
	//structures
	[SerializeField] GameObject beamPrefab;
	[SerializeField] GameObject floorPrefab;
	[SerializeField] GameObject columnPrefab;



	public void Init(Vector3 d, float l, Vector2 loc, float floorT, float floorS){ //temp structure parameters passed on to this function
		dimensions = d;
		level = l;
		location = loc;

		floorThickness = floorT;
		floorOffsetSize = floorS;

		/***	EDIT THESE TO ADJUST HOW MUCH STUFF GENERATES
		 * 		ACCORDING TO THE SPECS OF THE ROOM  
		 * 		right now it's just like a filler example ***/
		greenToCreate = (int)(level * 5); //more trees higher up
		peopleToCreate = (int)(5 - level * 5); //more people lower down

		//generate stuff
		GenerateOutlines ();

		GenerateGreens ();
		GeneratePeoples ();

		BuildFloors ();
		BuildColumns ();
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

	void BuildFloors() {

		Vector3 buildPos = new Vector3 (
			transform.position.x,
			transform.position.y - transform.localScale.y/2f + floorThickness/2f,
			transform.position.z
		);
		GameObject newFloor = Instantiate (floorPrefab, buildPos, Quaternion.identity);

		newFloor.transform.localScale = new Vector3 (
			transform.localScale.x + floorOffsetSize,
			floorThickness,
			transform.localScale.z + floorOffsetSize
		);
		newFloor.transform.parent = this.transform;

	}

	public void BuildColumns ()
	{

		// create corners ----------------------------

		Vector3 t_buildingBase1 = new Vector3 (transform.position.x - transform.localScale.x / 2, 
			transform.position.y,
			transform.position.z - transform.localScale.z / 2);

		GameObject newColumn1 = Instantiate (columnPrefab, t_buildingBase1, Quaternion.identity);

		Vector3 t_buildingBase2 = new Vector3 (transform.position.x + transform.localScale.x / 2, 
			transform.position.y,
			transform.position.z + transform.localScale.z / 2);

		GameObject newColumn2 = Instantiate (columnPrefab, t_buildingBase2, Quaternion.identity);

		Vector3 t_buildingBase3 = new Vector3 (transform.position.x + transform.localScale.x / 2, 
			transform.position.y,
			transform.position.z - transform.localScale.z / 2);

		GameObject newColumn3 = Instantiate (columnPrefab, t_buildingBase3, Quaternion.identity);

		Vector3 t_buildingBase4 = new Vector3 (transform.position.x - transform.localScale.x / 2, 
			transform.position.y,
			transform.position.z + transform.localScale.z / 2);

		GameObject newColumn4 = Instantiate (columnPrefab, t_buildingBase4, Quaternion.identity);


		columns.Add (newColumn1);
		columns.Add (newColumn2);
		columns.Add (newColumn3);
		columns.Add (newColumn4);


		Vector3 t_buildBase5 = new Vector3 (transform.position.x - transform.localScale.x / 2, 
			transform.position.y,
			transform.position.z);

		Vector3 t_buildBase6 = new Vector3 (transform.position.x + transform.localScale.x / 2, 
			transform.position.y,
			transform.position.z);

		Vector3 t_buildBase7 = new Vector3 (transform.position.x, 
			transform.position.y,
			transform.position.z - transform.localScale.z / 2);

		Vector3 t_buildBase8 = new Vector3 (transform.position.x, 
			transform.position.y,
			transform.position.z + transform.localScale.z / 2);


		// build x rows - - - -- - - - - -- - - - -


		for (int i = 0; i < Mathf.Floor (((transform.localScale.z / 2) - columnDistance / 2) / columnDistance); i++) {

			Vector3 buildPosNeg1;
			Vector3 buildPosPos1;

			Vector3 buildPosNeg2;
			Vector3 buildPosPos2;

			if (i == 0) {

				buildPosNeg1 = new Vector3 (t_buildBase5.x, t_buildBase5.y, t_buildBase5.z - columnDistance / 2);
				buildPosPos1 = new Vector3 (t_buildBase5.x, t_buildBase5.y, t_buildBase5.z + columnDistance / 2);


				buildPosNeg2 = new Vector3 (t_buildBase6.x, t_buildBase6.y, t_buildBase6.z - columnDistance / 2);
				buildPosPos2 = new Vector3 (t_buildBase6.x, t_buildBase6.y, t_buildBase6.z + columnDistance / 2);


			} else {
				buildPosNeg1 = new Vector3 (t_buildBase5.x, t_buildBase5.y, t_buildBase5.z - columnDistance / 2 - columnDistance * i);
				buildPosPos1 = new Vector3 (t_buildBase5.x, t_buildBase5.y, t_buildBase5.z + columnDistance / 2 + columnDistance * i);

				buildPosNeg2 = new Vector3 (t_buildBase6.x, t_buildBase6.y, t_buildBase6.z - columnDistance / 2 - columnDistance * i);
				buildPosPos2 = new Vector3 (t_buildBase6.x, t_buildBase6.y, t_buildBase6.z + columnDistance / 2 + columnDistance * i);
			}

			GameObject newColumnNeg1 = Instantiate (columnPrefab, buildPosNeg1, Quaternion.identity);
			columns.Add (newColumnNeg1);
			GameObject newColumnPos1 = Instantiate (columnPrefab, buildPosPos1, Quaternion.identity);
			columns.Add (newColumnPos1);

			GameObject newColumnNeg2 = Instantiate (columnPrefab, buildPosNeg2, Quaternion.identity);
			columns.Add (newColumnNeg2);
			GameObject newColumnPos2 = Instantiate (columnPrefab, buildPosPos2, Quaternion.identity);
			columns.Add (newColumnPos2);




		}

		// build z rows - - - -- - - - - -- - - - -


		for (int i = 0; i < Mathf.Floor (((transform.localScale.x / 2) - columnDistance / 2) / columnDistance); i++) {

			Vector3 buildPosNeg1;
			Vector3 buildPosPos1;

			Vector3 buildPosNeg2;
			Vector3 buildPosPos2;

			if (i == 0) {

				buildPosNeg1 = new Vector3 (t_buildBase7.x - columnDistance / 2, t_buildBase7.y, t_buildBase7.z);
				buildPosPos1 = new Vector3 (t_buildBase7.x + columnDistance / 2, t_buildBase7.y, t_buildBase7.z);

				buildPosNeg2 = new Vector3 (t_buildBase8.x - columnDistance / 2, t_buildBase8.y, t_buildBase8.z);
				buildPosPos2 = new Vector3 (t_buildBase8.x + columnDistance / 2, t_buildBase8.y, t_buildBase8.z);


			} else {
				buildPosNeg1 = new Vector3 (t_buildBase7.x - columnDistance / 2 - columnDistance * i, t_buildBase7.y, t_buildBase7.z);
				buildPosPos1 = new Vector3 (t_buildBase7.x + columnDistance / 2 + columnDistance * i, t_buildBase7.y, t_buildBase7.z);

				buildPosNeg2 = new Vector3 (t_buildBase8.x - columnDistance / 2 - columnDistance * i, t_buildBase8.y, t_buildBase8.z);
				buildPosPos2 = new Vector3 (t_buildBase8.x + columnDistance / 2 + columnDistance * i, t_buildBase8.y, t_buildBase8.z);
			}

			GameObject newColumnNeg1 = Instantiate (columnPrefab, buildPosNeg1, Quaternion.identity);
			columns.Add (newColumnNeg1);
			GameObject newColumnPos1 = Instantiate (columnPrefab, buildPosPos1, Quaternion.identity);
			columns.Add (newColumnPos1);

			GameObject newColumnNeg2 = Instantiate (columnPrefab, buildPosNeg2, Quaternion.identity);
			columns.Add (newColumnNeg2);
			GameObject newColumnPos2 = Instantiate (columnPrefab, buildPosPos2, Quaternion.identity);
			columns.Add (newColumnPos2);

		}


		// scale columns -------------------------------

		Vector3 t_Scale = new Vector3 (columnThickness, transform.localScale.y, columnThickness);

		for (int i = 0; i < columns.Count; i++) {
			columns [i].transform.localScale = t_Scale;
			columns [i].transform.parent = this.transform;
		}
	}


}
