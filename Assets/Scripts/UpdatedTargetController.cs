using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpdatedTargetController : MonoBehaviour
{

    public static List<GameObject> NorthWingDoors = new List<GameObject>();
    public static List<GameObject> SouthWingDoors = new List<GameObject>();
    public static List<GameObject> WestWingDoors = new List<GameObject>();
    public static List<GameObject> HubDoors = new List<GameObject>();

    public GameObject NorthWingTrigger;
    public GameObject SouthWingTrigger;
    public GameObject WestWingTrigger;

    
    

    void Start ()
    {
        foreach (GameObject tar in GameObject.FindGameObjectsWithTag("NorthWing"))
        {
            NorthWingDoors.Add(tar);
        }

        foreach (GameObject tar in GameObject.FindGameObjectsWithTag("SouthWing"))
        {
            SouthWingDoors.Add(tar);
        }

        foreach (GameObject tar in GameObject.FindGameObjectsWithTag("WestWing"))
        {
            WestWingDoors.Add(tar);
        }

        foreach (GameObject tar in GameObject.FindGameObjectsWithTag("HubDoor"))
        {
            HubDoors.Add(tar);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    

}
