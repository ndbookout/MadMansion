using UnityEngine;
using System.Collections;

public class Character_Camera_Controller : MonoBehaviour {
    //Variable settings Public
    public Transform target; //This the refrence to the ghost
    public float lookSmooth = 0.09f; // how fast do we want to look at the ghost
    public Vector3 offsetFromTargetGhost = new Vector3(0, 6, -8); // how far do we want the camera to be from ghost
    public float xTilt = 10; //How far we want our camera to be rotate on the xAxis is going to be up or down

    //Variable setting private
    private Vector3 destination = Vector3.zero; //This is where we want our camera to move too
    Character_Controller charController; // to acess the character rotation
    float rotationVelocity = 0; //This is to set how fast our rotation moves in the camera

    //Methods that we are using in our script

    void Start()
    {
        SetCameraTarget(target);
    }

    void SetCameraTarget(Transform t)
    {
        target = t;

        if (target != null)
        {
            if (target.GetComponent<Character_Controller>())
            {
                charController = target.GetComponent<Character_Controller>();
            }
            else
                Debug.LogError("The camera target need this character controller");
        }
        else
            Debug.LogError("Your camera needs a target");
    }

    void LateUpdate()
    {
        //moves the camera
        MoveToGhost();
        //this rotates the camera
        LookAtGhost();
    }

    void MoveToGhost()
    {
        destination = charController.TargetRotation * offsetFromTargetGhost;
        destination += target.position;
        transform.position = destination;
    }

    void LookAtGhost()
    {
        float eulerYAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y, ref rotationVelocity, lookSmooth);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x,eulerYAngle, 0);
    }



}
