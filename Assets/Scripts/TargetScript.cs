using UnityEngine;
using System.Collections;

public class TargetScript : MonoBehaviour
{
    private DoorController nearestDoor;

	void Awake()
    {
        nearestDoor = FindObjectOfType<DoorController>();
        //nearestDoor.AddToTargetList(gameObject);
    }
}
