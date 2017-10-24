﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SimpleJSON;

public class manager : MonoBehaviour {

	[SerializeField] fitter roomfitter;
	[SerializeField] GameObject building; 

	[Header ("for loading")]
	[SerializeField] GameObject cube;
	[SerializeField] GameObject greenPrefab;
	[SerializeField] GameObject peoplePrefab;


	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			roomfitter.create ();

		}

		if (Input.GetKeyDown (KeyCode.K)) {
			Debug.Log ("save the building");
			save ();
		}
		if (Input.GetKeyDown (KeyCode.L)) {
			Debug.Log ("load a building");
			load ();
		}
			
	}

	void save(){
		//json shit
		JSONObject data = new JSONObject();
		for (int i = 0; i < building.transform.childCount; i++) {
			JSONObject roomdata = new JSONObject();

			Transform container = building.transform.GetChild(i);
			Transform r = container.GetChild (0);

			roomdata ["room"] = r.gameObject.GetComponent<room> ().export ();
			//just get data for greens & people. the rest are generated by room and should be same when reloaded
			JSONObject greendata = new JSONObject();
			JSONObject peopledata = new JSONObject();
			int greencount = 0;
			int peoplecount = 0;
			for (int j = 1; j < r.transform.childCount; j++) {
				Transform roomchild = r.transform.GetChild (j);
				if (roomchild.name == "Green(Clone)") {
					JSONObject green = new JSONObject();
					green = roomchild.gameObject.GetComponent<green> ().export ();
					greendata[""+greencount] = green;
					greencount++;
				} else if (roomchild.name == "Person(Clone)") {
					JSONObject people = new JSONObject();
					people = roomchild.gameObject.GetComponent<person> ().export ();
					peopledata[""+peoplecount] = people;
					peoplecount++;
				}
			}
			roomdata ["greens"] = greendata;
			roomdata ["people"] = peopledata;
			roomdata ["xScale"].AsFloat = r.transform.localScale.x;
			roomdata ["yScale"].AsFloat = r.transform.localScale.y;
			roomdata ["zScale"].AsFloat = r.transform.localScale.z;
			roomdata ["xPos"].AsFloat = r.transform.position.x;
			roomdata ["yPos"].AsFloat = r.transform.position.y;
			roomdata ["zPos"].AsFloat = r.transform.position.z;
			roomdata ["containerxPos"].AsFloat = container.transform.position.x;
			roomdata ["containeryPos"].AsFloat = container.transform.position.y;
			roomdata ["containerzPos"].AsFloat = container.transform.position.z;
			data ["container"+i] = roomdata;
		}
		data ["num rooms"].AsInt = building.transform.childCount;

		string timestamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
		WriteJSONtoFile ("Assets/Resources/Buildings", timestamp+".txt", data);
	}

	void load(){
		//other json shit
		JSONNode json = ReadJSONFromFile("Assets/Resources/Buildings", "20171016193035.txt");

		roomfitter.delete ();
		roomfitter.clear ();

		List<GameObject> rms = new List<GameObject>();

		//ok load the bldg
		for (int i = 0; i < json ["num rooms"]; i++) {
			//container + room
			GameObject t = Instantiate (cube, new Vector3 (0,0,0), Quaternion.identity);
			GameObject p = new GameObject ();
			p.transform.position = new Vector3 (json["container"+i]["containerxPos"],json["container"+i]["containeryPos"],json["container"+i]["containerzPos"]);
			t.transform.parent = p.transform;
			t.transform.position = new Vector3 (json["container"+i]["xPos"],json["container"+i]["yPos"],json["container"+i]["zPos"]);
			p.transform.parent = building.transform;
			t.transform.localScale = new Vector3 (json["container"+i]["xScale"],json["container"+i]["yScale"],json["container"+i]["zScale"]);
			Vector3 d = new Vector3(json["container"+i]["room"]["dimensions x"],json["container"+i]["room"]["dimensions y"],json["container"+i]["room"]["dimensions z"]);
			Vector2 loc = new Vector2 (json ["container" + i] ["room"] ["location x"],json ["container" + i] ["room"] ["location y"]);
			float l = json ["container" + i] ["room"] ["level"];
			float floorT = json ["container" + i] ["room"] ["floor thickness"];
			float floorS = json ["container" + i] ["room"] ["floor offset size"];
			int numG = json ["container" + i] ["room"] ["num greens"];
			int numP = json ["container" + i] ["room"] ["num ppl"];
			t.GetComponent<room> ().InitFromSave (d,l,loc,floorT,floorS,numG,numP);
			rms.Add (t);

			//greens
			for (int j=0; j<json ["container" + i] ["room"] ["num greens"]; j++){
				GameObject newGreen = Instantiate (greenPrefab);
				newGreen.transform.localScale = Vector3.one * 0.5f; //TEMPORARY
				newGreen.transform.parent = t.transform;
				float posx = json ["container" + i] ["greens"] [""+j] ["position x"];
				float posy = json ["container" + i] ["greens"] [""+j] ["position y"];
				float posz = json ["container" + i] ["greens"] [""+j] ["position z"];
				newGreen.transform.position = new Vector3(posx,posy,posz);
				newGreen.GetComponent<green> ().Init (json ["container" + i] ["greens"] [""+j] ["id"]);
			}

			//people
			for (int j=0; j<json ["container" + i] ["room"] ["num ppl"]; j++){
				GameObject newPpl = Instantiate (peoplePrefab);
				newPpl.transform.localScale = Vector3.one * 0.5f; //TEMPORARY
				newPpl.transform.parent = t.transform;
				float posx = json ["container" + i] ["people"] [""+j] ["position x"];
				float posy = json ["container" + i] ["people"] [""+j] ["position y"];
				float posz = json ["container" + i] ["people"] [""+j] ["position z"];
				newPpl.transform.position = new Vector3(posx,posy,posz);
				newPpl.GetComponent<person> ().Init (json ["container" + i] ["people"] [""+j] ["id"]);
			}

		}

		roomfitter.setRoomList (rms);
	}


	//---------------
	//---------------	JSON stuff
	//---------------
	static void WriteJSONtoFile(string path, string fileName, JSONObject json){
		StreamWriter sw = new StreamWriter(path + "/" + fileName);

		sw.Write(json.ToString());

		sw.Close();
	}

	static JSONNode ReadJSONFromFile(string path, string fileName){
		StreamReader sr = new StreamReader (path + "/" + fileName);

		string resultstring = sr.ReadToEnd ();

		sr.Close ();

		JSONNode result = JSON.Parse (resultstring);

		return result;
	}

}
