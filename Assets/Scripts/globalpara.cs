using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalpara : MonoBehaviour{

	private static globalpara instance = null;

	//========================================================================
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

	private string[] parameters = {
		"size",
		"floors",
		"people",
		"trees",
		"furniture"
	};

	public string getParameter (int i) {
		return parameters [i];
	}

	public int getNumPara(){
		return parameters.Length;
	}

}
