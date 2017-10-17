using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class icon : MonoBehaviour {

	[SerializeField] Text label;

	string type;
	string parameter;

	float val;

	void Start () {
		val = 1;
	}

	public void seticon(string ppara){
		parameter = ppara;
		label.text = parameter;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void change (float v){
		val = v;
		globalpara.Instance.setValue (parameter, val);
	}

	public float getVal(){
		return val;
	}
}
