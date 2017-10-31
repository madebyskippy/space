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







	public void Init(){
		id = Random.Range (1, numSprites+1);
		GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/people/person" + id);
	}

	public void Init(int i){
		id = i;
		GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/people/person" + id);
	}

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        WalkRandomly();
	}

	public JSONObject export(){
		JSONObject data = new JSONObject();
		data ["position x"].AsFloat = transform.position.x;
		data ["position y"].AsFloat = transform.position.y;
		data ["position z"].AsFloat = transform.position.z;
		data ["id"].AsInt = id;
		return data;
	}

    void WalkRandomly() {
        if (agent.pathPending || agent.remainingDistance > 0.1f)
            return;

        agent.destination = randomWalkRange * Random.insideUnitCircle;
    }
}
