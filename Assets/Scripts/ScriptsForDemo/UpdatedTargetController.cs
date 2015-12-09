using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpdatedTargetController : MonoBehaviour
{

   
    public static List<GameObject> HubDoors = new List<GameObject>();

       
    void Start ()
    {
        

        foreach (GameObject tar in GameObject.FindGameObjectsWithTag("HubDoor"))
        {
            HubDoors.Add(tar);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    

}
