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
        CreateConnections();
	}

	void RebuildNavMesh() {
        navSrf.enabled = false;
        navSrf.enabled = true;
        //navSrf.RemoveData();
        navSrf.BuildNavMesh();

	}


    void CreateConnections() {
        
       for (int i = 0; i < floors.Count; i++){
            float distance = 1000f;
            GameObject closestFloor = null;
            for (int j = 0; j < floors.Count; j++) {
                if (floors[j]!= floors[i]) {
                    float currentDistance = Vector3.Distance(floors[i].transform.position, floors[j].transform.position);
                    if (currentDistance<distance) {
                        distance = currentDistance;
                        closestFloor = floors[j];
                    }
                }
            }
            Debug.Log(closestFloor.transform.position + " is closest to : " + floors[i].transform.position + " with a distance of : " + distance);
            Debug.DrawLine(floors[i].transform.position, closestFloor.transform.position, Color.red);
        }
    }

}