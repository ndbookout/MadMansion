using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SampleNavMeshAgent : MonoBehaviour {

    public Transform target;
    public Transform enclosedTarget;
    NavMeshAgent agent;
    public static List<Transform> Targets = new List<Transform>();
    public enum PossibleStates { idle, hasTarget };
    public PossibleStates state = new PossibleStates();
    float maxDistance;


    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        
	}

    //RaycastHit hit;

    //void FixedUpdate()
    //{

    //    Ray NPCRay = new Ray(this.gameObject.transform.position, Vector3.forward);
    //    Physics.Raycast(NPCRay, out hit, maxDistance = 1f);
    //    if (hit.collider.tag == "door")
    //    {
    //        agent.Stop();
    //        StartCoroutine(OpenDoor());
    //        target = enclosedTarget;



    //    }

    //}
    void Update ()
    {
        if ((transform.position - target.position).magnitude < .1 && state != PossibleStates.idle)
        {
            StartCoroutine(DoActions());
        }

        else if (state == PossibleStates.idle)
        {
            target = Targets[Random.Range(0, Targets.Count)];
            state = PossibleStates.hasTarget;
        }

        else if(state == PossibleStates.hasTarget && (transform.position - target.position).magnitude > .1)
            agent.SetDestination(target.position);
       
	}

    IEnumerator DoActions()
    {
        
        yield return new WaitForSeconds(3f);
        state = PossibleStates.idle;

    }

    //IEnumerator OpenDoor()
    //{
    //    target.GetComponent<Animator>().enabled = true;
    //    yield return new WaitForSeconds(2f);
    //}
}
