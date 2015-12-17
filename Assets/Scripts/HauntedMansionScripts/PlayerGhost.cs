using UnityEngine;
using System.Collections;

public class PlayerGhost : MonoBehaviour
{
    public static PlayerGhost player;

	// Use this for initialization
	void Awake ()
    {
        player = this;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
