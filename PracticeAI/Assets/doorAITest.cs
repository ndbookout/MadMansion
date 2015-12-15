using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class doorAITest : MonoBehaviour
{
    public GameObject target;
    private GameObject prevTarget;
    public Transform enclosedTarget;
    NavMeshAgent agent;
    //public static List<GameObject> Targets = new List<GameObject>();
    public enum PossibleStates { idle, hasTarget, scared, inRoom };
    public PossibleStates state = new PossibleStates();
    float maxDistance;
    public GameObject entrance;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //foreach (GameObject tar in GameObject.FindGameObjectsWithTag("target"))
        //{
        //    Targets.Add(tar);
        //}

        //foreach (GameObject tar in GameObject.FindGameObjectsWithTag("Door"))
        //{
        //    Targets.Add(tar);
        //}
    }

    RaycastHit hit;

    void FixedUpdate()
    {
        if (target.tag == "Door")
        {
            Ray NPCRay = new Ray(gameObject.transform.position, target.transform.position/*this.gameObject.transform.position + new Vector3(3,0,0)*/);
            Physics.Raycast(NPCRay, out hit, maxDistance = 3f);
            if (hit.collider.tag == "Door")
            {
                agent.Stop();
                if (hit.transform.GetChild(0) != null)
                {
                    enclosedTarget = hit.transform.GetChild(0);
                }
                TargetController.Targets.Remove(target);
                hit.transform.SendMessageUpwards("OpenDoor");
                StartCoroutine(OpenDoor());



            }
        }

    }
    void Update()
    {
        if (state == PossibleStates.idle)
        {
            if (target == null)
            {

                target = TargetController.Targets[Random.Range(0, TargetController.Targets.Count)];
            }

            else if (target != null && target != prevTarget)
            {
                state = PossibleStates.hasTarget;
            }

            else
            {
                target = TargetController.Targets[Random.Range(0, TargetController.Targets.Count)];
            }



        }

        //if (state == PossibleStates.scared)
        //      {
        //         target = entrance;
        //    StartCoroutine(RunScared());
        //      }

        else if ((transform.position - target.transform.position).magnitude < 1 && state != PossibleStates.idle)
        {
            StartCoroutine(DoActions());
        }


        else if (state == PossibleStates.hasTarget && (transform.position - target.transform.position).magnitude > 1)
        {
            agent.SetDestination(target.transform.position);
        }

        else if (state == PossibleStates.inRoom)
        {
            //agent.Resume();
            if ((transform.position - enclosedTarget.position).magnitude > 1)
            {
                agent.SetDestination(enclosedTarget.position);

            }

            else
                StartCoroutine(DoActions());
        }


    }

    IEnumerator DoActions()
    {
        
        yield return new WaitForSeconds(3f);
        state = PossibleStates.idle;
        prevTarget = target;
        target = null;

    }

    IEnumerator OpenDoor()
    {
        //if (target.GetComponent<Animator>().enabled == false)
        //{
            //enclosedTarget = target.transform.GetChild(0);
            //target.transform.DetachChildren();
            //target.GetComponent<Animator>().enabled = true;
            yield return new WaitForSeconds(2f);
            TargetController.Targets.Remove(target);
            //target = null;
            state = PossibleStates.inRoom;
            agent.Resume();
        //}
        //else
        //{
        //    yield return new WaitForSeconds(1f);
        //    target = enclosedTarget;
        //    agent.Resume();
        //}
    }

    //IEnumerator RunScared()
    //{
    //    yield return new WaitForSeconds(5f);
    //    target = Targets[Random.Range(0, Targets.Count)];
    //    state = PossibleStates.idle;
    //}
}

