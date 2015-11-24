using UnityEngine;
using System.Collections;
using System;

public class AITest : MonoBehaviour {

    Transform NPC;
    public Transform target;
    float moveSpeed = .1f;
    float maxDistance;
    

    void Start ()
    {
        NPC = this.gameObject.transform;
	}

    RaycastHit hit;
   
    void FixedUpdate()
    {
        Ray NPCRay = new Ray(NPC.position, Vector3.forward);
        Physics.Raycast(NPCRay, out hit, maxDistance = 5f);
        if (hit.collider.tag == "Door")
        {
            target = hit.transform;
            Vector3 distanceNPCtoTarget = (NPC.position - target.position);
            if (distanceNPCtoTarget.magnitude > 1)
            {
                NPC.LookAt(target);
                NPC.position = Vector3.Lerp(NPC.position, target.position, Time.deltaTime * moveSpeed);
            }
        }
    }
	void Update ()
    {
        
        
        //target = GameObject.FindWithTag("Door").transform;

        

    }

    
}
