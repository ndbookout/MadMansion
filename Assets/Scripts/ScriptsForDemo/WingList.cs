using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WingList : MonoBehaviour
{
    private GameObject npc;
    private List<GameObject> roomList = new List<GameObject>();

    void Start()
    {
        npc = GameObject.FindGameObjectWithTag("NPC");
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Room" && !roomList.Contains(collision.gameObject))
        {
            roomList.Add(collision.gameObject);
        }
        else if (collision.gameObject.tag == "NPC")
        {
            npc.SendMessage("UpdateWingList", roomList);
        }
    }
}
