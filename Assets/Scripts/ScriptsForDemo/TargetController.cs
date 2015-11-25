using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetController : MonoBehaviour
{
    public static List<GameObject> Targets = new List<GameObject>();

    void Start ()
    {
        foreach (DoorController door in FindObjectsOfType<DoorController>())
        {
            Targets.Add(door.gameObject);
        }

        foreach (GameObject tar in GameObject.FindGameObjectsWithTag("Target"))
        {
            Targets.Add(tar);
        }              
    }
}
