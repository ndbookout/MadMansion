using UnityEngine;
using System.Collections;

public class SwitchCamera : MonoBehaviour {

    public Camera[] cameras;
    public Camera masterCamera;
    public int cameraIndex;
    public string CAMERA_AXIS = "SwitcCamera";
    public float cameraInput;
    public Character_Camera_Controller camController;


    public LayerMask CameraSwitchBoard;
    public float distanceToPanel = 0.5f;

    // Use this for initialization
    void Start ()
    {
        //camController = GetComponent<Character_Camera_Controller>();
        //camController.enabled = true;
        cameraInput = 0;
        //Turn all cameras off, except the first default one

        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        //If any cameras were added to the controller, enable the first one
        if (cameras.Length > 0)
        {
            cameras[0].gameObject.SetActive(true);
            Debug.Log("Camera with name: " + cameras[0].GetComponent<Camera>().name + ", is now enabled");
        }

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
        //GetInput();
        SwitchCameraOnPress();
        masterCamera.transform.position = cameras[cameraIndex].transform.position;
        masterCamera.transform.rotation = cameras[cameraIndex].transform.rotation;
	}

    public bool switchPanel()
    {   //This will create an ray that will check the vector down, distance to groun will check the distance, ground will allows to jump
        return Physics.Raycast(transform.position, Vector3.forward, distanceToPanel, CameraSwitchBoard);
    }

    //void GetInput()
    //{
    //    cameraInput = Input.GetAxisRaw(CAMERA_AXIS);
    //}

    void SwitchCameraOnPress()
    {
        //Ray panelRay = new Ray(transform.position, transform.forward);
        //RaycastHit panelHit;
        //if (Physics.Raycast(panelRay, out panelHit, distanceToPanel, CameraSwitchBoard))
        // for (int i = 0; i < cameras.Length; i++)
        // {
        if (Input.GetKeyDown(KeyCode.G))
        {

            cameraIndex++;
            Debug.Log("G button has been pressed. Switching to the next camera");
            //camController.enabled = false;
        }
        if(cameraIndex < cameras.Length)
        {
            //cameras[cameraIndex - 1].gameObject.SetActive(false);
            //cameras[cameraIndex].gameObject.SetActive(true);
            Debug.Log("Camera with name: " + cameras[cameraIndex].GetComponent<Camera>().name + ", is now enabled");
        }
        else
        {
            //cameras[cameraIndex - 1].gameObject.SetActive(false);
            cameraIndex = 0;
            //cameras[cameraIndex].gameObject.SetActive(true);
            Debug.Log("Camera with name: " + cameras[cameraIndex].GetComponent<Camera>().name + ", is now enabled");
        }
          //  }

    } 
}

