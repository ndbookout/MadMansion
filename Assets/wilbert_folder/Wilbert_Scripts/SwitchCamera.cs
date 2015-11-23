using UnityEngine;
using System.Collections;

public class SwitchCamera : MonoBehaviour {

    public Camera[] cameras;
    public Camera masterCamera;
    public int cameraIndex;
    public string CAMERA_AXIS = "SwitcCamera";
    public float cameraInput;
    public LayerMask CameraSwitchBoard;
    public float distanceToPanel = 0.5f;

    // Use this for initialization
    void Start ()
    {
        cameraInput = 0;
	    foreach (Camera cam in cameras)
        {
            cam.depth = -10;
        }
        masterCamera.depth = 1;
        cameraIndex = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        GetInput();
        SwitchCameraOnPress();
        masterCamera.transform.position = cameras[cameraIndex].transform.position;
        masterCamera.transform.rotation = cameras[cameraIndex].transform.rotation;
	}

    public bool switchPanel()
    {   //This will create an ray that will check the vector down, distance to groun will check the distance, ground will allows to jump
        return Physics.Raycast(transform.position, Vector3.forward, distanceToPanel, CameraSwitchBoard);
    }

    void GetInput()
    {
        cameraInput = Input.GetAxisRaw(CAMERA_AXIS);
    }

    void SwitchCameraOnPress()
    {
        if (cameraInput > 0)
        {
            cameraIndex = (cameraIndex + 1) % cameras.Length;
        }
    }
}
