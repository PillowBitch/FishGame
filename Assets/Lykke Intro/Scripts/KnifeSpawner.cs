using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeSpawner : MonoBehaviour {

    
    public GameObject knifeProjectile;
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
            Instantiate(knifeProjectile, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
            countAtStart = 0;
        }
        
    }
}
