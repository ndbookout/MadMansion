using UnityEngine;
using System.Collections;

namespace Tiling
{
    public class BinaryTileNode : TileNode
    {
        private int cellNumber; //counts columns along x direction

        public BinaryTileNode() : base() { }
        public BinaryTileNode(int number) : base(number, Vector3.zero, Vector3.zero, null) { }
        public BinaryTileNode(int number, Vector3 position) : this(number, position, null, null) { }
        public BinaryTileNode(int number, Vector3 position, BinaryTileNode left, SingularTileNode right)
        {
            base.tileNumber = number;
            base.position = position;

            TileNodeList children = new TileNodeList(2);
            children[0] = left;
            children[1] = right;

            base.Neighbors = children;
        }

        public BinaryTileNode Left
        {
            get
            {
                if (base.Neighbors == null)
                    return null;
                else
                    return (BinaryTileNode)base.Neighbors[0];
            }
            set
            {
                if (base.Neighbors == null)
                    base.Neighbors = new TileNodeList(2);

                base.Neighbors[0] = value;
            }
        }

        public SingularTileNode Right
        {
            get
            {
                if (base.Neighbors == null)
                    return null;
                else
                    return (SingularTileNode)base.Neighbors[1];
            }
            set
            {
                if (base.Neighbors == null)
                    base.Neighbors = new TileNodeList(2);

                base.Neighbors[1] = value;
            }
        }

        void Initialize(int number, Vector3 position, int cellNumber)
        {
            this.tileNumber = number;
            this.position = position;
            this.cellNumber = cellNumber;

            this.scale = RootTile.Root.rootScale;
        }

        protected override void Start()
        {
            base.Start();

            tile = RootTile.Root.tile;
            Vector3 leftPosition = position + new Vector3(scale.x * 10, 0, 0);
            Vector3 rightPosition = position + new Vector3(0, 0, scale.z * 10);

            Debug.Log(cellNumber);

            if (cellNumber < RootTile.Root.roomSize.y)
            {
                RootTile.currentNumber++;
                GameObject leftTile = Instantiate(tile, leftPosition, Quaternion.identity) as GameObject;
                leftTile.transform.parent = this.transform;
                Left = leftTile.AddComponent<BinaryTileNode>();
                Left.Initialize(RootTile.currentNumber, leftPosition, cellNumber + 1);

                RootTile.currentNumber++;
                GameObject rightTile = Instantiate(tile, rightPosition, Quaternion.identity) as GameObject;
                rightTile.transform.parent = this.transform;
                Right = rightTile.AddComponent<SingularTileNode>();
                Right.Initialize(RootTile.currentNumber, rightPosition, 1);
            }
            else
            { }
            //Walls   
        }
    }
}
