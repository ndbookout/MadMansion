using UnityEngine;
using System.Collections;

//Controls Player Ghost Actions
public class PlayerGhost : MonoBehaviour
{
    private Ray ghostRay;
    private RaycastHit ghostHit;

    public float InteractDistance = 5f;
    private int doorMask = 1 << 12;
    private int humanMask = 1 << 15;
    private int enemyMask = 1 << 16;
    private int actionMask;

    public AudioClip scare;
    private AudioSource ghostAudio;
    private Animator ghostAnim;
    private bool actionTaken;

    public GameObject fireBlast;
    private bool fired;

    void Start()
    {
        ghostAnim = GetComponent<Animator>();
        ghostAudio = GetComponent<AudioSource>();
        actionMask = doorMask | humanMask;
    }

    // Update handles raycasting
    void Update ()
    {
        ghostRay = new Ray(transform.position + new Vector3(0, 1, 0), transform.forward);
        Debug.DrawRay(transform.position + new Vector3(0, 1, 0), transform.forward, Color.red, 5f);

        //Actions
        if (Physics.Raycast(ghostRay, InteractDistance, actionMask))
        {
            UI.instance.ToggleActionIcon(true);
            if (Input.GetKeyDown(KeyCode.X) && actionTaken == false)
            {
                actionTaken = true;
                OpenDoor();
                ScareHuman();             
            }
        }
        else
        {
            UI.instance.ToggleActionIcon(false);
        }

        //Attack
        if (Input.GetKeyDown(KeyCode.F) && fired == false)
        {
            fired = true;
            new Task(AnimateActions(("Attack"), ghostHit.collider));
        }
        if (Physics.Raycast(ghostRay, out ghostHit, InteractDistance, enemyMask))
            UI.instance.ToggleAttackIcon(true);
        else
            UI.instance.ToggleAttackIcon(false);           
    }

    void OpenDoor()
    {
        if (Physics.Raycast(ghostRay, out ghostHit, InteractDistance, doorMask))
        {
            new Task(AnimateActions(("Door"), ghostHit.collider));         
            Debug.Log("I SEE YOU DOOR!");          
        }
        else
            Debug.Log("I can't see the door...");
    }

    void ScareHuman()
    {
        if (Physics.Raycast(ghostRay, out ghostHit, InteractDistance, humanMask))
        {     
            Debug.Log("RUN YOU LITTLE CREEP, RUN!");

            new Task(AnimateActions("Scare", ghostHit.collider));
        }
        else
            Debug.Log("No one to scare :(");
    }

    IEnumerator AnimateActions(string action, Collider other)
    {      
        if (action == "Scare")
        {
            ghostAudio.PlayOneShot(scare);
            ghostAnim.SetBool("Scare", true);
            yield return new WaitForSeconds(0.5f);
            ghostAnim.SetBool("Scare", false);
            other.transform.GetComponent<NPC>().GetScared(2);
            //Debug.Log("BOO!");
        }
        else if (action == "Attack")
        {       
            ghostAnim.SetBool("Attack", true);
            yield return new WaitForSeconds(0.5f);
            ghostAnim.SetBool("Attack", false);
            new Task(FireBlast());
        }
        else if (action == "Door")
        {
            ghostAnim.SetBool("Attack", true);
            yield return new WaitForSeconds(0.5f);
            ghostAnim.SetBool("Attack", false);
            other.transform.GetComponent<DoorController>().ChangeDoorState();
        }

        actionTaken = false;
    }

    IEnumerator FireBlast()
    {
        GameObject fire = Instantiate(fireBlast, 
            transform.position + new Vector3(0, 1, transform.forward.z), Quaternion.identity) as GameObject;
        fire.transform.rotation = transform.rotation;
        yield return new WaitForSeconds(2);
        fired = false;
        Destroy(fire);     
    }
}
