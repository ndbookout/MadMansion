using UnityEngine;
using System.Collections;

namespace Tiling
{
    public class TernaryTileNode : TileNode
    {
        private int cellNumber; //used to assess progress in x direction

        public TernaryTileNode() : base() { }
        public TernaryTileNode(int number) : base(number, Vector3.zero, Vector3.zero, null) { }
        public TernaryTileNode(int number, Vector3 position) : this(number, position, null, null, null) { }
        public TernaryTileNode(int number, Vector3 position, TernaryTileNode forward, SingularTileNode left, SingularTileNode right)
        {
            base.tileNumber = number;
            base.position = position;

            TileNodeList children = new TileNodeList(3);
            children[0] = forward;
            children[1] = left;
            children[2] = right;

            base.Neighbors = children;
        }

        public TernaryTileNode Forward
        {
            get
            {
                if (base.Neighbors == null)
                    return null;
                else
                    return (TernaryTileNode)base.Neighbors[0];
            }
            set
            {
                if (base.Neighbors == null)
                    base.Neighbors = new TileNodeList(3);

                base.Neighbors[0] = value;
            }
        }

        public SingularTileNode Left
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
                    base.Neighbors = new TileNodeList(3);

                base.Neighbors[1] = value;
            }
        }

        public SingularTileNode Right
        {
            get
            {
                if (base.Neighbors == null)
                    return null;
                else
                    return (SingularTileNode)base.Neighbors[2];
            }
            set
            {
                if (base.Neighbors == null)
                    base.Neighbors = new TileNodeList(3);

                base.Neighbors[2] = value;
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
            Vector3 forwardPosition = position + new Vector3(scale.x * 10, 0, 0);
            Vector3 leftPosition = position + new Vector3(0, 0, scale.z * -10);
            Vector3 rightPosition = position + new Vector3(0, 0, scale.z * 10);

            Debug.Log(cellNumber);

            if (cellNumber < RootTile.Root.roomSize.y)
            {
                RootTile.currentNumber++;
                GameObject forwardTile = Instantiate(tile, forwardPosition, Quaternion.identity) as GameObject;
                forwardTile.transform.parent = this.transform;
                Forward = forwardTile.AddComponent<TernaryTileNode>();
                Forward.Initialize(RootTile.currentNumber, rightPosition, cellNumber + 1);

                RootTile.currentNumber++;
                GameObject leftTile = Instantiate(tile, leftPosition, Quaternion.identity) as GameObject;
                leftTile.transform.parent = this.transform;
                Left = leftTile.AddComponent<SingularTileNode>();
                Left.Initialize(RootTile.currentNumber, leftPosition, 1);

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
