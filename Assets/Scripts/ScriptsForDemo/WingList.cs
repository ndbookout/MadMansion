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
        Debug.Log(roomList.Count + "room was added");
        if (collision.gameObject.tag == "Room" && !roomList.Contains(collision.gameObject))
        {
            roomList.Add(collision.gameObject);
        }
        else if (collision.gameObject.tag == "NPC")
        {
            collision.gameObject.GetComponent<NPC>().UpdateWingList(roomList);
            //npc.SendMessage("UpdateWingList", roomList);
        }
    }
}
