using UnityEngine;
using System.Collections;

//Controls Player Ghost Actions
public class PlayerGhost : MonoBehaviour
{
    private Ray ghostRay;
    private RaycastHit ghostHit;

    public float InteractDistance = 5f;
    private int interactMask = 1 << 17;
    private int doorMask = 1 << 12;
    private int humanMask = 1 << 15;
    private int enemyMask = 1 << 16;

    // Update is called once per frame
    void Update ()
    {
        ghostRay = new Ray(transform.position + new Vector3(0, 1, 0), transform.forward);

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.DrawRay(transform.position + new Vector3(0, 1, 0), transform.forward, Color.red, 5f);

            OpenDoor();
            ScareHuman();
            AttackEnemy();
        }
    }

    void OpenDoor()
    {
        if (Physics.Raycast(ghostRay, out ghostHit, InteractDistance, doorMask))
        {
            ghostHit.collider.transform.GetComponent<DoorController>().ChangeDoorState();
            Debug.Log("I SEE YOU DOOR!");
        }
        else
            Debug.Log("I can't see the door...");
    }

    void ScareHuman()
    {
        if (Physics.Raycast(ghostRay, out ghostHit, InteractDistance, humanMask))
        {
            ghostHit.collider.transform.GetComponent<NPC>().GetScared(2);
            Debug.Log("RUN YOU LITTLE CREEP, RUN!");
        }
        else
            Debug.Log("No one to scare :(");
    }

    void AttackEnemy()
    {
        if (Physics.Raycast(ghostRay, out ghostHit, InteractDistance, enemyMask))
        {

        }
    }
}
