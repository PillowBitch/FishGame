using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goinsides : MonoBehaviour {

    public float rangeHorizontal;
    float chosenNumber;

	// Use this for initialization
	void Start () {
        chosenNumber = Random.Range(rangeHorizontal, -rangeHorizontal);
        transform.position = new Vector3(transform.position.x + chosenNumber, transform.position.y, transform.position.z);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
