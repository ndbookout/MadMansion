﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HumanAI : MonoBehaviour
{
    private GameObject target;
    private GameObject prevTarget;
    private Transform enclosedTarget;

    Ray NPCRay;
    RaycastHit hit;
    NavMeshAgent agent;
    int doorLayer = 1 << 12;

    public enum PossibleStates { idle, hasTarget, scared, running };
    public PossibleStates state = new PossibleStates();
    public float maxDistance;
    public GameObject entrance;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
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

        if (state == PossibleStates.scared)
        {
            //target = entrance;
            StartCoroutine(RunScared());
        }

        else if ((transform.position - target.transform.position).magnitude < 1 && state != PossibleStates.idle)
        {
            StartCoroutine(DoActions());
        }
        else if (state == PossibleStates.hasTarget && (transform.position - target.transform.position).magnitude > 1)
        {
            agent.SetDestination(target.transform.position);
        }
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            NPCRay = new Ray(gameObject.transform.position, gameObject.transform.forward);
            //Physics.Raycast(NPCRay, out hit, maxDistance);
            Debug.Log(Physics.Raycast(NPCRay, maxDistance, doorLayer));
            Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward);

            if (Physics.Raycast(NPCRay, out hit, maxDistance, doorLayer))
            {
                if (hit.collider.gameObject.GetComponent<DoorController>() != null)
                {
                    agent.Stop();
                    TargetController.Targets.Remove(target);

                    DoorController door = hit.collider.gameObject.GetComponent<DoorController>();
                    door.ChangeDoorState();         
                    target = door.roomTargets[0];

                    StartCoroutine(OpenDoor());
                }
            }
        }
    }

    void OnTriggerEnter(Collider collide)
    {
        if (collide.name == "Player")
            state = PossibleStates.scared;
    }

    IEnumerator DoActions()
    {
        state = PossibleStates.idle;
        yield return new WaitForSeconds(3f);   
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
            
            //target = null;
            state = PossibleStates.hasTarget;
            agent.Resume();
        //}
        //else
        //{
        //    yield return new WaitForSeconds(1f);
        //    target = enclosedTarget;
        //    agent.Resume();
        //}
    }

    IEnumerator RunScared()
    {
        state = PossibleStates.running;
        yield return new WaitForSeconds(3f);      
        state = PossibleStates.idle;
    }
}

