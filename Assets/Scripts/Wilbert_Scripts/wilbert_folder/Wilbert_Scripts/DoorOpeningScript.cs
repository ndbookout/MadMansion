using UnityEngine;
using System.Collections;

public class DoorOpeningScript : MonoBehaviour
{
    public float doorInteractDistance = 5f;
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Ray doorRay = new Ray(transform.position, transform.forward);
            RaycastHit doorHit;
            if(Physics.Raycast(doorRay,out doorHit, doorInteractDistance))
            {
                if (doorHit.collider.CompareTag("Door"))
                {
                    doorHit.collider.transform.parent.GetComponent<DoorController>().ChangeDoorState();
                }
            }
        }
	}
}
