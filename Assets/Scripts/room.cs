using System.Collections;
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
	float floorThickness;
	float floorOffsetSize;

	float columnThickness = 0.1f;
	float columnDistance = 1f;

	int greenToCreate;
	int peopleToCreate;

	List<GameObject> columns = new List <GameObject> ();
	List<GameObject> beams = new List <GameObject> ();

	//prefabs to use - - - - - - - - - -
	//outlines
	[SerializeField] GameObject outlinePrefab;
	//population stuff
	[SerializeField] GameObject greenPrefab;
	[SerializeField] GameObject peoplePrefab;
	//structures
	[SerializeField] GameObject beamPrefab;
	[SerializeField] GameObject floorPrefab;
	[SerializeField] GameObject columnPrefab;
	[SerializeField] GameObject wallPrefab;
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

		floorThickness = floorT;
		floorOffsetSize = floorS;

		/***	EDIT THESE TO ADJUST HOW MUCH STUFF GENERATES
		 * 		ACCORDING TO THE SPECS OF THE ROOM  
		 * 		right now it's just like a filler example ***/
		greenToCreate = (int)(level * 5); //more trees higher up
		peopleToCreate = (int)(5 - level * 5); //more people lower down

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
		return data;
	}

	public void InitFromSave(Vector3 d, float l, Vector2 loc, float floorT, float floorS, int green, int people){
		dimensions = d;
		level = l;
		location = loc;

		floorThickness = floorT;
		floorOffsetSize = floorS;

		greenToCreate = green;
		peopleToCreate = people;

		Generate ();
	}

	public void Generate() {

		//generate stuff
		//GenerateOutlines ();

		BuildFloors ();
//		BuildWalls ();
		BuildColumns ();
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

	void BuildWalls() {

		// adjustable parameters
		float wallThickness = 0.1f;

		
//	-	. X		
//	|		.
//	+	.
// 	Z

		Vector3 newPos1 = new Vector3 (
			transform.position.x - transform.localScale.x / 2,
			transform.position.y,
			transform.position.z);
			
		GameObject newWall1 = Instantiate (wallPrefab, newPos1,Quaternion.identity);

		Vector3 newScale1 = new Vector3 (wallThickness, transform.localScale.y, transform.localScale.z);

		newWall1.transform.localScale = newScale1;
        newWall1.transform.parent = this.transform.parent;


//		.	- X	
//	.		|
//		.	+
// 	Z

		Vector3 newPos2 = new Vector3 (
			transform.position.x + transform.localScale.x / 2,
			transform.position.y,
			transform.position.z);

		GameObject newWall2 = Instantiate (wallPrefab, newPos2,Quaternion.identity);

		Vector3 newScale2 = new Vector3 (wallThickness, transform.localScale.y, transform.localScale.z);

		newWall2.transform.localScale = newScale2;
        newWall2.transform.parent = this.transform.parent;

//	-	_	+ X
//	.		.
//		.
// Z
	
		Vector3 newPos3 = new Vector3 (
			transform.position.x,
			transform.position.y,
			transform.position.z - transform.localScale.z / 2);

		GameObject newWall3 = Instantiate (wallPrefab, newPos3,Quaternion.identity);

		Vector3 newScale3 = new Vector3 (transform.localScale.x, transform.localScale.y, wallThickness);

		newWall3.transform.localScale = newScale3;
        newWall3.transform.parent = this.transform.parent;

//		.	  X
//	.		.
//	-	_	+ 
//  Z

		Vector3 newPos4 = new Vector3 (
			transform.position.x,
			transform.position.y,
			transform.position.z + transform.localScale.z / 2);

		GameObject newWall4 = Instantiate (wallPrefab, newPos4,Quaternion.identity);

		Vector3 newScale4 = new Vector3 (transform.localScale.x, transform.localScale.y, wallThickness);

		newWall4.transform.localScale = newScale4;
        newWall4.transform.parent = this.transform.parent;


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
		float gridDistanceMain = 0.5f;
		float gridDistanceSecond = 0.3f;
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
		int gridDivisionsMain = Mathf.FloorToInt(1f / gridDistanceMain);	// a bit funky, works if our room sizes are full unity unit sizes, no 1.3 or whatever
		int gridDivisionsSecond = Mathf.FloorToInt(1f / gridDistanceSecond);
		gridDistanceMain = 1f / gridDivisionsMain;
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
			gridDistanceX = gridDistanceMain;
			gridDistanceZ = gridDistanceMain;
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
			gridDistanceZ = gridDistanceMain;
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
			gridDistanceX = gridDistanceMain;
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
