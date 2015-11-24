using UnityEngine;
using System.Collections;

namespace Tiling
{
    public enum RoomType
    {
        Hallway, //Singular root tile
        Square, //Binary root tile
        Triad //Ternary root tile
    }

    public class RootTile : MonoBehaviour
    {
        private static RootTile root;
        public static RootTile Root
        {
            get { return root; }
            set { root = value; }
        }

        public RoomType room;
        public Vector2 roomSize;
        public GameObject singleTile;
        public GameObject binaryTile;
        public GameObject ternaryTile;
        public GameObject tile;

        public static int currentNumber = 0;

        private int rootNumber;
        public Vector3 rootPosition;
        public Vector3 rootScale;
        private GameObject rootTile;

        public RootTile()
        {
            rootTile = null;
        }

        public void Clear()
        {
            rootTile = null;
        }

        // Use this for initialization
        private void Awake()
        {
            Root = this;
            if (currentNumber == 0)
                rootNumber = 0;
            else
                rootNumber = currentNumber;
            currentNumber = rootNumber + 1;

            if ((int)room == 0)
            {
                rootTile = singleTile;
            }
            else if ((int)room == 1)
            {
                rootTile = binaryTile;
            }
            else if ((int)room == 2)
            {
                rootTile = ternaryTile;
            }

            Instantiate(rootTile, rootPosition, Quaternion.identity);
        }
    }
}
