using UnityEngine;
using System.Collections;

namespace Rooms
{

    public class Room : MonoBehaviour
    {
        public GameObject doorObject;
        public GameObject[] doorSpaces; //0 = south, 1 = west, 2 = north, 3 = east

        private Direction[] doorDirections;

        //public List<GameObject> targetList;
        //public List<GameObject> doorList;

        public void SetData(params Direction[] directions)
        {
            doorDirections = directions;

            InitializeRoom();
        }

        void InitializeRoom()
        {
            foreach (Direction dir in doorDirections)
            {
                if (dir == Direction.South)
                {
                    SetDoor(0);
                }
                else if (dir == Direction.West)
                {
                    SetDoor(1);
                }
                else if (dir == Direction.North)
                {
                    SetDoor(2);
                }
                else if (dir == Direction.East)
                {
                    SetDoor(3);
                }
            }            
        }

        void SetDoor(int doorNum)
        {
            Vector3 doorPosition = doorSpaces[doorNum].transform.position;
            Destroy(doorSpaces[doorNum]);

            if (doorNum == 1 || doorNum == 3)
                Instantiate(doorObject, doorPosition, Quaternion.Euler(new Vector3(0, 90, 0)));
            else
                Instantiate(doorObject, doorPosition, Quaternion.identity);
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
