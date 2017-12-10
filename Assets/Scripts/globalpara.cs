using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum parameters {
	stability, size, density, fidelity, chaos,

	walls, thickness, distance, roof, 	//stability
	width, height, depth,				//size
	in_out, up_down,					//density
										//fidelity
	rooms, structure					//chaos
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

	int numPara = 5;
	int numActivePara;
	int[] numSmallPara = new int[]{4,3,2,0,2}; //for stab, size, density, fidel, chaos
	int[] SmallParaStartIndex = new int[5]; //this is stupid but dont fight me

	private float[] parameterValue;

	private bool[] eventState;

	string[] names;

	void Start(){
		int total = 0;
		for (int i = 0; i < numSmallPara.Length; i++) {
			SmallParaStartIndex [i] = total;
			total += numSmallPara [i];
		}

		int length = System.Enum.GetNames(typeof(parameters)).Length;
		parameterValue = new float[length];
		for (int i=0; i<length; i++){
			parameterValue [i] = 0.75f;
		}
		length = System.Enum.GetNames(typeof(events)).Length;
		eventState = new bool[length];
		for (int i=0; i<length; i++){
			eventState [i] = false;
		}

		//start with these many parameters.
		numActivePara = 5;

		TextAsset nameText = (TextAsset)Resources.Load("texts/firstnames");
		names = nameText.text.Split("\n"[0]);
	}

	public string getRandomName(){
		return names [Random.Range (0, names.Length)];
	}

	//-----
	//for parameter value stuff
	//-----

	public void setValue (parameters p, float val){
		parameterValue [(int)p] = val;
		//if it's a big slider
		if ((int)p < numPara) {
			//IT'S BIG!!1
			//change all the small ones with it
			for (int i = 0; i < numSmallPara [(int)p]; i++) {
				parameterValue [numPara+SmallParaStartIndex[(int)p]+i] = val;
			}
		}
	}

	public float getBigAverage(parameters p){
		float total = 0;
		int num = numSmallPara [(int)p];
		for (int i = 0; i < num; i++) {
			total += parameterValue [numPara + SmallParaStartIndex [(int)p] + i];
		}

		return (float)(total / (float)num);
	}

	public float getValue (parameters p){
		return parameterValue[(int)p];
	}

	public int getNumPara(){
		return numPara;
	}

	public int getNumActivePara(){
		return numActivePara;
	}

	public int getNumSmallP(parameters p){
		return numSmallPara [(int)p];
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
