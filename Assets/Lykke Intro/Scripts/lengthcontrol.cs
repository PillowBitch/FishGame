using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lengthcontrol : MonoBehaviour {

    public float countAtStart = 0;
    public float fireOnThisAmount;
    public float increasePerSecond;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        countAtStart = countAtStart + increasePerSecond * Time.deltaTime;
        if (countAtStart >= fireOnThisAmount)
        {
            Destroy(gameObject);
        }
	}
}
