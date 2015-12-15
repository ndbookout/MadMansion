using UnityEngine;
using System.Collections;

public class DoorOpeningScript : MonoBehaviour
{
    public float doorInteractDistance = 5f;

    private int doorMask = 1 << 12;

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.DrawRay(transform.position + new Vector3(0, 1, 0), transform.forward, Color.red, 5f);

            RaycastHit doorHit;
            if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.forward, out doorHit, doorInteractDistance, doorMask))
            {
                doorHit.collider.transform.GetComponent<DoorController>().ChangeDoorState();
                Debug.Log("I SEE YOU DOOR!");
            }
            else
                Debug.Log("I can't see the door...");
        }
	}
}
