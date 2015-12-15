using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorController : MonoBehaviour
{
    //DoorOpenScript handles opening and closing of doors
    public bool doorOpen = false; //door is closed at the start of the game
    public float doorOpenAngle = 90;
    private Quaternion doorCloseAngle;
    public float doorSmoothOpening = 2f;

    //Door handles targeting inside room
    //public List<GameObject> roomTargets;

    //void Awake()
    //{
    //    roomTargets = new List<GameObject>();
    //}

    //public void AddToTargetList(GameObject newTarget)
    //{
    //    roomTargets.Add(newTarget);
    //}

    //Below controls door opening---
    void Start()
    {
        doorCloseAngle = transform.localRotation;
    }

    public void ChangeDoorState()
    {
        doorOpen = !doorOpen;
        GetComponent<AudioSource>().Play();

        if (doorOpen) //open = true
        {
            Quaternion doorTargetRotationOpen = Quaternion.Euler(doorCloseAngle.x, doorOpenAngle, doorCloseAngle.z);
            transform.localRotation *= doorTargetRotationOpen; //Quaternion.Slerp(transform.localRotation, doorTargetRotationOpen, doorSmoothOpening * Time.deltaTime);
        }
        else
        {
            Quaternion doorTargetRotationClose = doorCloseAngle;
            transform.localRotation = doorTargetRotationClose; //Quaternion.Slerp(transform.localRotation, doorTargetRotationClose, doorSmoothOpening * Time.deltaTime);
            //TargetController.Targets.Add(gameObject);
        }
    }
}
