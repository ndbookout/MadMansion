using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Rooms
{

    public enum Direction
    {
        South, //south is -z direction
        West, //west is -x direction
        North, //north is +z direction
        East //east is +x direction
    }
    public enum RoomType
    {
        DeadEnd,
        TwoDoor,
        ThreeDoor,
        FourDoor
    }

    public class RoomGenerator : MonoBehaviour
    {
        //Eventually want an array of possible rooms
        public GameObject room;

        //For determining where rooms should go
        public RoomSpace startRoom;
        private RoomSpace currentRoom;
        private List<Direction> connectingDirections;
        private List<RoomSpace> connectingRooms;
        private List<RoomSpace> finishedRooms;
        private RaycastHit roomHit;
        private int roomMask = 1 << 14;

        private bool noRoomSpaces;
        private int roomCount;

        void Start()
        {
            connectingDirections = new List<Direction>();
            connectingRooms = new List<RoomSpace>();
            finishedRooms = new List<RoomSpace>();

            startRoom.connectingDoor = Direction.North;
            currentRoom = startRoom;
            noRoomSpaces = false;
            roomCount = 0;

            ManageRooms();            
        }

        void ManageRooms()
        {
            CreateNewRoom();

            while(!noRoomSpaces)
            {
                if (roomCount == connectingRooms.Count)
                {
                    noRoomSpaces = true;
                    break;
                }

                currentRoom = connectingRooms[roomCount];
                CreateNewRoom();
                roomCount++;       
            }           
        }

        void CreateNewRoom()
        {
            finishedRooms.Add(currentRoom);

            connectingDirections.Clear();
            foreach (Direction dir in currentRoom.NewRoom(room))
                connectingDirections.Add(dir);

            FindConnectingRooms();
        }

        void FindConnectingRooms()
        {
            Vector3 rayDirection;           

            foreach (Direction dir in connectingDirections)
            {
                if (dir == Direction.South)
                {
                    rayDirection = new Vector3(0, 0, -1);
                    SearchForRoom(rayDirection, Direction.North);
                }
                else if (dir == Direction.West)
                {
                    rayDirection = new Vector3(-1, 0, 0);
                    SearchForRoom(rayDirection, Direction.East);
                }
                else if (dir == Direction.North)
                {
                    rayDirection = new Vector3(0, 0, 1);
                    SearchForRoom(rayDirection, Direction.South);
                }
                else if (dir == Direction.East)
                {
                    rayDirection = new Vector3(1, 0, 0);
                    SearchForRoom(rayDirection, Direction.West);
                }
            }
        }

        private void SearchForRoom(Vector3 rayDirection, Direction connectingDoor)
        {
            if (Physics.Raycast(currentRoom.transform.position, rayDirection, out roomHit, 100, roomMask))
            {
                Debug.Log(roomHit.collider.name);

                if (roomHit.collider != null)
                {
                    if (roomHit.collider.tag == "RoomSpace")
                    {
                        RoomSpace nextRoom = roomHit.collider.GetComponent<RoomSpace>();

                        if (!finishedRooms.Contains(nextRoom))
                        {
                            connectingRooms.Add(nextRoom);
                            connectingRooms[connectingRooms.Count - 1].connectingDoor = connectingDoor;
                        }
                    }
                }
            }
        }
    }
}
