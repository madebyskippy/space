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

	[SerializeField] GameObject bridgePrefab;

    private void Awake() {
        navSrf = GetComponent<NavMeshSurface>();
    }

    public void InitializeConnections() {
		rooms = fitScript.getRooms ();
        floors.Clear();
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
            float distance = 10000f;
            GameObject closestFloor = null;
            for (int j = 0; j < floors.Count; j++) {
                float currentDistance = Vector3.Distance(floors[i].transform.position, floors[j].transform.position);

                if (currentDistance < distance && currentDistance > 0f) {
                        distance = currentDistance;
                        closestFloor = floors[j];
                   }
            }

			float bridgeOffset = 0.5f;
			bridgeOffset = 0f;
			float bridgeWidth = 0.3f;


			Vector3 floorPos = floors[i].transform.position;
			Vector3 targetPos = closestFloor.transform.position;

			Vector3 bridgeXPos = new Vector3 (0,0,0);
			Vector3 bridgeZPos = new Vector3 (0,0,0);

			bool bridgeXneeded = false;
			bool bridgeZneeded = false;

			bool targetRight = false;
			bool targetLeft = false;
			bool targetFront = false;
			bool targetBehind = false;

			float bridgeXX = 0;
			float bridgeXZ = 0;
			float bridgeZX = 0;
			float bridgeZZ = 0;

			// check in which direction the target room is located and if a connection is needed on the corresbonding x or z axis

			float x1 = floorPos.x;
			float x2 = targetPos.x;
			float w1 = floors [i].transform.localScale.x / 2f;
			float w2 = closestFloor.transform.localScale.x / 2f;

			float z1 = floorPos.z;
			float z2 = targetPos.z;
			float d1 = floors [i].transform.localScale.z / 2f;
			float d2 = closestFloor.transform.localScale.z / 2f;


			Vector3 intersectionPoint; 

			int rnd = Random.Range (0, 2);
			if (rnd < 1) {
				intersectionPoint = new Vector3 (x1, floorPos.y, z1);
			} else {
				intersectionPoint = new Vector3 (x2, floorPos.y, z2);
			}
				


			if (floorPos.x < targetPos.x) { // target is right of - - -
				Debug.Log("IT'S TO THE RIGHT");
				if ((x2-w2 + bridgeOffset) > (x1+w1 - bridgeOffset)) { // bridge x is needed (not overlapping)
					Debug.Log ("need the X");
					targetRight = true;
					bridgeXPos = new Vector3 (x1 - w1 - 0.5f*(x1-x2-w1-w2),floorPos.y, floorPos.z);
//					bridgeXX = Vector3.Distance (floorPos + new Vector3 (floors[i].transform.localScale.x/2,0,0),
//						targetPos - new Vector3 (closestFloor.transform.localScale.x/2, 0, 0));
					bridgeXX = x2-w2-x1-w1;
					bridgeXZ = bridgeWidth;
					if (bridgeXX > 0) {
						Debug.Log ("REALLY need the X");
						bridgeXneeded = true;	
					}

				}
			} else if (floorPos.x > targetPos.x) { // target is left of - - -
				Debug.Log("IT'S TO THE LEFT");
				if ((targetPos.x + closestFloor.transform.localScale.x / 2 - bridgeOffset) < // bridge x is needed (not overlapping)
					(floorPos.x - floors[i].transform.localScale.x / 2 + bridgeOffset)) {
					Debug.Log ("need the X");
					targetLeft = true;
					bridgeXPos = new Vector3 (x2 - w2 - 0.5f*(x2-x1-w1-w2),floorPos.y, floorPos.z);
					bridgeXX = x1-w1-x2-w2;
					bridgeXZ = bridgeWidth;
					if (bridgeXX > 0) {
						Debug.Log ("REALLY need the X");
						bridgeXneeded = true;
					}
				}
			}

			if (floorPos.z < targetPos.z) { // target is in front of - - -
				if ((targetPos.z - closestFloor.transform.localScale.z / 2 + bridgeOffset) > // bridge z is needed (not overlapping)
					(floorPos.z + floors[i].transform.localScale.z / 2 - bridgeOffset)) {
					targetFront = true;
					bridgeZPos = new Vector3 (floorPos.x,floorPos.y, z1 - d1 - 0.5f*(z1-z2-d1-d2));
					bridgeZZ = z2-d2-z1-d1;
					bridgeZX = bridgeWidth;
					if (bridgeZZ > 0) {
						bridgeZneeded = true;
					}
				}

			} else if (floorPos.z > targetPos.z) { // target is behind - - -
				if ((targetPos.z + closestFloor.transform.localScale.z / 2 - bridgeOffset) < // bridge z is needed (not overlapping)
					(floorPos.z - floors[i].transform.localScale.z / 2 + bridgeOffset)) {
					targetBehind = true;
					bridgeZPos = new Vector3 (floorPos.x, floorPos.y,  z2 - d2 - 0.5f*(z2-z1-d1-d2));
					bridgeZZ = z1-d1-z2-d2;
					bridgeZX = bridgeWidth;
					if (bridgeZZ > 0) {
						bridgeZneeded = true;
					}
				}
			}

//			Instantiate (bridgePrefab, targetPos, Quaternion.identity).transform.parent = rooms [i].transform;
//			Instantiate (bridgePrefab, floorPos, Quaternion.identity).transform.parent = rooms [i].transform;
				
			// create bridges if needed 

			if (bridgeXneeded) {
				Debug.Log ("MAKE THE X!!!!");
				GameObject newXBridge = Instantiate(bridgePrefab, bridgeXPos,Quaternion.identity);
//				newXBridge.transform.localPosition = bridgeXPos;
				newXBridge.transform.position = new Vector3(x1+w1+((x2-w2)-(x1+w1))*0.5f,floorPos.y,floorPos.z);
//				newXBridge.transform.LookAt (targetPos+new Vector3(w2,0f,0f));
				newXBridge.transform.localScale = new Vector3 (bridgeXX, 0.2f,bridgeXZ);
				newXBridge.transform.parent = rooms [i].transform;
			}
			if (bridgeZneeded) {
				Debug.Log ("MAKE THE Z!!!!");
				GameObject newZBridge = Instantiate(bridgePrefab, bridgeZPos,Quaternion.identity);
//				newZBridge.transform.localPosition = bridgeZPos;
				newZBridge.transform.position = new Vector3(floorPos.x,floorPos.y,z1+d1+((z2-d2)-(z1+d1))*0.5f);
//				newZBridge.transform.LookAt (targetPos+new Vector3(0f,0f,d2));
				newZBridge.transform.localScale = new Vector3 (bridgeZX, 0.2f,bridgeZZ);
				newZBridge.transform.parent = rooms [i].transform;
			}
				

//			newBridge.transform.localScale = new Vector3 (1f, 0.2f, 1f);


            //Debug.Log(closestFloor.transform.position + " is closest to : " + floors[i].transform.position + " with a distance of : " + distance);
            Debug.DrawLine(floors[i].transform.position, closestFloor.transform.position, Color.red, 20f);
        }
    }
}