using UnityEngine;
using System.Collections;

public class Character_Controller : MonoBehaviour {
    //These are our public serializable setting to control from the inspector in Unity
    [System.Serializable]
    public class MoveSettings // This will enclose our movement 
    {
        public float fowardVelocity = 12; //This is the velocity of the foward movement of the character
        public float rotateVelocity = 100; //This is the rotation velocity of the character
        public float jumpVelocity = 25; //This is the jump velocity of the character in the upward direction
        public float distanceToGrounded = 0.1f; //This is the total distance the character is to the ground this will be a raycast that will return a distance this will be a bool check
        public Transform[] groundCheckPoints;
        public LayerMask ground; //This will check the the layer and what specific object he can jump from
    }

    [System.Serializable]
    public class PhysSettings //These will encapsulate our physic variables
    {
        public float downwardAcceleration = 0.75f;
    }

    [System.Serializable]
    public class InputSettings //These will ecapsualte our input settings
    {
        public float inputDelay = 0.1f;
        public string FOWARD_AXIS = "Vertical";
        public string TURN_AXIS = "Horizontal";
        public string JUMP_AXIS = "Jump";
    }


    //These are the refrences to the classes this will make the classes modifiable from the inspector
    public MoveSettings moveSetting = new MoveSettings();
    public PhysSettings physSetting = new PhysSettings();
    public InputSettings inputSetting = new InputSettings();

    //Variable Settings Private
    Vector3 ghostVelocity = Vector3.zero;
    Vector3 terrainNormal = Vector3.zero;
    private Quaternion targetRotation; //Holds rotation to turn too
    private Rigidbody ghostBody; //This will hold the ghost postion from the rigidbody
    private float forwardInput; 
    private float turnInput;
    private float jumpInput;

    private Animator ghostAnim;


    //Methods that we are using in our script
    void Start()
    {
        forwardInput = 0; //When we start our game the character will be at 0 movement
        turnInput = 0; //When we start our game the character will have 0 rotation applied
        jumpInput = 0;
        targetRotation = transform.rotation;// This set the character rotation to the object the script is applied too

        ghostBody = GetComponent<Rigidbody>();
        ghostAnim = GetComponent<Animator>();
    }

    void Update()
    {
        GetInput();
        
    }

    void FixedUpdate()
    {
        Run();
        Jump();
        Turn();

        Animate();

        ghostBody.velocity = transform.TransformDirection(ghostVelocity);
        //ghostBody.velocity = ghostVelocity;
    } 

    void GetInput()
    {
        forwardInput = Input.GetAxis(inputSetting.FOWARD_AXIS);//interpolated will get any value from -1 to 1
        turnInput = Input.GetAxis(inputSetting.TURN_AXIS);//interpolated will get any value from -1 to 1
        jumpInput = Input.GetAxisRaw(inputSetting.JUMP_AXIS);//non-interpolate will only get values -1, 0, or 1 we only care that the value is greather than 0 because that will let us know that the player press the space bar
    }

    void Animate()
    {
        ghostAnim.SetFloat("Forward", forwardInput);
    }

    void Run()
    {
        if (Mathf.Abs(forwardInput) > inputSetting.inputDelay) //This is checking that our foward movement is greater than 0.1
        {
            //move
            ghostVelocity.z = moveSetting.fowardVelocity * forwardInput;//So to move foward we only care about the .z vector of the character 
        }
        else
            //zero velocity
            ghostVelocity.z = 0; //This vector was create to modify only the axis we only care about base on the input
    }
    public Quaternion TargetRotation // This will allow us to access the rotation from our camera controller so that we can base the camera rotation of the character rotation
    {
        get { return targetRotation; }
    }

    void Turn()
    {
        if (Mathf.Abs(turnInput) > inputSetting.inputDelay)
        {
            targetRotation *= Quaternion.AngleAxis(moveSetting.rotateVelocity * turnInput * Time.deltaTime, Vector3.up);
        }
            transform.rotation = targetRotation;
    }

    public bool Grounded()
    {   //This will create an ray that will check the vector down, distance to groun will check the distance, ground will allows to jump
        return Physics.Raycast(transform.position, Vector3.down, moveSetting.distanceToGrounded, moveSetting.ground);
    }

    void Jump()
    {
        if (jumpInput > 0 && Grounded())
        {
            //jump
            ghostVelocity.y = moveSetting.jumpVelocity;
        }
        else if (jumpInput == 0 && Grounded())
        {
            //zero out our veloctity.y
            ghostVelocity.y = 0;
        }
        else
        {
            //decrease velocity.y
            ghostVelocity.y -= physSetting.downwardAcceleration;
        }
    }

}
