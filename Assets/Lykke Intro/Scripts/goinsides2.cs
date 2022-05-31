using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goinsides2 : MonoBehaviour {

    public float rangeVertical;
    float chosenNumber;

    // Use this for initialization
    void Start () {
        chosenNumber = Random.Range(rangeVertical, -rangeVertical);
        transform.position = new Vector3(transform.position.x, transform.position.y + chosenNumber, transform.position.z);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
