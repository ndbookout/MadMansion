using UnityEngine;
using UnityEngine.UI; // This is added because we are using the Unity UI in our code to get all the fucntions
using System.Collections;

public class GameStart : MonoBehaviour {

    public Button startText; //This is our game start button 


    // Use this for initialization
    void Start ()
    {
        startText = startText.GetComponent<Button>(); // Same as the top
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void StartLevel() //this will load the first stage when play is hit
    {
        Application.LoadLevel(1);
    }
}
