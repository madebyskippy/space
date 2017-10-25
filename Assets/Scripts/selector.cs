using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class selector : MonoBehaviour {

	int numCircleParameters;
	int numStructureParameters;

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
		icons = new GameObject[numCircleParameters];
			
		fancycircle ();
	}

	//for overall parameters
	void fancycircle(){
		float angle = 2f * Mathf.PI / numCircleParameters;

		for (int i = 0; i < numCircleParameters; i++) {

			GameObject s = Instantiate (slider,Vector3.zero, Quaternion.identity);
			s.transform.SetParent(this.transform);
			s.transform.localPosition = new Vector3 (radius + offsetx, radius + offsety, 0f);
			s.transform.localScale = new Vector3 (0.75f, 0.75f, 0.75f);
			s.transform.localRotation = Quaternion.Euler (new Vector3 (0f, 0f, angle * i * Mathf.Rad2Deg));
			string p = globalpara.Instance.getParameterName (i);
			s.GetComponent<icon> ().seticon (p);
			icons [i] = s;

		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	public float getValue(int i){
		return icons [i].GetComponent<icon> ().getVal ();
	}

	public void setValue(int i, float v){
		icons [i].GetComponent<Slider> ().value = v;
	}
}
