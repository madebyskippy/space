using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class icon : MonoBehaviour {

	/*
	 * to initiate
	 * needs to know what it controls
	 * 
	 * to keep track of
	 * how far it is from the center
	 * 
	 * it should only move on that line
	 */

	string type;
	Vector2 max, min;

	float val;

	void Start () {
		val = 1;
	}

	public void seticon(string ptype){//, Vector2 pmax, Vector2 pmin){
		type = ptype;
//		max = pmax;
//		min = pmin;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void change (float v){
		val = v;
	}

	public float getVal(){
		return val;
	}
}
