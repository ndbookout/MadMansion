using UnityEngine;
using System.Collections;

namespace Rooms
{

    public class RoomSpace : MonoBehaviour
    {
        public Direction[] bannedDirections;
        public Direction connectingDoor;

        private RoomType room;
        private Direction[] directions;

        public Direction[] NewRoom(GameObject room)
        {
            GameObject newRoom = Instantiate(room, transform.position, Quaternion.identity) as GameObject;

            PickRoomType();
            PickDirections();

            while (!CheckDirections())
                PickDirections(); 

            Room newRoomData = newRoom.GetComponent<Room>();
            newRoomData.SetData(directions);
            return directions;
        }

        private void PickRoomType()
        {
            //Room type
            if (bannedDirections.Length >= 2)
                room = (RoomType)Random.Range(0, 2);
            else if (bannedDirections.Length == 1)
                room = (RoomType)Random.Range(0, 3);
            else           
                room = (RoomType)Random.Range(0, 4);             
        }

        private void PickDirections()
        {
            //Door directions
            if (room == RoomType.DeadEnd)
            {
                directions = new Direction[1];
                directions[0] = connectingDoor;
            }
            else if (room == RoomType.TwoDoor)
            {
                directions = new Direction[2];
                directions[0] = connectingDoor;
                directions[1] = (Direction)Random.Range(0, 4);
            }
            else if (room == RoomType.ThreeDoor)
            {
                directions = new Direction[3];
                directions[0] = connectingDoor;
                directions[1] = (Direction)Random.Range(0, 4);
                directions[2] = (Direction)Random.Range(0, 4);
            }
            else if (room == RoomType.FourDoor)
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
    }
}
