using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class person : MonoBehaviour {

	[SerializeField] int numSprites;

	int id;

	void Awake(){
		id = Random.Range (1, numSprites+1);
		GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/people/person" + id);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
