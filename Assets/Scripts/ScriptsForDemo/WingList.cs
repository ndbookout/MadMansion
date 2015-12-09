using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WingList : MonoBehaviour {

    public GameObject player;
    public string wing;
    private List<GameObject> roomList = new List<GameObject>();

	void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Room" && !roomList.Contains(collision.gameObject))
        {
            roomList.Add(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Player")
        {
            player.SendMessage("UpdateWingList", roomList);
        }
    }
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
