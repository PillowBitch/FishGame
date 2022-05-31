using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tosscustom : MonoBehaviour {

    float nudges;
    public float dropRate;
    public float smallest;
    public float biggest;
    float nudge2;
    public float smallest2;
    public float biggest2;

    // Use this for initialization
    void Start () {
        nudges = Random.Range(smallest, biggest);
        nudge2 = Random.Range(smallest2, biggest2);
    }
	
	// Update is called once per frame
	void Update () {
        nudge2 = nudge2 - dropRate * Time.deltaTime;
        transform.position = new Vector3(transform.position.x + nudges * Time.deltaTime, transform.position.y + nudge2 * Time.deltaTime, transform.position.z);
    }
}
