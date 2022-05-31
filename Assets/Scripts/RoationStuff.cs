using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoationStuff : MonoBehaviour
{

    

    // Start is called before the first frame update
    void Start()
    {
        //Rotation of 30 degrees around x-axis and 90 degrees around y-axis.
        Vector3 rotationEuler = new Vector3(30, 90, 0);
        //Converting the rotation vector into a quaternion.
        Quaternion rotation = Quaternion.Euler(rotationEuler);

        //Quaternion.eulerAngles returns a Vector3 and therefor it is perfectly valid code to set a position as the rotations euler angles.
        transform.position = rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
