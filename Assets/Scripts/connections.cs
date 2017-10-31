using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

public class connections : MonoBehaviour {

	[SerializeField] fitter fitScript;
	List<GameObject> rooms = new List <GameObject> ();
	List<GameObject> floors = new List <GameObject> ();
    NavMeshSurface navSrf;

    private void Awake() {
        navSrf = GetComponent<NavMeshSurface>();
    }

    public void InitializeConnections() {
		rooms = fitScript.getRooms ();
		for (int i = 0; i < rooms.Count; i++) {
			floors.Add(rooms[i].GetComponent<room>().GetFloor());
		}

        RebuildNavMesh();
	}

	void RebuildNavMesh() {
        navSrf.enabled = false;
        navSrf.enabled = true;
        //navSrf.RemoveData();
        navSrf.BuildNavMesh();



	}

}