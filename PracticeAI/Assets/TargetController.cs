using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetController : MonoBehaviour {

    public static List<GameObject> Targets = new List<GameObject>();

    void Start ()
    {
        foreach (GameObject tar in GameObject.FindGameObjectsWithTag("Door"))
        {
            Targets.Add(tar);
        }

        foreach (GameObject tar in GameObject.FindGameObjectsWithTag("target"))
        {
            Targets.Add(tar);
        }
                
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
