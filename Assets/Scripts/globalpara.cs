using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	[SerializeField] int numStructPara;

	private Dictionary<string, float> parameters = new Dictionary<string, float>(){
		{"bounds",			1f },	//building bounds (length width)
		{"height",			1f },	//building height
		{"fidelity", 		1f },	//room fidelity
		{"chaos",			1f },	//room symmetry/randomness
		{"density", 		1f },	//room density 
		{"cohesion",		1f } 	//coheisveness of types of structure
		// from here should be struct para
		,{"floorThickness",		1f}
//		,{"floorOffsetSize",	1f}
//		,{"columnThickness",	1f}
//		,{"columnDistance",		1f}
//		,{"beamWidthMain",		1f}
//		,{"beamHeightMain",		1f}
//		,{"beamWidthSecond",	1f}
//		,{"beamHeightSecond",	1f}
//		,{"beamLengthOffset",	1f}
//		,{"gridDistanceMain",	1f}
//		,{"gridDistanceSecond",	1f}
//		,{"beamHeightOffset",	1f}

	};

	public string getParameterName (int i) {
		string[] pkeys = new string[parameters.Count];
		parameters.Keys.CopyTo (pkeys, 0);
		return pkeys [i];
	}

	public void setValue (string para, float val){
		parameters [para] = val;
	}

	public float getValue (string para){
		return parameters[para];
	}

	public int getNumPara(){
		return numPara;
	}

	public int getNumStructPara(){
		return numStructPara;
	}
}
