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
        public bool smoothFollow = true;
        public float smooth = 0.05f;

        public float adjustmentDistance = -0.04549421f;

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

    [System.Serializable]
    public class DebugSetting
    {
        public bool drawDesiredCollisionLines = true;
        public bool drawAdjustedCollisionLines = true;
    }

   

    //Variable settings Public
    public Transform target; //This the refrence to the ghost
    public PositionSettings cameraPosition = new PositionSettings();
    public OrbitSetting cameraOrbit = new OrbitSetting();
    public InputSettings cameraInput = new InputSettings();
    public DebugSetting debug = new DebugSetting();
    public CollisionHandler collision = new CollisionHandler();

    //Variable setting private
    Vector3 targetPosition = Vector3.zero;
    Vector3 destination = Vector3.zero;
    Vector3 adjustedDestination = Vector3.zero;
    Vector3 cameraVelocity = Vector3.zero;
    Character_Controller charController;
    //These floats will store our input values
    float verticalOrbitInput;
    float horizontalOrbitInput;
    float zoomInput;
    float horizontalOrbitSnapInput;

    //Methods that we are using in our script

    void Start()
    {

        verticalOrbitInput = 0;
        horizontalOrbitInput = 0;
        zoomInput = 0;
        horizontalOrbitSnapInput = 0;

        SetCameraTarget(target);
        MoveToGhost();

        collision.Initialize(Camera.main);
        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desiredCameraClipPoints);
    }

    void Update()
    {
        GetInput();
        OrbitTarget();
        ZoomInOnTarget();
    }

    void FixedUpdate()
    {   //moving
        MoveToGhost();
        //rotating
        LookAtGhost();
        //player input orbit
        OrbitTarget();

        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desiredCameraClipPoints);

        //drawing debug lines here
        for (int i = 0; i < 5; i++)
        {
            if (debug.drawDesiredCollisionLines)
            {
                Debug.DrawLine(targetPosition, collision.desiredCameraClipPoints[i], Color.white);
            }
            if (debug.drawAdjustedCollisionLines)
            {
                Debug.DrawLine(targetPosition, collision.adjustedCameraClipPoints[i], Color.red);
            }
        }

        collision.CheckColliding(targetPosition); //using raycast
        cameraPosition.adjustmentDistance = collision.GetAdjustedDistanceWithRayFrom(targetPosition);
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

    void MoveToGhost()
    {
        targetPosition = target.position + Vector3.up * cameraPosition.targetPosOffset.y + Vector3.forward * cameraPosition.targetPosOffset.z + transform.TransformDirection(Vector3.right * cameraPosition.targetPosOffset.x);
        destination = Quaternion.Euler(cameraOrbit.xRotation, cameraOrbit.yRotation + target.eulerAngles.y, 0) * -Vector3.forward * cameraPosition.distanceFromTarget;
        destination += targetPosition;

        if (collision.cameraColliding)
        {
            adjustedDestination = Quaternion.Euler(cameraOrbit.xRotation, cameraOrbit.yRotation + target.eulerAngles.y, 0) * Vector3.forward * cameraPosition.adjustmentDistance;
            adjustedDestination += targetPosition;

            if (cameraPosition.smoothFollow)
            {
                //use smooth damp function
                transform.position = Vector3.SmoothDamp(transform.position, adjustedDestination, ref cameraVelocity, cameraPosition.smooth);
            }
            else
                transform.position = adjustedDestination;
        }
        else
        {
            if (cameraPosition.smoothFollow)
            {
                //use smooth damp function
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref cameraVelocity, cameraPosition.smooth);
            }
            else
                transform.position = destination;
        }
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
        cameraPosition.distanceFromTarget += zoomInput * cameraPosition.zoomSmooth * Time.deltaTime;

        if(cameraPosition.distanceFromTarget > cameraPosition.maxZoom)
        {
            cameraPosition.distanceFromTarget = cameraPosition.maxZoom;
        }
        if(cameraPosition.distanceFromTarget < cameraPosition.minZoom)
        {
            cameraPosition.distanceFromTarget = cameraPosition.minZoom;
        }
    }

    [System.Serializable]
    public class CollisionHandler
    {
        public LayerMask collisionLayer;

        public bool cameraColliding = false;
  
        public Vector3[] adjustedCameraClipPoints;

        public Vector3[] desiredCameraClipPoints;

        Camera camera;


        public void Initialize(Camera cam)
        {
            camera = cam;
            adjustedCameraClipPoints = new Vector3[5];
            desiredCameraClipPoints = new Vector3[5];
        }

        public void UpdateCameraClipPoints(Vector3 camPosition, Quaternion atRotation, ref Vector3[] intoArray)
        {
            if (!camera)
                return;
            //clear the contents of intoArray
            intoArray = new Vector3[5];

            float z = camera.nearClipPlane;
            float x = Mathf.Tan(camera.fieldOfView / 3.41f) * z;
            float y = x / camera.aspect;

            //Find the clip points and put them into an array
            //Top left
            intoArray[0] = (atRotation * new Vector3(-x, y, z)) + camPosition; //added and rotated the point relative to camera
            //Top Right
            intoArray[1] = (atRotation * new Vector3(x, y, z)) + camPosition; //added and rotated the point relative to camera
            //Bottom left
            intoArray[2] = (atRotation * new Vector3(-x, -y, z)) + camPosition; //added and rotated the point relative to camera
            //Bottom Right
            intoArray[3] = (atRotation * new Vector3(x, -y, z)) + camPosition;//added and rotated the point relative to camera
            //camera Position
            intoArray[4] = camPosition - camera.transform.forward;
        }

        bool CollisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 fromPosition)
        {
            for (int i = 0; i < clipPoints.Length; i++)
            {
                Ray ray = new Ray(fromPosition, clipPoints[i] - fromPosition);
                float distance = Vector3.Distance(clipPoints[i], fromPosition);
                if(Physics.Raycast(ray, distance, collisionLayer))
                {
                    return true;
                }
            }
            return false;
        }

        public float GetAdjustedDistanceWithRayFrom(Vector3 from)
        {
            float distance = -1;

            for(int i = 0; i < desiredCameraClipPoints.Length; i++)
            {
                Ray ray= new Ray(from, desiredCameraClipPoints[i] - from);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit))
                {
                    if (distance == -1)
                        distance = hit.distance;
                    else
                    {
                        if (hit.distance < distance)
                            distance = hit.distance;
                    }
                }
            }

            if (distance == -1)
                return 0;
            else
                return distance;
        }

        public void CheckColliding(Vector3 targetPosition)
        {
            if(CollisionDetectedAtClipPoints(desiredCameraClipPoints, targetPosition))
            {
                cameraColliding = true;
            }
            else
            {
                cameraColliding = false;
            }
        }

    }

}
