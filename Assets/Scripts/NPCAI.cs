using UnityEngine;
using System.Collections;

public class NPCAI : MonoBehaviour
{

    public enum possibleLocations { NorthWing, WestWing, Hub, SouthWing};
    public enum possibleStates {findingTarget, traveling, scared, investigating};
    public possibleStates state = new possibleStates();
    public possibleLocations location = new possibleLocations();
    NavMeshAgent agent;
    public Transform target;
    public float fear;
    public Transform scaredTarget;
    

	void Start ()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
	}


    void Update()
    {
        if (state == possibleStates.findingTarget)
        {
            state = possibleStates.traveling;
            AcquireTarget();
        }

        else if (state == possibleStates.scared)
        {
            RunScared();
        }

        else if (state == possibleStates.traveling && (this.transform.position - target.transform.position).magnitude < 1.5f)
        {
            state = possibleStates.findingTarget;
        }
    }
    void FindNPCLocation()
    {
        RaycastHit floorHit;
        
        Ray FloorRay = new Ray(this.transform.position, this.transform.position + new Vector3(0, -5));
        if (Physics.Raycast(this.transform.position, Vector3.down, out floorHit))
        {
            
            if (floorHit.collider.tag == "Hub")
            {
                location = possibleLocations.Hub;
            }

            else if (floorHit.collider.tag == "NorthWing")
            {
                location = possibleLocations.NorthWing;
            }

            else if (floorHit.collider.tag == "SouthWing")
            {
                location = possibleLocations.SouthWing;
            }

            else if (floorHit.collider.tag == "WestWing")
            {
                location = possibleLocations.WestWing;
            }
        }
    }        
	
	private void AcquireTarget ()
    {
        
        FindNPCLocation();
	    if (location == possibleLocations.Hub)
        {
            
            target = UpdatedTargetController.HubDoors[Random.Range(0, UpdatedTargetController.HubDoors.Count)].transform;
            
            Travel(target);
        }

        else if (location == possibleLocations.NorthWing)
        {
            target = UpdatedTargetController.NorthWingDoors[Random.Range(0, UpdatedTargetController.NorthWingDoors.Count)].transform;
            
            Travel(target);
        }

        else if (location == possibleLocations.WestWing)
        {
            target = UpdatedTargetController.WestWingDoors[Random.Range(0, UpdatedTargetController.WestWingDoors.Count)].transform;
            
            Travel(target);
        }

        else if (location == possibleLocations.SouthWing)
        {
            target = UpdatedTargetController.SouthWingDoors[Random.Range(0, UpdatedTargetController.SouthWingDoors.Count)].transform;
            
            Travel(target);
        }
    }

    private void Travel(Transform targetDestination)
    {
        agent.SetDestination(targetDestination.position);
    }

    private void RunScared()
    {
       
        scaredTarget = GameObject.FindGameObjectWithTag("Entrance").transform;
        StartCoroutine(RunningScared(scaredTarget));
        
    }

    IEnumerator RunningScared(Transform entranceDoor)
    {
        agent.SetDestination(entranceDoor.position);
        agent.speed = 10;
        yield return new WaitForSeconds(fear);
        agent.speed = 3.5f;
        state = possibleStates.findingTarget;

    }
}
