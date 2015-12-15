using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public GameObject thisDoor;
    private Transform thisChild;
    public static bool isDoorOpen = false;
	
	void Start ()
    {
        thisDoor = this.gameObject;
        thisChild = thisDoor.transform.GetChild(0);
	}
	
	
	void Update ()
    {
	    
	}

    void OpenDoor()
    {
        thisDoor.transform.DetachChildren();
        GetComponent<Animator>().enabled = true;
        
    }

    void closeDoor()
    {
        StartCoroutine(CloseDoor());
        thisChild.SetParent(thisDoor.transform);
        TargetController.Targets.Add(thisDoor);
    }
    

    IEnumerator CloseDoor()
    {
        thisDoor.GetComponent<Animator>().SetTrigger("New Trigger");
        yield return new WaitForSeconds(2f);
        thisDoor.GetComponent<Animator>().enabled = false;
    }
}
