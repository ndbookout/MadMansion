using UnityEngine;
using System.Collections;

namespace Rooms
{

    public class SetRenderQueue : MonoBehaviour
    {
        public Direction direction;

        // Use this for initialization
        void Start()
        {
            if (direction == Direction.South)
                GetComponent<Renderer>().material.renderQueue = 2001;
            else if (direction == Direction.West)
                GetComponent<Renderer>().material.renderQueue = 2002;
            else if (direction == Direction.North)
                GetComponent<Renderer>().material.renderQueue = 2003;
            else if (direction == Direction.West)
                GetComponent<Renderer>().material.renderQueue = 2004;
        }
    }
}
