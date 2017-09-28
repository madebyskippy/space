﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class populate : MonoBehaviour {

	List<GameObject> rooms = new List <GameObject> ();

	List<LineRenderer> roomOutlines = new List <LineRenderer> ();

	[SerializeField] GameObject outlinePrefab;
	[SerializeField] GameObject personPrefab;
	[SerializeField] GameObject greenPrefab;

	[SerializeField] List <Sprite> peopleSprites = new List <Sprite> ();
	[SerializeField] List <Sprite> greenSprites = new List <Sprite> ();

	[Range(0, 10)]
	[SerializeField] int nrPeople;

	[Range(0, 10)]
	[SerializeField] int nrGreen;


	List<GameObject> currentPeople = new List <GameObject> ();
	List<GameObject> currentGreen = new List <GameObject> ();


	void Awake () {
//		rooms.AddRange(GameObject.FindGameObjectsWithTag ("Room"));
	}



	void Update () {
//		if (Input.GetKeyDown(KeyCode.P)) {
//			Outline ();
//			Populate ();
//		}
	}

	public void setRooms(List<GameObject> r){
		rooms = r;
	}

	public void Populate(GameObject parent) {
		Debug.Log ("populating");
		PopPeople (parent);
		PopGreen (parent);
	}

	void PopGreen(GameObject parent) {
		// populate witch green
		for (int i = 0; i < rooms.Count; i++) {
			float sizeModifier = (rooms [i].transform.localScale.x * rooms [i].transform.localScale.y * rooms [i].transform.localScale.z)*0.05f;
			// green to create depending on nr green and size of rooms
			float greenToCreate = nrPeople * sizeModifier;
			for (int j = 0; j < greenToCreate; j++) {
				Vector3 newPos = new Vector3 (
					rooms [i].transform.position.x + Random.Range (-rooms [i].transform.localScale.x / 2, rooms [i].transform.localScale.x / 2),
					rooms [i].transform.position.y -  rooms[i].transform.localScale.y/2 + 1,
					rooms [i].transform.position.z + Random.Range (-rooms [i].transform.localScale.z / 2, rooms [i].transform.localScale.z / 2)
				);
				GameObject newGreen = Instantiate (greenPrefab, newPos, Quaternion.identity);
				newGreen.GetComponent<SpriteRenderer> ().sprite = greenSprites [Random.Range (0, greenSprites.Count)];
				newGreen.transform.parent = parent.transform;
				currentGreen.Add (newGreen);

			}
		}
	}

	void PopPeople(GameObject parent) {
		// populate witch people
		for (int i = 0; i < rooms.Count; i++) {
			float sizeModifier = (rooms [i].transform.localScale.x * rooms [i].transform.localScale.y * rooms [i].transform.localScale.z)*0.05f;
			// people to create depending on nr people and size of rooms
			float peopleToCreate = nrPeople * sizeModifier;
			for (int j = 0; j < peopleToCreate; j++) {
				Vector3 newPos = new Vector3 (
					rooms [i].transform.position.x + Random.Range (-rooms [i].transform.localScale.x / 2, rooms [i].transform.localScale.x / 2),
					rooms [i].transform.position.y -  rooms[i].transform.localScale.y/2 + 1,
					rooms [i].transform.position.z + Random.Range (-rooms [i].transform.localScale.z / 2, rooms [i].transform.localScale.z / 2)
				);
				GameObject newPerson = Instantiate (personPrefab, newPos, Quaternion.identity);
				newPerson.GetComponent<SpriteRenderer> ().sprite = peopleSprites [Random.Range (0, peopleSprites.Count)];
				newPerson.transform.parent = parent.transform;
				currentPeople.Add (newPerson);
					
			}
		}
	}

	// create line renderer outlines for rooms
	public void Outline () {
		for (int i = 0; i < rooms.Count; i++) {
			// get vertex info of room meshs
			Mesh mesh = rooms[i].GetComponent<MeshFilter> ().mesh;
			List<Vector3> cornerPoints = new List <Vector3> ();
			mesh.GetVertices (cornerPoints);
			for (int j = 0; j < cornerPoints.Count; j++) {
				cornerPoints [j] = rooms[i].transform.TransformPoint (cornerPoints [j]);
			}

			//generate floor outlines
			GameObject floorOutlineGO = Instantiate(outlinePrefab, rooms[i].transform.position, Quaternion.identity);
			LineRenderer floorLine = floorOutlineGO.GetComponent<LineRenderer> ();
			roomOutlines.Add (floorLine);
			floorLine.positionCount = 5;
			floorLine.SetPosition (0,cornerPoints[1]);
			floorLine.SetPosition (1,cornerPoints[7]);
			floorLine.SetPosition (2,cornerPoints[6]);
			floorLine.SetPosition (3,cornerPoints[0]);
			floorLine.SetPosition (4,cornerPoints[1]);

			// generate ceiling outlines
			GameObject ceilingOutlineGO = Instantiate(outlinePrefab, rooms[i].transform.position, Quaternion.identity);
			LineRenderer ceilingLine = ceilingOutlineGO.GetComponent<LineRenderer> ();
			roomOutlines.Add (ceilingLine);
			ceilingLine.positionCount = 5;
			ceilingLine.SetPosition (0,cornerPoints[3]);
			ceilingLine.SetPosition (1,cornerPoints[5]);
			ceilingLine.SetPosition (2,cornerPoints[4]);
			ceilingLine.SetPosition (3,cornerPoints[2]);
			ceilingLine.SetPosition (4,cornerPoints[3]);

			// generate walls outlines
			GameObject wallOutline1 = Instantiate(outlinePrefab, rooms[i].transform.position, Quaternion.identity);
			LineRenderer wallLine1 = wallOutline1.GetComponent<LineRenderer> ();
			roomOutlines.Add (wallLine1);
			wallLine1.positionCount = 2;
			wallLine1.SetPosition (0,cornerPoints[7]);
			wallLine1.SetPosition (1,cornerPoints[5]);
			GameObject wallOutline2 = Instantiate(outlinePrefab, rooms[i].transform.position, Quaternion.identity);
			LineRenderer wallLine2 = wallOutline2.GetComponent<LineRenderer> ();
			roomOutlines.Add (wallLine2);
			wallLine2.positionCount = 2;
			wallLine2.SetPosition (0,cornerPoints[6]);
			wallLine2.SetPosition (1,cornerPoints[4]);
			GameObject wallOutline3 = Instantiate(outlinePrefab, rooms[i].transform.position, Quaternion.identity);
			LineRenderer wallLine3 = wallOutline3.GetComponent<LineRenderer> ();
			roomOutlines.Add (wallLine3);
			wallLine3.positionCount = 2;
			wallLine3.SetPosition (0,cornerPoints[0]);
			wallLine3.SetPosition (1,cornerPoints[2]);
			GameObject wallOutline4 = Instantiate(outlinePrefab, rooms[i].transform.position, Quaternion.identity);
			LineRenderer wallLine4 = wallOutline4.GetComponent<LineRenderer> ();
			roomOutlines.Add (wallLine4);
			wallLine4.positionCount = 2;
			wallLine4.SetPosition (0,cornerPoints[1]);
			wallLine4.SetPosition (1,cornerPoints[3]);




		}
	}
}
	