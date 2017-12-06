using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bigSlider : MonoBehaviour {

	parameters para;

	parameters[] small;
	GameObject[] smallsliders;

	GameObject sliderPrefab;

	bool isActive;
	bool smallActive;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void setSliders(){
		float angle = 2f * Mathf.PI / 5; //just set it to 5 so they're not all crowded around the big one

		for (int i = 0; i < small.Length; i++) {
			GameObject s = Instantiate (sliderPrefab,Vector3.zero, Quaternion.identity);
			s.transform.SetParent(this.transform);
			Vector3[] corners = new Vector3[4];
			GetComponent<RectTransform> ().GetLocalCorners (corners);
			s.transform.localPosition = corners[2]-0.5f*(corners [2]-corners[3]);
			s.transform.localScale = Vector3.one * 0.65f;
			s.transform.localRotation = Quaternion.Euler (new Vector3 (0f, 0f, angle * i * Mathf.Rad2Deg-(360f/5f)*(0.5f*(small.Length-1))));
			s.GetComponent<icon> ().seticon (small[i]);
			s.GetComponent<Slider> ().interactable = false;
			smallsliders [i] = s;
		}
	}

	public void setActive(bool a){
		isActive = a;
		GetComponent<Slider> ().interactable = isActive;
		if (!isActive) {
			for (int i = 0; i < smallsliders.Length; i++) {
				smallsliders[i].GetComponent<Slider> ().interactable = false;
			}
		}
	}

	public void setSmallActive(bool sa){
		smallActive = sa;
		if (smallActive) {
			GetComponent<Slider> ().interactable = false;
			for (int i = 0; i < smallsliders.Length; i++) {
				smallsliders[i].GetComponent<Slider> ().interactable = true;
			}
		} else {
			GetComponent<Slider> ().interactable = true;
			for (int i = 0; i < smallsliders.Length; i++) {
				smallsliders[i].GetComponent<Slider> ().interactable = false;
			}
		}
	}

	public void prefab(GameObject s){
		sliderPrefab = s;
	}

	public void setP(parameters p){
		para = p;
	}

	public void setSmallP(parameters[] s){
		small = s;
		smallsliders = new GameObject[small.Length];
		setSliders ();
	}
}
