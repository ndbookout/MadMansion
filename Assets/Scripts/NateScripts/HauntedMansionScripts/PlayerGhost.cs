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

    private Animator ghostAnim;

    void Start()
    {
        ghostAnim = GetComponent<Animator>();

        actionMask = doorMask | humanMask | enemyMask;
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        ghostRay = new Ray(transform.position + new Vector3(0, 1, 0), transform.forward);
        Debug.DrawRay(transform.position + new Vector3(0, 1, 0), transform.forward, Color.red, 5f);

        if (Physics.Raycast(ghostRay, InteractDistance, actionMask))
        {
            UI.instance.ToggleActionIcon(true);
            if (Input.GetKeyDown(KeyCode.X))
            {
                OpenDoor();
                ScareHuman();
                AttackEnemy();
            }
        }
        else
        {
            UI.instance.ToggleActionIcon(false);
        }
            
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

    void AttackEnemy()
    {
        if (Physics.Raycast(ghostRay, out ghostHit, InteractDistance, enemyMask))
        {
            new Task(AnimateActions(("Attack"), ghostHit.collider));
        }
    }

    IEnumerator AnimateActions(string action, Collider other)
    {
        if (action == "Scare")
        {
            ghostAnim.SetBool("Scare", true);
            yield return new WaitForSeconds(0.5f);
            ghostAnim.SetBool("Scare", false);
            other.transform.GetComponent<NPC>().GetScared(2);
        }
        else if (action == "Attack")
        {
            ghostAnim.SetBool("Attack", true);
            yield return new WaitForSeconds(0.5f);
            ghostAnim.SetBool("Attack", false);
        }
        else if (action == "Door")
        {
            ghostAnim.SetBool("Attack", true);
            yield return new WaitForSeconds(0.5f);
            ghostAnim.SetBool("Attack", false);
            other.transform.GetComponent<DoorController>().ChangeDoorState();
        }
    }
}
