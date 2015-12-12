using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WingList : MonoBehaviour
{
    private List<GameObject> roomList;

    void Start()
    {
        roomList = new List<GameObject>();
    }

    void Update()
    {
        Debug.Log("Rooms in wing: " + roomList.Count);
    }

    void OnTriggerEnter(Collider collision)
    {    
        if (collision.gameObject.tag == "NPC")
        {
            collision.gameObject.GetComponent<NPC>().UpdateWingList(roomList);
            //npc.SendMessage("UpdateWingList", roomList);
        }

        if (collision.gameObject.tag == "Room")
        {
            if (!roomList.Contains(collision.gameObject))
                roomList.Add(collision.gameObject);
        }
    }
}
