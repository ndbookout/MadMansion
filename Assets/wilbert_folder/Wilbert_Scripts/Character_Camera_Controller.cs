using UnityEngine;
using System.Collections;

public class Character_Camera_Controller : MonoBehaviour {
    //Serializable Classes that will be using for the camera
    [System.Serializable]
    public class PositionSettings
    {
        public Vector3 targetPosOffset = new Vector3(0, 3.4f, 0); //This will create an artificial orgin to the character we can set our camera to look at
        public float lookSmooth = 100f;//This will allow the camera to rotate smoothly around the character when rotating
        public float distanceFromTarget = -8;//this will modify the distance from target when zooming in
        public float zoomSmooth = 100;//This is how fast we will zoom in, to the target
        public float maxZoom = -2;//These are a zoom treash holds so how close and how far we can get from the target
        public float minZoom = -15;
    }

    [System.Serializable]
    public class OrbitSetting
    {
        public float xRotation = 20;//This will modify the rotation of the camera through input on the x and y axis
        public float yRotation = -180;
        public float maxXRotaion = 25;//This is the rotation on the vertical axis this will make sure this wont go below the character and max rotation that wont pass the character
        public float minXRotation = -85;
        public float verticalOrbitSmooth = 150;//This will be the rotation on the x and y axis and how smooth it will rotate
        public float horizontalOrbitSmooth = 150;
    }

    [System.Serializable]
    public class InputSettings
    {
        public string ORBIT_HORIZONTAL_SNAP = "OrbitHorizontalSnap";// snap the y rotation to the back of the target
        public string ORBIT_HORIZONTAL = "OrbitHorizontal";// rotates the camera on the y axis
        public string ORBIT_VERTICAL = "OrbitVertical"; //rotates the camera on the x axis
        public string ZOOM = "Mouse ScrollWheel";//zoom in and out of the target
    }

    //Variable settings Public
    public Transform target; //This the refrence to the ghost
    public PositionSettings cameraPosition = new PositionSettings();
    public OrbitSetting cameraOrbit = new OrbitSetting();
    public InputSettings cameraInput = new InputSettings();

    //Variable setting private
    Vector3 targetPosition = Vector3.zero;
    Vector3 destination = Vector3.zero;
    Character_Controller charController;
    //These floats will store our input values
    float verticalOrbitInput;
    float horizontalOrbitInput;
    float zoomInput;
    float horizontalOrbitSnapInput;

    //Methods that we are using in our script

    void Start()
    {
        SetCameraTarget(target);

        targetPosition = target.position + cameraPosition.targetPosOffset;
        destination = Quaternion.Euler(cameraOrbit.xRotation, cameraOrbit.yRotation + target.eulerAngles.y, 0) * -Vector3.forward * cameraPosition.distanceFromTarget;
        destination += targetPosition;
        transform.position = destination;
    }

    void Update()
    {
        GetInput();
        OrbitTarget();
        ZoomInOnTarget();
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
        targetPosition = target.position + cameraPosition.targetPosOffset;
        destination = Quaternion.Euler(cameraOrbit.xRotation, cameraOrbit.yRotation + target.eulerAngles.y, 0) * -Vector3.forward * cameraPosition.distanceFromTarget;
        destination += targetPosition;
        transform.position = destination;
    }

    void LookAtGhost()
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, cameraPosition.lookSmooth * Time.deltaTime);
    }
    void GetInput()
    {
        verticalOrbitInput = Input.GetAxisRaw(cameraInput.ORBIT_VERTICAL);
        horizontalOrbitInput = Input.GetAxisRaw(cameraInput.ORBIT_HORIZONTAL);
        horizontalOrbitSnapInput = Input.GetAxisRaw(cameraInput.ORBIT_HORIZONTAL_SNAP);
        zoomInput = Input.GetAxisRaw(cameraInput.ZOOM);
    }
    void OrbitTarget()
    {
        if(horizontalOrbitSnapInput > 0)
        {
            cameraOrbit.yRotation = -180;
        }

        cameraOrbit.xRotation += -verticalOrbitInput * cameraOrbit.verticalOrbitSmooth * Time.deltaTime;
        cameraOrbit.yRotation += -horizontalOrbitInput * cameraOrbit.horizontalOrbitSmooth * Time.deltaTime;

        if(cameraOrbit.xRotation > cameraOrbit.maxXRotaion)
        {
            cameraOrbit.xRotation = cameraOrbit.maxXRotaion;
        }
        if (cameraOrbit.xRotation < cameraOrbit.minXRotation)
        {
            cameraOrbit.xRotation = cameraOrbit.minXRotation;
        }
    }
    void ZoomInOnTarget()
    {

    }


}
