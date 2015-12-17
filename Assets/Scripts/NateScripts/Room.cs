using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Rooms
{
    public class Room : MonoBehaviour
    {
        public GameObject doorObject;
        public GameObject[] doorSpaces; //0 = south, 1 = west, 2 = north, 3 = east

        private List<Direction> doorDirs;

        //public List<GameObject> targetList;
        //public List<GameObject> doorList;

        private void Awake()
        {
            doorDirs = new List<Direction>();
        }

        public void SetData(params Direction[] directions)
        {
            foreach (Direction dir in directions)
                doorDirs.Add(dir);

            InitializeRoom();
        }

        void InitializeRoom()
        {
            foreach (Direction dir in doorDirs)
            {
                if (dir == Direction.South)
                {
                    if (CheckForRoom(dir))
                        SetDoor(0, false);
                    else
                        SetDoor(0, true);
                }
                else if (dir == Direction.West)
                {
                    if (CheckForRoom(dir))
                        SetDoor(1, false);
                    else
                        SetDoor(1, true);
                }
                else if (dir == Direction.North)
                {
                    if (CheckForRoom(dir))
                        SetDoor(2, false);
                    else
                        SetDoor(2, true);
                }
                else if (dir == Direction.East)
                {
                    if (CheckForRoom(dir))
                        SetDoor(3, false);
                    else
                        SetDoor(3, true);
                }
            }            
        }

        private bool CheckForRoom(Direction dir)
        {
            RaycastHit roomHit;
            int roomMask = 1 << 14;

            Vector3 rayDirection;
            //for ray direction
            if (dir == Direction.South)
                rayDirection = new Vector3(0, 0, -1);
            else if (dir == Direction.West)
                rayDirection = new Vector3(-1, 0, 0);
            else if (dir == Direction.North)
                rayDirection = new Vector3(0, 0, 1);
            else if (dir == Direction.East)
                rayDirection = new Vector3(1, 0, 0);
            else
            {
                rayDirection = new Vector3(0, 0, 0);
                Debug.LogError("Invalid direction");
            }

            //perform test for already initialized room
            if (Physics.Raycast(transform.position, rayDirection, out roomHit, 20, roomMask))
            {
                RoomSpace otherRoom = roomHit.collider.gameObject.GetComponent<RoomSpace>();
                if (otherRoom != null)
                {
                    if (otherRoom.roomData.initialized == true)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        void SetDoor(int doorNum, bool withDoor)
        {
            Vector3 doorPosition = doorSpaces[doorNum].transform.position;
            Destroy(doorSpaces[doorNum]);

            if (withDoor == true)
            {
                if (doorNum == 1 || doorNum == 3)
                    Instantiate(doorObject, doorPosition, Quaternion.Euler(new Vector3(0, 90, 0)));
                else
                    Instantiate(doorObject, doorPosition, Quaternion.identity);
            }       
        }

        //public List<GameObject> GetTargetList()
        //{
        //    return targetList;
        //}

        //public void SetTargetList(GameObject tar)
        //{
        //    targetList.Add(tar);
        //}

        //public void SetDoorList(GameObject door)
        //{
        //    doorList.Add(door);
        //}
    }
}
