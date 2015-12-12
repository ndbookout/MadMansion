using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{
    public enum possibleLocations { Hub, ElseWhere };
    public enum possibleStates { findingRoom, traveling, scared, investigating, searchingRoom, waiting};
    public possibleStates state = new possibleStates();
    public possibleLocations location = new possibleLocations();

    private List<GameObject> currentWingRoomList = new List<GameObject>();
    private List<GameObject> currentRoomTargetList = new List<GameObject>();
    private List<GameObject> HubDoors = new List<GameObject>();

    public static float fear = 6;
    public GameObject currentWing;
    NavMeshAgent agent;
    public Transform entrance;
    private Transform target;

    void Start()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        entrance = GameObject.FindGameObjectWithTag("Entrance").transform;
        foreach (GameObject tar in GameObject.FindGameObjectsWithTag("HubDoor"))
        {
            HubDoors.Add(tar);
        }
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
                    doorHit.collider.gameObject.SendMessage("ChangeDoorState");
                    StartCoroutine(WaitForDoorToOpen());
                }
            }
        }

        else if (state == possibleStates.scared)
        {
            state = possibleStates.waiting;
            RunScared();
        }
    }

    void FindNewRoom()
    {
        FindNPCLocation();
        if (location == possibleLocations.Hub)
        {
            if (HubDoors.Count > 0)
            {
                currentWing = HubDoors[Random.Range(0, HubDoors.Count)];
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
            HubDoors.Remove(currentWing);
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

    public void UpdateWingList(List<GameObject> currentWingsList)
    {
        currentWingRoomList.Clear();
        foreach (GameObject room in currentWingsList)
        {
            currentWingRoomList.Add(room);
        }
        Debug.Log(currentWingsList.Count);

    }

    IEnumerator WaitForDoorToOpen()
    {
        state = possibleStates.waiting;
        agent.Stop();
        yield return new WaitForSeconds(2f);
        agent.Resume();
        state = possibleStates.traveling;
    }

    void UpdateRoomList(List<GameObject> targetsInCurrentRoom)
    {
        currentRoomTargetList.Clear();
        foreach (GameObject tar in targetsInCurrentRoom)
        {
            currentRoomTargetList.Add(tar);
        }
        Debug.Log(currentRoomTargetList.Count);
    }
}
