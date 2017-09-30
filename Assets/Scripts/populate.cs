using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class populate : MonoBehaviour {

	List<GameObject> rooms = new List <GameObject> ();

	[SerializeField] GameObject personPrefab;
	[SerializeField] GameObject greenPrefab;

	[SerializeField] List <Sprite> peopleSprites = new List <Sprite> ();
	[SerializeField] List <Sprite> greenSprites = new List <Sprite> ();

	[Range(0, 10)]
	[SerializeField] int nrPeople;

	[Range(0, 10)]
	[SerializeField] int nrGreen;


	List<GameObject> currentPeople = new List <GameObject> ();
	List<GameObject> currentGreen = new List <GameObject> ();


	void Awake () {
//		rooms.AddRange(GameObject.FindGameObjectsWithTag ("Room"));
	}



	void Update () {
//		if (Input.GetKeyDown(KeyCode.P)) {
//			Populate ();
//		}
	}

	public void setRooms(List<GameObject> r){
		rooms = r;
	}

	public void Populate(GameObject parent) {
		Debug.Log ("populating");
		PopPeople (parent);
		PopGreen (parent);
	}

	void PopGreen(GameObject parent) {
		// populate witch green
		for (int i = 0; i < rooms.Count; i++) {
			float sizeModifier = (rooms [i].transform.localScale.x * rooms [i].transform.localScale.y * rooms [i].transform.localScale.z)*0.05f;
			// green to create depending on nr green and size of rooms
			float greenToCreate = nrGreen * sizeModifier;
			for (int j = 0; j < greenToCreate; j++) {
				Vector3 newPos = new Vector3 (
					rooms [i].transform.position.x + Random.Range (-rooms [i].transform.localScale.x / 2, rooms [i].transform.localScale.x / 2),
					rooms [i].transform.position.y -  rooms[i].transform.localScale.y/2 + 1,
					rooms [i].transform.position.z + Random.Range (-rooms [i].transform.localScale.z / 2, rooms [i].transform.localScale.z / 2)
				);
				GameObject newGreen = Instantiate (greenPrefab, newPos, Quaternion.identity);
				newGreen.GetComponent<SpriteRenderer> ().sprite = greenSprites [Random.Range (0, greenSprites.Count)];
				newGreen.transform.parent = parent.transform;
				currentGreen.Add (newGreen);

			}
		}
	}

	void PopPeople(GameObject parent) {
		// populate witch people
		for (int i = 0; i < rooms.Count; i++) {
			float sizeModifier = (rooms [i].transform.localScale.x * rooms [i].transform.localScale.y * rooms [i].transform.localScale.z)*0.05f;
			// people to create depending on nr people and size of rooms
			float peopleToCreate = nrPeople * sizeModifier;
			for (int j = 0; j < peopleToCreate; j++) {
				Vector3 newPos = new Vector3 (
					rooms [i].transform.position.x + Random.Range (-rooms [i].transform.localScale.x / 2, rooms [i].transform.localScale.x / 2),
					rooms [i].transform.position.y -  rooms[i].transform.localScale.y/2 + 1,
					rooms [i].transform.position.z + Random.Range (-rooms [i].transform.localScale.z / 2, rooms [i].transform.localScale.z / 2)
				);
				GameObject newPerson = Instantiate (personPrefab, newPos, Quaternion.identity);
				newPerson.GetComponent<SpriteRenderer> ().sprite = peopleSprites [Random.Range (0, peopleSprites.Count)];
				newPerson.transform.parent = parent.transform;
				currentPeople.Add (newPerson);
					
			}
		}
	}



}
	