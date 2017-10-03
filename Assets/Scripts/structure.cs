using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class structure : MonoBehaviour {

	[SerializeField] GameObject beam;
	[SerializeField] GameObject floor;
	[SerializeField] GameObject column;
	[Range(0.1f, 0.5f)]
	[SerializeField] float floorThickness;
	[Range(-0.5f, 0.5f)]
	[SerializeField] float floorOffsetSize;


	public void CreateStructures () {
		GenerateFloors ();
	}

	void GenerateFloors() {
		
			Vector3 buildPos = new Vector3 (
				transform.position.x,
				transform.position.y - transform.position.y/2f + floorThickness/2f,
				transform.position.z
			);
			GameObject newFloor = Instantiate (floor, buildPos, Quaternion.identity);
			
			newFloor.transform.localScale = new Vector3 (
				transform.localScale.x + floorOffsetSize,
				floorThickness,
				transform.localScale.z + floorOffsetSize
			);

	}




}
