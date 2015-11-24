using UnityEngine;
using System.Collections;

public class DoorOpenScript : MonoBehaviour {

    public bool doorOpen = false; //door is closed at the start of the game
    public float doorOpenAngle = 90;
    public float doorCloseAngle = 0;
    public float doorSmoothOpening = 2f;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (doorOpen) //open = true
        {
            Quaternion doorTargetRotationOpen = Quaternion.Euler(0, doorOpenAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, doorTargetRotationOpen, doorSmoothOpening * Time.deltaTime);
        }
        else
        {
            Quaternion doorTargetRotationClose = Quaternion.Euler(0, doorCloseAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, doorTargetRotationClose, doorSmoothOpening * Time.deltaTime);
        }
    }

    public void ChangeDoorState()
    {
        doorOpen = !doorOpen;
        GetComponent<AudioSource>().Play();
    }
}
