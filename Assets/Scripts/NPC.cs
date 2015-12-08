using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{
    public enum possibleLocations { Hub, ElseWhere };
    public enum possibleStates { findingRoom, traveling, scared, investigating, searchingRoom, running};
    public possibleStates state = new possibleStates();
    public possibleLocations location = new possibleLocations();

    private List<GameObject> currentWingRoomList = new List<GameObject>();
    private List<GameObject> currentRoomTargetList = new List<GameObject>();

    public static float fear = 6;
    public GameObject currentWing;
    NavMeshAgent agent;
    public Transform entrance;
    private Transform target;

    void Start()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        entrance = GameObject.FindGameObjectWithTag("Entrance").transform;
    }

    
    void Update ()
    {
        if (state == possibleStates.findingRoom)
        {
            state = possibleStates.traveling;
            FindNewRoom();
        }

        else if (state == possibleStates.investigating)
        {
            state = possibleStates.searchingRoom;
            AcquireTarget();
        }
        else if (state == possibleStates.searchingRoom && (this.transform.position - target.transform.position).magnitude < 1.5f)
        {
            //run search target method here
            currentRoomTargetList.Remove(target.gameObject);
            state = possibleStates.investigating;
        }

        else if (state == possibleStates.traveling && (this.transform.position - target.transform.position).magnitude < 1.5f)
        {
            if (location == possibleLocations.ElseWhere)
            {
                currentWingRoomList.Remove(target.gameObject);
                state = possibleStates.investigating;
            }
            else
            {
                state = possibleStates.findingRoom;
            }
        }

        else if (state == possibleStates.traveling)
        {
            RaycastHit doorHit;
            if(Physics.Raycast(this.transform.position, target.transform.position, out doorHit, 2f))
            {
                if (doorHit.collider.tag == "Door")
                {
                    agent.Stop();
                    //run opendoor method
                    agent.Resume();
                }
            }
        }

        else if (state == possibleStates.scared)
        {
            state = possibleStates.running;
            RunScared();
        }
    }

    void FindNewRoom()
    {
        FindNPCLocation();
        if (location == possibleLocations.Hub)
        {
            if (UpdatedTargetController.HubDoors.Count > 0)
            {
                currentWing = UpdatedTargetController.HubDoors[Random.Range(0, UpdatedTargetController.HubDoors.Count)];
                target = currentWing.transform;
                Travel(target);
            }
            else
            {
                //load lose state
            }
        }
        else if (currentWingRoomList.Count > 0)
        {
            target = currentWingRoomList[Random.Range(0, currentWingRoomList.Count)].transform;
            Travel(target);
        }

        else
        {
            UpdatedTargetController.HubDoors.Remove(currentWing);
            target = entrance;
            Travel(target);
        }
    }

    void AcquireTarget()
    {
        if (currentRoomTargetList.Count > 0)
        {
            target = currentRoomTargetList[Random.Range(0, currentRoomTargetList.Count)].transform;
            Travel(target);
        }
        else
        {
            state = possibleStates.findingRoom;
        }
    }

    private void Travel(Transform targetDestination)
    {
        agent.SetDestination(targetDestination.position);
    }

    void FindNPCLocation()
    {
        RaycastHit floorHit;

        
        if (Physics.Raycast(this.transform.position, Vector3.down, out floorHit))
        {

            if (floorHit.collider.tag == "Hub")
            {
                location = possibleLocations.Hub;
            }

            else
            {
                location = possibleLocations.ElseWhere;
            }
        }
    }

    private void RunScared()
    {
        
        
        StartCoroutine(RunningScared(entrance));

    }

    IEnumerator RunningScared(Transform entranceDoor)
    {
        agent.SetDestination(entranceDoor.position);
        
        agent.speed = 10;
        yield return new WaitForSeconds(fear);
        agent.speed = 3.5f;
        state = possibleStates.findingRoom;

    }

    void UpdateWingList(List<GameObject> currentWingsList)
    {
        currentWingRoomList.Clear();
        foreach (GameObject room in currentWingsList)
        {
            currentWingRoomList.Add(room);
        }

    }

    void UpdateRoomList(List<GameObject> targetsInCurrentRoom)
    {
        currentRoomTargetList.Clear();
        foreach (GameObject tar in targetsInCurrentRoom)
        {
            currentRoomTargetList.Add(tar);
        }
    }
}
