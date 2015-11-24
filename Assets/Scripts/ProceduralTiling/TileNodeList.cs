using System.Collections;
using System.Collections.ObjectModel;

namespace Tiling
{
    public class TileNodeList : Collection<TileNode>
    {
        public TileNodeList() : base() { }

        public TileNodeList(int initialSize)
        {
            // Add the specified number of items
            for (int i = 0; i < initialSize; i++)
                base.Items.Add(default(TileNode));
        }

        public TileNode FindByNumber(int num)
        {
            // search the list for the value
            foreach (TileNode node in Items)
                if (node.tileNumber.Equals(num))
                    return node;

            // if we reached here, we didn't find a matching node
            return null;
        }
    }
}
