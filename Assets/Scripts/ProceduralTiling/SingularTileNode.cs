using UnityEngine;
using System.Collections;

namespace Tiling
{
    public class SingularTileNode : TileNode
    {
        private int cellNumber = 0; //counts rows along z direction

        public SingularTileNode() : base() { }
        public SingularTileNode(int number, Vector3 position) : this(number, position, 0, null) { }
        public SingularTileNode(int number, Vector3 position, int cellNumber) : this(number, position, cellNumber, null) { }
        public SingularTileNode(int number, Vector3 position, int cellNum, SingularTileNode next)
        {
            base.tileNumber = number;
            base.position = position;
            this.cellNumber = cellNum;

            TileNodeList child = new TileNodeList(1);
            child[0] = next;

            base.Neighbors = child;
        }

        public SingularTileNode Next
        {
            get
            {
                if (base.Neighbors == null)
                    return null;
                else
                    return (SingularTileNode)base.Neighbors[0];
            }
            set
            {
                if (base.Neighbors == null)
                    base.Neighbors = new TileNodeList(1);

                base.Neighbors[0] = value;
            }
        }

        public void Initialize(int number, Vector3 position, int cellNumber)
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
            Vector3 nextPosition = position + new Vector3(0, 0, transform.localScale.z * 10);

            Debug.Log(cellNumber);

            if (cellNumber < RootTile.Root.roomSize.x)
            {
                RootTile.currentNumber++;
                GameObject nextTile = Instantiate(tile, nextPosition, Quaternion.identity) as GameObject;
                nextTile.transform.parent = this.transform;

                Next = nextTile.AddComponent<SingularTileNode>();
                Next.Initialize(RootTile.currentNumber, nextPosition, cellNumber + 1);
            }
            else
            { }
            //Walls   
        }
    }
}
