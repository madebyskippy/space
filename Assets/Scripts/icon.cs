using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class icon : MonoBehaviour {

	[SerializeField] Text label;

	[SerializeField] parameters parameter;

	float val;

	void Start () {
		val = 1;
		label.text = parameter.ToString();
	}

	public void seticon(parameters ppara){
		parameter = ppara;
		label.text = parameter.ToString();
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
