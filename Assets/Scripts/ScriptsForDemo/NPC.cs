using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour
{
    public enum possibleLocations { Hub, ElseWhere };
    public enum possibleStates { findingRoom, traveling, scared, investigating, searchingRoom, waiting, running};
    public possibleStates state = new possibleStates();
    public possibleLocations location = new possibleLocations();

    private List<GameObject> currentWingRoomList = new List<GameObject>();
    private List<GameObject> currentRoomTargetList = new List<GameObject>();
    private List<GameObject> HubDoors = new List<GameObject>();

    public AudioClip scream1;
    public AudioClip scream2;
    private AudioSource humanAudio;
    
    public static float fear = 0;
    public GameObject currentWing;
    NavMeshAgent agent;
    public Transform entrance;
    private Transform target;

    void Start()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        entrance = GameObject.FindGameObjectWithTag("Entrance").transform;
        SetHubDoorList();
        StartCoroutine(FearCooldownControl());

        humanAudio = GetComponent<AudioSource>();
    }

    
    void Update ()
    {
        if (state == possibleStates.findingRoom)
        {
            this.gameObject.GetComponent<Animator>().SetBool("isMoving", true);
            state = possibleStates.traveling;
            FindNewRoom();
        }

        else if (state == possibleStates.investigating)
        {
            state = possibleStates.searchingRoom;
            AcquireTarget();
        }
        else if (state == possibleStates.searchingRoom && (this.transform.position - target.transform.position).magnitude < 3f)
        {
            //run search target method here, if tagged bone, destroy object and add to an inventory
            state = possibleStates.investigating;
            this.gameObject.GetComponent<Animator>().SetBool("isSearching", true);
            StartCoroutine(SearchingObjectInRoom());
            currentRoomTargetList.Remove(target.gameObject);

        }

        else if (state == possibleStates.traveling && (this.transform.position - target.transform.position).magnitude < 2f)
        {
            if (location == possibleLocations.ElseWhere)
            {
                LookAround();
                currentWingRoomList.Remove(target.gameObject);
                state = possibleStates.investigating;
            }
            else
            {
                LookAround();
                state = possibleStates.findingRoom;
            }
        }

        else if (state == possibleStates.traveling)
        {
            RaycastHit doorHit;
            if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.forward, out doorHit, 2f, 1 << 12))
            {
                doorHit.collider.transform.GetComponent<DoorController>().ChangeDoorState();
                StartCoroutine(WaitForDoorToOpen());
            }
        }

        else if (state == possibleStates.scared && (this.transform.position - entrance.position).magnitude < 3)
        {
            SceneManager.LoadScene(2);
            Debug.Log("Your NPC got too scared and ran");
        }

        else if (state == possibleStates.scared)
        {
            state = possibleStates.running;
            RunScared();
        }
    }

    void SetHubDoorList()
    {
        foreach (GameObject tar in GameObject.FindGameObjectsWithTag("HubDoor"))
        {
            HubDoors.Add(tar);
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
                SetHubDoorList();
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
        this.gameObject.GetComponent<Animator>().SetBool("isScared", false);
        state = possibleStates.findingRoom;

    }  

    IEnumerator WaitForDoorToOpen()
    {
        state = possibleStates.waiting;
        
        yield return new WaitForSeconds(1f);
        
        state = possibleStates.traveling;
    }

    public void UpdateRoomList(List<GameObject> targetsInCurrentRoom)
    {
        currentRoomTargetList.Clear();
        foreach (GameObject tar in targetsInCurrentRoom)
        {
            currentRoomTargetList.Add(tar);
        }
        Debug.Log(currentRoomTargetList.Count);
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

    IEnumerator SearchingObjectInRoom()
    {
        agent.Stop();
        
        yield return new WaitForSeconds(1.5f);
        if (target.gameObject.tag == "Bone")
        {
            Debug.Log("You found a bone");
            UI.instance.FindBone();
            currentRoomTargetList.Remove(target.gameObject);
            Destroy(target.gameObject);
            AcquireTarget();
        }
        else if (target.gameObject.tag == "Skull")
        {
            Debug.Log("You found the skull");
            UI.instance.FindSkull();
            currentRoomTargetList.Remove(target.gameObject);
            Destroy(target.gameObject);
            AcquireTarget();
        }
        this.gameObject.GetComponent<Animator>().SetBool("isSearching", false);
        agent.Resume();
    }

    IEnumerator FearCooldownControl()
    {
        if (fear > 0)
        {
            fear--;
        }
        yield return new WaitForSeconds(10f);
        StartCoroutine(FearCooldownControl());
    }

    public void GetScared(float fearIncrease)
    {
        if (humanAudio.isPlaying != true)
        {
            if (fear < 8)
                humanAudio.PlayOneShot(scream1);
            else if (fear >= 8)
                humanAudio.PlayOneShot(scream2);
        }

        this.GetComponent<Animator>().SetBool("isScared", true);
        fear += fearIncrease;
        if (fear > 10)
        {
            fear = 10;
        }
        state = possibleStates.scared;
    }

    IEnumerator LookAround()
    {
        agent.Stop();
        this.GetComponent<Animator>().SetBool("isMoving", false);
        yield return new WaitForSeconds(1.5f);
        this.GetComponent<Animator>().SetBool("isMoving", true);
        agent.Resume();

    }
}
