using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class icon : MonoBehaviour {

	string type;

	float val;

	void Start () {
		val = 1;
	}

	public void seticon(string ptype){
		type = ptype;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void change (float v){
		val = v;
		globalpara.Instance.setValue (type, val);
	}

	public float getVal(){
		return val;
	}
}
