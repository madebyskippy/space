using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class structure : MonoBehaviour {

	List<GameObject> rooms = new List <GameObject> ();

	[SerializeField] GameObject beam;
	[SerializeField] GameObject floor;
	[Range(0.1f, 0.5f)]
	[SerializeField] float floorThickness;
	[Range(-0.5f, 0.5f)]
	[SerializeField] float floorOffsetSize;

	[SerializeField] GameObject column;

	List<GameObject> floors = new List <GameObject> ();


	void Awake () {
		rooms.AddRange(GameObject.FindGameObjectsWithTag ("Room"));
	}

	public void CreateStructures () {
		GenerateFloors ();
	}

	void GenerateFloors() {
		for (int i = 0; i < rooms.Count; i++) {
			Vector3 buildPos = new Vector3 (
				rooms[i].transform.position.x,
				rooms[i].transform.position.y - rooms[i].transform.position.y/2f + floorThickness/2f,
				rooms[i].transform.position.z
			);
			GameObject newFloor = Instantiate (floor, buildPos, Quaternion.identity);
			floors.Add (newFloor);
			newFloor.transform.localScale = new Vector3 (
				rooms[i].transform.localScale.x + floorOffsetSize,
				floorThickness,
				rooms[i].transform.localScale.z + floorOffsetSize
			);
		}
	}

	public void ClearStructures () {
		floors.ForEach (Destroy);
	}
}
