using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum parameters {
	bounds, height, fidelity, chaos, density, cohesion, 
	floorThickness
};

public enum events {
	peoplemeet
};

public class globalpara : MonoBehaviour{

	private static globalpara instance = null;

	//========================================================================

	//code help from hang ruan :-)
	public static globalpara Instance {
		get { 
			return instance;
		}
	}

	void Awake () {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}
	//========================================================================

	[SerializeField] int numPara;
	int numActivePara;

	private float[] parameterValue;

	private bool[] eventState;

//	TextAsset nameText;
	string[] names;

	void Start(){
		int length = System.Enum.GetNames(typeof(parameters)).Length;
		parameterValue = new float[length];
		for (int i=0; i<length; i++){
			parameterValue [i] = 1f;
		}
		length = System.Enum.GetNames(typeof(events)).Length;
		eventState = new bool[length];
		for (int i=0; i<length; i++){
			eventState [i] = false;
		}

		//start with 3 parameters.
		numActivePara = 3;

		TextAsset nameText = (TextAsset)Resources.Load("texts/firstnames");
		names = nameText.text.Split("\n"[0]);
	}

	public string getRandomName(){
		return names [Random.Range (0, names.Length)];
	}

	//-----
	//for parameter value stuff
	//-----

	public void setValue (parameters para, float val){
		parameterValue [(int)para] = val;
	}

	public float getValue (parameters para){
		return parameterValue[(int)para];
	}

	public int getNumPara(){
		return numPara;
	}

	public int getNumActivePara(){
		return numActivePara;
	}

	//-----
	//for event state stuff
	//-----

	public void setState(events e, bool s){
		eventState [(int)e] = s;
		//you can do stuff here to set the numactivepara to be higher
		//(thus giving u more sliders)
		//(depending on what events have been happened)
	}
	public bool getState(events e){
		return eventState [(int)e];
	}

}
