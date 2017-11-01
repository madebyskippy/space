using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum parameters {bounds, height, fidelity, chaos, density, cohesion, floorThickness};

public enum achievements {peoplemeet};

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

	private Dictionary<string, float> parameters = new Dictionary<string, float>(){
		{"bounds",			1f },	//building bounds (length width)
		{"height",			1f },	//building height
		{"fidelity", 		1f },	//room fidelity
		{"chaos",			1f },	//room symmetry/randomness
		{"density", 		1f },	//room density 
		{"cohesion",		1f } 	//coheisveness of types of structure

		,{"floorThickness",		0.1f}
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

	private float[] parameterState;

	private bool[] achievementState;

	void Start(){
		int length = System.Enum.GetNames(typeof(parameters)).Length;
		parameterState = new float[length];
		for (int i=0; i<length; i++){
			parameterState [i] = 1f;
		}
		length = System.Enum.GetNames(typeof(achievements)).Length;
		achievementState = new bool[length];
		for (int i=0; i<length; i++){
			achievementState [i] = false;
		}
	}

	public void setValue (parameters para, float val){
		parameterState [(int)para] = val;
	}

	public float getValue (parameters para){
		return parameterState[(int)para];
	}

	public int getNumPara(){
		return numPara;
	}

	public int getNumActivePara(){
		return numActivePara;
	}

}
