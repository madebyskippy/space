using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class connections : MonoBehaviour {

	[SerializeField] fitter fitScript;
	List<GameObject> rooms = new List <GameObject> ();
	List<GameObject> floors = new List <GameObject> ();


	public void InitializeConnections() {
		rooms = fitScript.getRooms ();
		for (int i = 0; i < rooms.Count; i++) {
			floors.Add(rooms[i].GetComponent<room>().GetFloor());
		}

	}

	void UpdateNavMesh() {
		for (int i = 0; i < floors.Count; i++) {
//			floors[i].GetComponent<navm
		}
	}

}