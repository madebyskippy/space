using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class eventBox : MonoBehaviour {

	[SerializeField] events E;

	[SerializeField] Text title;

	// Use this for initialization
	void Start () {
		title.text = "Event "+((int)E+1)+"\n"+title.text;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public events getEvent(){
		return E;
	}

	public void greyOut(){
		title.color = new Color (0, 0, 0, 0.25f);
	}
}
