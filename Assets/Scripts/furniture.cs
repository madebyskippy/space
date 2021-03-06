﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class furniture : MonoBehaviour {

	[SerializeField] int numSprites;

	int id;

	public void Init(){
		id = Random.Range (1, numSprites+1);
		GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/furniture/furniture" + id);
	}

	public void Init(int i){
		id = i;
		GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/furniture/furniture" + id);
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public JSONObject export(){
		JSONObject data = new JSONObject();
		data ["position x"].AsFloat = transform.position.x;
		data ["position y"].AsFloat = transform.position.y;
		data ["position z"].AsFloat = transform.position.z;
		data ["id"].AsInt = id;
		return data;
	}
}
