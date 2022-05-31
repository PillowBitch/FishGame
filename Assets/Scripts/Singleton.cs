using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    //Singletons uses a static variable reference, typically named instance 
    //which points to the one instance of the singleton object.
    public static Singleton instance;

    //Awake is called before any other methods, 
    //this insures that whatever is inside awake happens before anything else.
    private void Awake()
    {
        //First the script checks if an instance of the object already exists.
        //This is done by checking if the static variable "instance" is defined as null or not.
        //If "instance" is defined as null, we know that no other instances of this object exists.
        //So then we defines "instance" as "this" which points instance to this script.
        if (instance == null)
        {
            instance = this;
        }
        //If our singleton is found to already be defined we immediately destroy this instance of the singleton.
        //This ensures only one instance of the object is ever found.
        else
        {
            Destroy(this);
        }
    }

    //once everything is defined. Other scripts can access methods within our singleton object by refering to the "instance" object.
    //For example if another script wanted to run the method below it would be done as follows:
    //Singleton.instance.Method()
    public void Method()
    {
        Debug.Log("This is a public method.");
    }
}
