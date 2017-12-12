﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class room : MonoBehaviour {

	/*
	 * keeps track of stuff and generates stuff for each room.
	 * 
	 */

	Vector3 dimensions; //width length height
	float level; //how high it is relative to whole building height. this is a %
	Vector2 location; //the xy of the room on the level

	// structure parameters

	//thickness
	float globalThickness;

	float floorThickness;
	float columnThickness;
	float beamThickness;
	float wallThickness;
	// disctance
	float globalDistance;

	float columnDistance;

	float floorOffsetSize;

	int greenToCreate;
	int peopleToCreate;
	int furnToCreate;

	List<GameObject> columns = new List <GameObject> ();
	List<GameObject> beams = new List <GameObject> ();

	//prefabs to use - - - - - - - - - -
	//outlines
	[SerializeField] GameObject outlinePrefab;
	//population stuff
	[SerializeField] GameObject greenPrefab;
	[SerializeField] GameObject peoplePrefab;
	[SerializeField] GameObject furnPrefab;
	//structures
	[SerializeField] GameObject beamPrefab;
	[SerializeField] GameObject floorPrefab;
	[SerializeField] GameObject columnPrefab;
	[SerializeField] GameObject wallPrefab;
	[SerializeField] GameObject archPrefab;
	[SerializeField] GameObject archPrefab2;
	//test beams
	[SerializeField] Material mainBeamMat;
	[SerializeField] Material secondBeamMat;

	//elements picked from prefabs to generate in the room - - - - - - - - - -
	GameObject mainBeam;
	GameObject secondBeam;
	GameObject floor;
	GameObject column;
	GameObject wall;

	//actual created elements to pass on to other scripts - - - - - - - - - -
	GameObject myFloor;

	public void Init(Vector3 d, float l, Vector2 loc, float floorT, float floorS){ //temp structure parameters passed on to this function

		dimensions = d;
		level = l;
		location = loc;

		globalThickness = Mathf.Max((globalpara.Instance.getValue (parameters.thickness)),0.1f);
		floorThickness = globalThickness*0.5f;
		columnThickness = globalThickness*0.3f;
		beamThickness = globalThickness*0.3f;
		wallThickness = globalThickness * 0.5f;

		globalDistance = Mathf.Max((globalpara.Instance.getValue (parameters.distance)),0.1f);
		columnDistance = globalDistance;

		floorOffsetSize = floorS;

		/***	EDIT THESE TO ADJUST HOW MUCH STUFF GENERATES
		 * 		ACCORDING TO THE SPECS OF THE ROOM  ***/
		greenToCreate = (int)(level * 5); //more trees higher up
//		peopleToCreate = (int)(5 - level * 5); //more people lower down
		furnToCreate = Random.Range(0,5); //just 0-5 furnitures per room

		//for people
		float roomArea = (float)(d.x*d.y) / 100f; //100 is the maximum room area
		float stabilityinfluence = 1f-l*globalpara.Instance.getBigAverage(parameters.stability);
		peopleToCreate = (int)Random.Range (1f, 100f * roomArea);
		peopleToCreate = (int)Mathf.Max(1f,(int)(peopleToCreate*stabilityinfluence));
//		Debug.Log (roomArea+ "," + stabilityinfluence+","+peopleToCreate);

		//greens
		greenToCreate = (int)Random.Range(1f,50f*(d.z/10f));
		greenToCreate = (int)((float)greenToCreate * l);
		float densityinfluence = 1f-globalpara.Instance.getBigAverage (parameters.density);
		greenToCreate = (int)((float)greenToCreate * densityinfluence);
		Debug.Log (d.z + "," +l+","+ densityinfluence+","+greenToCreate);


		Generate ();

        // NO DELAY
        Fill();
        // DELAY
        //Invoke("Fill", 0.2f);
		

	}

	public JSONObject export(){
		JSONObject data = new JSONObject();
		data ["dimensions x"].AsFloat = dimensions.x;
		data ["dimensions y"].AsFloat = dimensions.y;
		data ["dimensions z"].AsFloat = dimensions.z;
		data ["level"].AsFloat = level;
		data ["location x"].AsFloat = location.x;
		data ["location y"].AsFloat = location.y;
		data ["floor thickness"].AsFloat = floorThickness;
		data ["floor offset size"].AsFloat = floorOffsetSize;
		data ["num greens"].AsInt = greenToCreate;
		data ["num ppl"].AsInt = peopleToCreate;
		data ["num furn"].AsInt = furnToCreate;
		return data;
	}

	public void InitFromSave(Vector3 d, float l, Vector2 loc, float floorT, float floorS, int green, int people, int furn){
		dimensions = d;
		level = l;
		location = loc;

		floorThickness = floorT;
		floorOffsetSize = floorS;

		greenToCreate = green;
		peopleToCreate = people;
		furnToCreate = furn;

		Generate ();
	}

	public void Generate() {

		//generate stuff
		//GenerateOutlines ();

		BuildFloors ();
		BuildWalls ();
//		BuildArchs();
//		BuildColumns ();
		BuildBeams ();
	}

	public void Fill(){
		GenerateGreens ();
		GeneratePeoples ();
		GenerateFurniture ();
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
		floorOutlineGO.transform.parent = this.transform.parent;
		LineRenderer floorLine = floorOutlineGO.GetComponent<LineRenderer> ();
		floorLine.positionCount = 5;
		floorLine.SetPosition (0, cornerPoints [1]);
		floorLine.SetPosition (1, cornerPoints [7]);
		floorLine.SetPosition (2, cornerPoints [6]);
		floorLine.SetPosition (3, cornerPoints [0]);
		floorLine.SetPosition (4, cornerPoints [1]);

		// generate ceiling outlines
		GameObject ceilingOutlineGO = Instantiate (outlinePrefab, gameObject.transform.position, Quaternion.identity);
        ceilingOutlineGO.transform.parent = this.transform.parent;
		LineRenderer ceilingLine = ceilingOutlineGO.GetComponent<LineRenderer> ();
		ceilingLine.positionCount = 5;
		ceilingLine.SetPosition (0, cornerPoints [3]);
		ceilingLine.SetPosition (1, cornerPoints [5]);
		ceilingLine.SetPosition (2, cornerPoints [4]);
		ceilingLine.SetPosition (3, cornerPoints [2]);
		ceilingLine.SetPosition (4, cornerPoints [3]);

		// generate walls outlines
		GameObject wallOutline1 = Instantiate (outlinePrefab, gameObject.transform.position, Quaternion.identity);
        wallOutline1.transform.parent = this.transform.parent;
		LineRenderer wallLine1 = wallOutline1.GetComponent<LineRenderer> ();
		wallLine1.positionCount = 2;
		wallLine1.SetPosition (0, cornerPoints [7]);
		wallLine1.SetPosition (1, cornerPoints [5]);
		GameObject wallOutline2 = Instantiate (outlinePrefab, gameObject.transform.position, Quaternion.identity);
        wallOutline2.transform.parent = this.transform.parent;
		LineRenderer wallLine2 = wallOutline2.GetComponent<LineRenderer> ();
		wallLine2.positionCount = 2;
		wallLine2.SetPosition (0, cornerPoints [6]);
		wallLine2.SetPosition (1, cornerPoints [4]);
		GameObject wallOutline3 = Instantiate (outlinePrefab, gameObject.transform.position, Quaternion.identity);
        wallOutline3.transform.parent = this.transform.parent;
		LineRenderer wallLine3 = wallOutline3.GetComponent<LineRenderer> ();
		wallLine3.positionCount = 2;
		wallLine3.SetPosition (0, cornerPoints [0]);
		wallLine3.SetPosition (1, cornerPoints [2]);
		GameObject wallOutline4 = Instantiate (outlinePrefab, gameObject.transform.position, Quaternion.identity);
        wallOutline4.transform.parent = this.transform.parent;
		LineRenderer wallLine4 = wallOutline4.GetComponent<LineRenderer> ();
		wallLine4.positionCount = 2;
		wallLine4.SetPosition (0, cornerPoints [1]);
		wallLine4.SetPosition (1, cornerPoints [3]);

	}

	void GenerateGreens(){
		for (int i = 0; i < greenToCreate; i++) {
			GameObject newGreen = Instantiate (greenPrefab);
			newGreen.transform.localScale = Vector3.one * 0.5f; //TEMPORARY
            newGreen.transform.parent = this.transform.parent;
			float posy = -0.5f;
			float posx = Random.Range(-0.5f,0.5f);
			float posz = Random.Range(-0.5f,0.5f);
			newGreen.transform.localPosition = new Vector3(posx,posy,posz);
			newGreen.GetComponent<green> ().Init ();
		}
	}

	void GeneratePeoples(){
		for (int i = 0; i < peopleToCreate; i++) {
			GameObject newPpl = Instantiate (peoplePrefab);
			//newPpl.transform.localScale = Vector3.one * 0.5f; //TEMPORARY // TEMPORARY remomved
            newPpl.transform.parent = this.transform.parent;
			float posy = -0.5f;
			float posx = Random.Range(-0.5f,0.5f);
			float posz = Random.Range(-0.5f,0.5f);
			newPpl.transform.localPosition = new Vector3(posx,posy,posz);
			newPpl.GetComponent<person> ().Init ();
		}
	}

	void GenerateFurniture(){
		for (int i = 0; i < furnToCreate; i++) {
			GameObject newFurn = Instantiate (furnPrefab);
			newFurn.transform.localScale = Vector3.one * 0.5f; //TEMPORARY
			newFurn.transform.parent = this.transform.parent;
			float posy = 0.5f;
			float posx = Random.Range(-0.5f,0.5f);
			float posz = Random.Range(-0.5f,0.5f);
			newFurn.transform.localPosition = new Vector3(posx,posy,posz);
			newFurn.GetComponent<furniture> ().Init ();
		}
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
        newFloor.transform.parent = this.transform.parent;
		myFloor = newFloor;
	}


	void BuildArchs() {

		// edges - - - - 
		SubBuildArchEdge(-1f,-1f);
		SubBuildArchEdge(-1f,1f);
		SubBuildArchEdge(1f,-1f);
		SubBuildArchEdge (1f, 1f);

		// archs - - - - -
		SubBuildArch(archPrefab2,-1f,0f);
		SubBuildArch(archPrefab2,1f,0f);
		SubBuildArch(archPrefab,0f,-1f);
		SubBuildArch(archPrefab,0f,1f);

		// tops - - - - - 

		SubBuildArchTop(-1f,0f);
		SubBuildArchTop(1f,0f);
		SubBuildArchTop(0f,-1f);
		SubBuildArchTop(0f,1f);

	}
	void SubBuildArchEdge(float xDir, float zDir) {
		
		Vector3 edgePos = new Vector3 (
			transform.position.x + xDir * (transform.localScale.x * 0.5f),
			transform.position.y,
			transform.position.z + zDir * (transform.localScale.z * 0.5f)
		);
		Vector3 edgeScale = new Vector3 (wallThickness,transform.localScale.y, wallThickness);
		GameObject edge = Instantiate (wallPrefab, edgePos, Quaternion.identity);
		edge.transform.localScale = edgeScale;
		edge.transform.parent = this.transform.parent;
	}
	void SubBuildArch(GameObject prefab, float xDir, float zDir){
		Vector3 archPos = new Vector3 (
			transform.position.x + xDir * ( transform.localScale.x / 2),
			transform.position.y + transform.localScale.y / 3,
			transform.position.z + zDir * ( transform.localScale.z / 2));

		GameObject newArch = Instantiate (prefab, archPos,Quaternion.identity);

		Vector3 archScale;
		if (zDir == 0f) {
			archScale = new Vector3 (wallThickness, transform.localScale.y / 3, transform.localScale.z - wallThickness);
		} else {
			archScale = new Vector3 (transform.localScale.x - wallThickness, transform.localScale.y / 3, wallThickness);
		}
		newArch.transform.localScale = archScale;
		newArch.transform.parent = this.transform.parent;
		
	}
	void SubBuildArchTop(float xDir, float zDir) {
		Vector3 topPos = new Vector3 (
			transform.position.x + xDir * ( transform.localScale.x / 2),
			transform.position.y + (transform.localScale.y / 12f) *6f - transform.localScale.y/ 24f,
			transform.position.z + zDir * ( transform.localScale.z / 2));

		GameObject newTop = Instantiate (wallPrefab, topPos,Quaternion.identity);

		Vector3 topScale;
		if (zDir == 0f) {
			topScale = new Vector3 (wallThickness, transform.localScale.y / 12, transform.localScale.z - wallThickness);
		} else {
			topScale = new Vector3 (transform.localScale.x - wallThickness, transform.localScale.y / 12, wallThickness);
		}
		newTop.transform.localScale = topScale;
		newTop.transform.parent = this.transform.parent;
	}

	void BuildWalls() {

		SubBuildWall (0f, 1f);
		SubBuildWall (0f, -1f);
		SubBuildWall (1f, 0f);
		SubBuildWall (-1f, 0f);

	}

	void SubBuildWall (float xDir, float zDir) {

		Vector3 wallPos = new Vector3 (
			transform.position.x + xDir *( transform.localScale.x * 0.5f),
			transform.position.y,
			transform.position.z + zDir *(transform.localScale.z * 0.5f));

		GameObject newWall = Instantiate (wallPrefab, wallPos,Quaternion.identity);
		Vector3 newScale;

		if (zDir == 0f) {
			newScale = new Vector3 (wallThickness, transform.localScale.y, transform.localScale.z);
		} else {
			newScale = new Vector3 (transform.localScale.x, transform.localScale.y, wallThickness);
		}

		newWall.transform.localScale = newScale;
		newWall.transform.parent = this.transform.parent;
	}

	void BuildColumns () {

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

		// yeah we have for loops again!!! *******

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
            columns [i].transform.parent = this.transform.parent;
		}
	}

	void BuildBeams (){

//		mainBeam = 

		// adjustable parameters for beams
		float beamWidthMain = 0.1f;
		float beamHeightMain = 0.2f;
		float beamWidthSecond = 0.05f;
		float beamHeightSecond = 0.1f;
		float beamLengthOffset = 0f;

		float gridDistanceSecond = columnDistance*0.5f;
		bool squareGrid = false;
		// values depending on parameters
		float beamHeightOffset = beamHeightSecond / 2;
		float beamHeightX;
		float beamWidthX;
		float beamHeightZ;
		float beamWidthZ;
		Material matX;
		Material matZ;
		float beamHeightOffsetX;
		float beamHeightOffsetZ;
		float gridDistanceX;
		float gridDistanceZ;
		int gridDivisionsMain = Mathf.FloorToInt(1f / columnDistance);	// a bit funky, works if our room sizes are full unity unit sizes, no 1.3 or whatever
		int gridDivisionsSecond = Mathf.FloorToInt(1f / gridDistanceSecond);
		columnDistance = 1f / gridDivisionsMain;
		gridDistanceSecond = 1f / gridDivisionsSecond;
		float gridDivisionsX;
		float gridDivisionsZ;
		// check which is main beam axis and change values accordingly (main axis is the longer direction of the room)
		// square grid > no difference between x and z
		if (squareGrid) {
			beamHeightZ = beamHeightMain;
			beamWidthZ = beamWidthMain;
			beamHeightX = beamHeightMain;
			beamWidthX = beamWidthMain;
			beamHeightOffsetZ = 0f;
			beamHeightOffsetX = 0f;
			matZ = mainBeamMat;
			matX = mainBeamMat;
			gridDistanceX = columnDistance;
			gridDistanceZ = columnDistance;
			gridDivisionsX = gridDivisionsMain;
			gridDivisionsZ = gridDivisionsMain;

		} else if (transform.localScale.z > transform.localScale.x) {
			beamHeightZ = beamHeightMain;
			beamWidthZ = beamWidthMain;
			beamHeightX = beamHeightSecond;
			beamWidthX = beamWidthSecond;
			beamHeightOffsetZ = beamHeightOffset;
			beamHeightOffsetX = 0f;
			matZ = mainBeamMat;
			matX = secondBeamMat;
			gridDistanceZ = columnDistance;
			gridDistanceX = gridDistanceSecond;
			gridDivisionsZ = gridDivisionsMain;
			gridDivisionsX = gridDivisionsSecond;
		} else { //x is also main axis if x and z dimensions of room are the same
			beamHeightX = beamHeightMain;
			beamWidthX = beamWidthMain;
			beamHeightZ = beamHeightSecond;
			beamWidthZ = beamWidthSecond;
			beamHeightOffsetX = beamHeightOffset;
			beamHeightOffsetZ = 0f;
			matX = mainBeamMat;
			matZ = secondBeamMat;
			gridDistanceX = columnDistance;
			gridDistanceZ = gridDistanceSecond;
			gridDivisionsX = gridDivisionsMain;
			gridDivisionsZ = gridDivisionsSecond;
		}

//		------> X - axis beams
//		.  .  .
//		------>
//		.  .  .
//		------>

		for (int i = 0; i < gridDivisionsX * transform.localScale.z +1f; i++) {
	
			Vector3 newBuildPosX = new Vector3 (
				                       transform.position.x ,
				                       transform.position.y + transform.localScale.y / 2 - beamHeightX / 2 - beamHeightOffsetX,
				transform.position.z - transform.localScale.z / 2 + gridDistanceX * i);
		
			GameObject newBeamX = Instantiate (beamPrefab, newBuildPosX, Quaternion.identity);
			newBeamX.GetComponent<MeshRenderer> ().material = matX;

			Vector3 newScaleX = new Vector3 (
				                   transform.localScale.x + beamLengthOffset,
				                   beamHeightX,
				                   beamWidthX);
		
			newBeamX.transform.localScale = newScaleX;

            newBeamX.transform.parent = this.transform.parent;
		}

//		|	.	|
//		|	.	|
//		|	.	|
//		|	.	|
//		|	.	|
//		v		v
//		Z - axis beams

		for (int i = 0; i < gridDivisionsZ * transform.localScale.x +1f; i++) {

			Vector3 newBuildPosZ = new Vector3 (
				transform.position.x - transform.localScale.x / 2 + gridDistanceZ * i,
				                       transform.position.y + transform.localScale.y / 2 - beamHeightZ / 2 - beamHeightOffsetZ,
				                       transform.position.z );

			GameObject newBeamZ = Instantiate (beamPrefab, newBuildPosZ, Quaternion.identity);
			newBeamZ.GetComponent<MeshRenderer> ().material = matZ;

			Vector3 newScaleZ = new Vector3 (
				                   beamWidthZ,
				                   beamHeightZ,
				                   transform.localScale.z + beamLengthOffset);

			newBeamZ.transform.localScale = newScaleZ;

            newBeamZ.transform.parent = this.transform.parent;
		}
	}
		
	public GameObject GetFloor() {
		return myFloor;
	}

}
