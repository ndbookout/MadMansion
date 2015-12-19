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

    public AudioClip[] noises;
    private AudioSource ghoulAudio;
    private Task noise;

	void Start ()
    {
        ghoulAgent = this.gameObject.GetComponent<NavMeshAgent>();
        foreach (GameObject tar in GameObject.FindGameObjectsWithTag("Target"))
        {
            allTargets.Add(tar);
        }

        ghoulAudio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        NoiseMaker();        

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
            if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.forward, 5f, 1 << 12))
            {
                return;
            }
            else
            {
                if (Physics.SphereCast(transform.position + new Vector3(0, 1, 0), transform.lossyScale.magnitude * 2, transform.forward, out npcHit, 5f, 1 << 15))
                {
                    ghoulState = ghoulStates.ChasingNPC;
                    npc = npcHit.collider.gameObject;
                    npc.gameObject.GetComponent<NPC>().GetScared(5f);
                }
            }           
        }

        else if (ghoulState == ghoulStates.ChasingNPC && (this.transform.position - npc.transform.position).magnitude > 12)
        {
            ghoulState = ghoulStates.GettingNewTarget;
        }

        else if (ghoulState == ghoulStates.ChasingNPC && (this.transform.position - npc.transform.position).magnitude < 2)
        {
            //lose state
            RaycastHit npcHit;
            if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.forward, out npcHit, 5f, 1 << 15))
            {
                ghoulState = ghoulStates.ChasingNPC;
                npc = npcHit.collider.gameObject;
                npc.gameObject.GetComponent<NPC>().GetScared(5f);
            }
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
        Wander(ghoulTarget);
    }

    void Wander(Transform tar)
    {
        ghoulAgent.SetDestination(tar.position);
        this.GetComponent<Animator>().SetBool("isMoving", true);
    }
    void ChaseNpc(GameObject npc)
    {
        ghoulAgent.SetDestination(npc.transform.position);
    }

    void NoiseMaker()
    {
        if (noise == null)
            noise = new Task(MakeNoises());
        else if (noise != null)
        {
            if (noise.Running)
                return;
            else
                noise = new Task(MakeNoises());
        }
    }

    IEnumerator MakeNoises()
    {
        int rand = Random.Range(0, noises.Length);
        yield return new WaitForSeconds(rand + 3);
        if (ghoulAudio.isPlaying != true)
        {
            ghoulAudio.PlayOneShot(noises[rand]);
        }
        yield return new WaitForSeconds(rand + 5);
        if (ghoulAudio.isPlaying != true)
        {
            ghoulAudio.PlayOneShot(noises[rand]);
        }
        yield return new WaitForSeconds(rand + 4);
        if (ghoulAudio.isPlaying != true)
        {
            ghoulAudio.PlayOneShot(noises[rand]);
        }
    }

    public IEnumerator Kill()
    {
        if (Random.Range(0, 2) <= 1)
            ghoulAudio.PlayOneShot(noises[4]);
        else
            ghoulAudio.PlayOneShot(noises[5]);

        yield return new WaitForSeconds(1);

        if (noise != null)
            noise.Stop();
        Destroy(gameObject);
    }
}
