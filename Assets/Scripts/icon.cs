using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class icon : MonoBehaviour {

	[SerializeField] Text label;

	string type;

	float val;

	void Start () {
		val = 1;
	}

	public void seticon(string ptype){
		type = ptype;
		label.text = type;
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
