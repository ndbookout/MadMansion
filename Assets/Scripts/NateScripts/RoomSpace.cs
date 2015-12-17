using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Rooms
{
    public class RoomSpace : MonoBehaviour
    {
        public RoomData roomData;

        public Direction[] bannedDirections;
        public Direction connectingDoor;

        private RoomType roomType;
        private Direction[] directions;

        private List<GameObject> roomTargetsList;

        void Awake()
        {
            roomData = new RoomData(false);
            roomTargetsList = new List<GameObject>();
        }

        public Direction[] NewRoom(GameObject room)
        {
            GameObject newRoom = Instantiate(room, transform.position, Quaternion.identity) as GameObject;

            PickRoomType();
            PickDirections();

            while (!CheckDirections())
            {
                PickRoomType();
                PickDirections();
            }         

            Room newRoomInfo = newRoom.GetComponent<Room>();
            newRoomInfo.SetData(directions); //this calls Room script

            roomData = new RoomData(true, roomType, directions);
            return directions; //returns to RoomGenerator
        }

        private void PickRoomType()
        {
            //Room type       
            if (bannedDirections.Length >= 3)           
                roomType = RoomType.DeadEnd;         
            else if (bannedDirections.Length == 2)
                roomType = (RoomType)Random.Range(0, 2);
            else if (bannedDirections.Length == 1)
                roomType = (RoomType)Random.Range(1, 3);
            else if (bannedDirections.Length == 0)           
                roomType = (RoomType)Random.Range(1, 4);             
        }

        private void PickDirections()
        {
            //Door directions
            if (roomType == RoomType.DeadEnd)
            {
                directions = new Direction[1];
                directions[0] = connectingDoor;
            }
            else if (roomType == RoomType.TwoDoor)
            {
                directions = new Direction[2];
                directions[0] = connectingDoor;
                directions[1] = (Direction)Random.Range(0, 4);
            }
            else if (roomType == RoomType.ThreeDoor)
            {
                directions = new Direction[3];
                directions[0] = connectingDoor;
                directions[1] = (Direction)Random.Range(0, 4);
                directions[2] = (Direction)Random.Range(0, 4);
            }
            else if (roomType == RoomType.FourDoor)
            {
                directions = new Direction[4];
                directions[0] = connectingDoor;
                directions[1] = (Direction)Random.Range(0, 4);
                directions[2] = (Direction)Random.Range(0, 4);
                directions[3] = (Direction)Random.Range(0, 4);
            }
        }

        private bool CheckDirections()
        {
            for (int i = 0; i < directions.Length; i++)
            {
                for (int j = i + 1; j < directions.Length; j++)
                {
                    if (directions[i] == directions[j])
                        return false;
                }
            }

            foreach(Direction banDir in bannedDirections)
            {
                foreach (Direction dir in directions)
                {
                    if (banDir == dir)                 
                        return false;                                    
                }
            }

            return true;
        }


        private void OnTriggerEnter(Collider collide)
        {
            if (collide.tag == "Target" && !roomTargetsList.Contains(collide.gameObject))
            {
                roomTargetsList.Add(collide.gameObject);
            }

            if (collide.tag == "NPC")
            {
                collide.gameObject.GetComponent<NPC>().UpdateRoomList(roomTargetsList);
                //npc.SendMessage("UpdateRoomList", roomTargetsList);
            }
        }

        private void Update()
        {
            Debug.Log(this.gameObject + " targets: " + roomTargetsList.Count);
        }
    }
}
