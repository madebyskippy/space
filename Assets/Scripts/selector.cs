using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selector : MonoBehaviour {

	/*
	 * TODO 
	 * make the sliders affect stuff
	 * draw the lines
	 * 
	 */

	[SerializeField] int numParameters;
	[SerializeField] GameObject slider;
	[SerializeField] GameObject line;

	GameObject[] icons;
	GameObject[] lines;

	//for display
	float radius = 125;
	float offsetx = 25;
	float offsety = 35;

	// Use this for initialization
	void Start () {
		icons = new GameObject[numParameters];
		lines = new GameObject[numParameters];
		//set up pentagon
		float angle = 2f * Mathf.PI / numParameters;
		for (int i = 0; i < numParameters; i++) {
//
//			GameObject lmid = Instantiate(line);
//			lmid.transform.parent = this.gameObject.transform;
//
//			lmid.GetComponent<LineRenderer> ().SetPosition (0, this.transform.TransformPoint(new Vector3(radius+offsetx,radius+offsety,0)));
//			lmid.GetComponent<LineRenderer> ().SetPosition (1, icon [i].transform.position);

			GameObject s = Instantiate (slider,Vector3.zero, Quaternion.identity);
			s.transform.parent = this.transform;
			s.transform.localPosition = new Vector3 (radius + offsetx, radius + offsety, 0f);
			s.transform.localScale = new Vector3 (0.75f, 0.75f, 0.75f);
			s.transform.localRotation = Quaternion.Euler (new Vector3 (0f, 0f, angle * i * Mathf.Rad2Deg));
			s.GetComponent<icon> ().seticon ("floor");//global.getParameter(i));
			icons [i] = s;

			if (i > 0) {
				//draw the lineeee
				GameObject l = Instantiate(line);
				l.transform.parent = this.gameObject.transform;
				l.GetComponent<LineRenderer> ().SetPosition (0, this.transform.TransformPoint(new Vector3(radius+offsetx+radius*Mathf.Cos(angle*(i-1)),radius+offsety+radius*Mathf.Sin(angle*(i-1)),0f)));
				l.GetComponent<LineRenderer> ().SetPosition (1, this.transform.TransformPoint(new Vector3(radius+offsetx+radius*Mathf.Cos(angle*(i)),radius+offsety+radius*Mathf.Sin(angle*(i)),0f)));
				lines[i-1]=l;
			}
		}
		GameObject lend = Instantiate(line);
		lend.transform.parent = this.gameObject.transform;
		lend.GetComponent<LineRenderer> ().SetPosition (0, this.transform.TransformPoint(new Vector3(radius+offsetx+radius*Mathf.Cos(angle*(numParameters-1)),radius+offsety+radius*Mathf.Sin(angle*(numParameters-1)),0f)));
		lend.GetComponent<LineRenderer> ().SetPosition (1, this.transform.TransformPoint(new Vector3(radius+offsetx+radius,radius+offsety,0f)));
		lines [numParameters - 1] = lend;
	}
	
	// Update is called once per frame
	void Update () {
		drawlines ();
	}

	void drawlines(){
		float angle = 2f * Mathf.PI / numParameters;
		for (int i = 1; i < numParameters; i++) {
			float x1 = radius + offsetx + radius * Mathf.Cos (angle * (i - 1)) * icons [i - 1].GetComponent<icon> ().getVal ();
			float y1 = radius + offsety + radius * Mathf.Sin (angle * (i - 1)) * icons [i - 1].GetComponent<icon> ().getVal ();
			float x2 = radius + offsetx + radius * Mathf.Cos (angle * (i)) * icons [i].GetComponent<icon> ().getVal ();
			float y2 = radius + offsety + radius * Mathf.Sin (angle * (i)) * icons [i].GetComponent<icon> ().getVal ();
			lines[i-1].GetComponent<LineRenderer> ().SetPosition (0, this.transform.TransformPoint(new Vector3(x1,y1,0f)));
			lines[i-1].GetComponent<LineRenderer> ().SetPosition (1, this.transform.TransformPoint(new Vector3(x2,y2,0f)));
		}
		float x = radius + offsetx + radius * Mathf.Cos (angle * (numParameters - 1)) * icons [numParameters - 1].GetComponent<icon> ().getVal ();
		float y = radius + offsety + radius * Mathf.Sin (angle * (numParameters - 1)) * icons [numParameters - 1].GetComponent<icon> ().getVal ();
		lines[numParameters-1].GetComponent<LineRenderer> ().SetPosition (0, this.transform.TransformPoint(new Vector3(x,y,0f)));
		x = radius + offsetx + radius * icons [0].GetComponent<icon> ().getVal ();
		y = radius + offsety;
		lines[numParameters-1].GetComponent<LineRenderer> ().SetPosition (1, this.transform.TransformPoint(new Vector3(x,y,0f)));
	}
}
