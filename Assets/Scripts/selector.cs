﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class selector : MonoBehaviour {

	int numCircleParameters;
	int numActiveParameters;

	[SerializeField] GameObject slider;
	[SerializeField] GameObject line;

	GameObject[] icons;

	//for display
	float radius = 125;
	float offsetx = 25;
	float offsety = 35;

	// Use this for initialization
	void Start () {
		numCircleParameters = globalpara.Instance.getNumPara ();
		numActiveParameters = globalpara.Instance.getNumActivePara ();
		icons = new GameObject[numCircleParameters];
			
		fancycircle ();
	}

	//for overall parameters
	void fancycircle(){
		float angle = 2f * Mathf.PI / numActiveParameters;

		for (int i = 0; i < numActiveParameters; i++) {

			GameObject s = Instantiate (slider,Vector3.zero, Quaternion.identity);
			s.transform.SetParent(this.transform);
			s.transform.localPosition = new Vector3 (radius + offsetx, radius + offsety, 0f);
			s.transform.localScale = new Vector3 (0.75f, 0.75f, 0.75f);
			s.transform.localRotation = Quaternion.Euler (new Vector3 (0f, 0f, angle * i * Mathf.Rad2Deg));
			s.GetComponent<icon> ().seticon ((parameters)i);
			icons [i] = s;
			setValue (i, globalpara.Instance.getValue ((parameters)(i)));
		}
	}

	void clear(){
		for (int i = 0; i < transform.childCount; i++) {
			Destroy (transform.GetChild (i).gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
//		temporary to see if you can change the circle willy nilly
//		if (Input.GetKeyDown (KeyCode.N)) {
//			numActiveParameters = 5;
//			clear ();
//			fancycircle ();
//		}
	}

	public float getValue(int i){
		return icons [i].GetComponent<icon> ().getVal ();
	}

	public void setValue(int i, float v){
		icons [i].GetComponent<Slider> ().value = v;
	}
}
