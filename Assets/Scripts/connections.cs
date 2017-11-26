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

	bool drawDebugLines = false;

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
		drawDebugLines = false;
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

			float x1 = floorPos.x;
			float x2 = targetPos.x;
			float w1 = floors [i].transform.localScale.x *0.5f;
			float w2 = closestFloor.transform.localScale.x *0.5f;

			float z1 = floorPos.z;
			float z2 = targetPos.z;
			float d1 = floors [i].transform.localScale.z *0.5f;
			float d2 = closestFloor.transform.localScale.z *0.5f;

            float xdir = 1;
            if (x1 > x2)
                xdir = -1;

            float xedge1 = x1 + w1 * xdir;
            float xedge2 = x2 - w2 * xdir;
            float xscale = Mathf.Abs(xedge2 - xedge1);


            float xpos = xedge1 + xdir * 0.5f * Mathf.Abs(xedge2 - xedge1);

			if (xdir == 1 && xedge1 < xedge2 || xdir == -1 && xedge1 > xedge2) {
				Vector3 bridgeXPos = new Vector3 (xpos, floors [i].transform.position.y, floors [i].transform.position.z);
				Vector3 bridgeXScale = new Vector3 (xscale, 0.1f, bridgeWidth);

				GameObject newXBridge = Instantiate (bridgePrefab, new Vector3(0,0,0), Quaternion.identity);
				newXBridge.transform.localPosition = bridgeXPos;
				newXBridge.transform.localScale = bridgeXScale;
				newXBridge.transform.parent = floors [i].transform;
			}


            float zdir = 1;
            if (z1 > z2)
                zdir = -1;

            float zedge1 = z1 + d1 * zdir;
            float zedge2 = z2 - d2 * zdir;
            float zscale = Mathf.Abs(zedge2 - zedge1);


            float zpos = zedge1 + zdir * 0.5f * Mathf.Abs(zedge2 - zedge1);

			if (zdir == 1 && zedge1 < zedge2 || zdir == -1 && zedge1 > zedge2) {
				Vector3 bridgeZPos = new Vector3 (targetPos.x, floors [i].transform.position.y, zpos);
				Vector3 bridgeZScale = new Vector3 (bridgeWidth, 0.1f, zscale);

				GameObject newZBridge = Instantiate (bridgePrefab,new Vector3(0,0,0), Quaternion.identity);
				newZBridge.transform.localPosition = bridgeZPos;
				newZBridge.transform.localScale = bridgeZScale;
				newZBridge.transform.parent = floors [i].transform;
			}

		
            
        }
		drawDebugLines = true;
    }

	void Update() {

		if (drawDebugLines) {

			for (int i = 0; i < floors.Count; i++) {
				float distance = 10000f;
				GameObject closestFloor = null;
				for (int j = 0; j < floors.Count; j++) {
					float currentDistance = Vector3.Distance (floors [i].transform.position, floors [j].transform.position);

					if (currentDistance < distance && currentDistance > 0f) {
						distance = currentDistance;
						closestFloor = floors [j];
					}
				}

				Debug.DrawLine (floors [i].transform.position, closestFloor.transform.position, Color.red, 1f);
			
			}
		}

	}
}