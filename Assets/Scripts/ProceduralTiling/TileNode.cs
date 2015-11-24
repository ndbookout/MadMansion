using UnityEngine;
using System.Collections;

namespace Tiling
{

    public class TileNode : MonoBehaviour
    {
        private Transform tileTransform;
        protected Vector3 scale;

        public int tileNumber;
        public Vector3 position;
        protected TileNodeList neighbors;
        protected GameObject tile;

        public GameObject Tile
        {
            get { return tile; }
            set { tile = value; }
        }

        public TileNodeList Neighbors
        {
            get { return neighbors; }
            set { neighbors = value; }
        }

        public TileNode() { }
        public TileNode(int number) : this(number, Vector3.zero, Vector3.zero, null, null) { }
        public TileNode(int number, Vector3 position, Vector3 scale) : this(number, position, scale, null, null) { }
        public TileNode(int number, Vector3 position, Vector3 scale, GameObject tile) : this (number, position, scale, tile, null) { }
        public TileNode(int number, Vector3 position, Vector3 scale, GameObject tile, TileNodeList neighbors)
        {
            this.tileNumber = number;
            this.position = position;
            this.scale = scale;
            this.neighbors = neighbors;
            this.tile = tile;
        }

        private void Awake()
        {
            scale = RootTile.Root.rootScale;
        }

        protected virtual void Start()
        {
            tileTransform = GetComponent<Transform>();
            tileTransform.localScale = scale;
            Debug.Log(scale);
        }
    }
}
