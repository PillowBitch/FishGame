using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waitreplace : MonoBehaviour {

    public float countAtStart = 0;
    public float fireOnThisAmount;
    public float increasePerSecond;

    public GameObject spawneds;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        countAtStart = countAtStart + increasePerSecond * Time.deltaTime;
        if (countAtStart >= fireOnThisAmount)
        {
            Destroy(gameObject);
            Instantiate(spawneds, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}
