using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ghoul : MonoBehaviour {

    NavMeshAgent ghoulAgent;
    private List<GameObject> allTargets = new List<GameObject>();
    private enum ghoulStates { GettingNewTarget, Wandering, ChasingNPC};
    private ghoulStates ghoulState = new ghoulStates();
    private Transform ghoulTarget;
    public GameObject npc;

	void Start ()
    {
        ghoulAgent = this.gameObject.GetComponent<NavMeshAgent>();
        foreach (GameObject tar in GameObject.FindGameObjectsWithTag("Target"))
        {
            allTargets.Add(tar);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (ghoulState == ghoulStates.GettingNewTarget)
        {
            ghoulState = ghoulStates.Wandering;
            AcquireTarget();
        }

        else if (ghoulState == ghoulStates.Wandering && (this.transform.position - ghoulTarget.transform.position).magnitude < 3)
        {
            ghoulState = ghoulStates.GettingNewTarget;
        }

        else if (ghoulState == ghoulStates.Wandering)
        {
            RaycastHit npcHit;
            if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.forward, out npcHit, 5f, 1 << 15))
            {
                ghoulState = ghoulStates.ChasingNPC;
                npc = npcHit.collider.gameObject;
                npc.gameObject.GetComponent<NPC>().GetScared(5f);
            }
        }

        else if (ghoulState == ghoulStates.ChasingNPC && (this.transform.position - npc.transform.position).magnitude > 15)
        {
            ghoulState = ghoulStates.GettingNewTarget;
        }

        else if (ghoulState == ghoulStates.ChasingNPC && (this.transform.position - npc.transform.position).magnitude < 2)
        {
            //lose state
            Debug.Log("Your NPC got eaten");
        }
        else if (ghoulState == ghoulStates.ChasingNPC)
        {
            ChaseNpc(npc);
        }
	}

    void AcquireTarget()
    {
        ghoulTarget = allTargets[Random.Range(0, allTargets.Count)].transform;
    }

    void Travel(Transform tar)
    {
        ghoulAgent.SetDestination(tar.position);
    }
    void ChaseNpc(GameObject npc)
    {
        ghoulAgent.SetDestination(npc.transform.position);
    }
}
