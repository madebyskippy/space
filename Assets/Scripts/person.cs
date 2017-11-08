using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.AI;

public class person : MonoBehaviour {

	[SerializeField] int numSprites;



	int id;
    public float randomWalkRange = 3f;
    NavMeshAgent agent;

	string name;

	TextMesh nameText;

	bool hovering;

	public void Init(){
		id = Random.Range (1, numSprites+1);
		GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/people/person" + id);
		name = globalpara.Instance.getRandomName ();
		nameText.text = name;
	}

	public void Init(int i, string n){
		id = i;
		GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/people/person" + id);
		name = n;
		nameText.text = name;
	}

	// Use this for initialization
	void Awake () {
        agent = GetComponent<NavMeshAgent>();
		nameText = transform.GetChild (0).gameObject.GetComponent<TextMesh> ();
		hovering = false;
	}
	
	// Update is called once per frame
	void Update () {
        WalkRandomly();
		if (hovering) {
			nameText.gameObject.SetActive (true);
		} else {
			nameText.gameObject.SetActive (false);
		}
		hovering = false;
	}

	public JSONObject export(){
		JSONObject data = new JSONObject();
		data ["position x"].AsFloat = transform.position.x;
		data ["position y"].AsFloat = transform.position.y;
		data ["position z"].AsFloat = transform.position.z;
		data ["name"] = name;
		data ["id"].AsInt = id;
		return data;
	}

    void WalkRandomly() {
        if (agent.pathPending || agent.remainingDistance > 0.1f)
            return;

        agent.destination = randomWalkRange * Random.insideUnitCircle;
    }

	public void hover(){
		hovering = true;
	}
}
