using UnityEngine;
using UnityEngine.UI; // This is added because we are using the Unity UI in our code to get all the fucntions
using System.Collections;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour {

    public Button startText; //This is our game start button 

<<<<<<< HEAD

    // Use this for initialization
    void Start ()
    {
        startText = startText.GetComponent<Button>(); // Same as the top
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

=======
>>>>>>> d6ce7b8075a8315469ebfeedc87afaf8b052c218
    public void StartLevel() //this will load the first stage when play is hit
    {
        SceneManager.LoadScene(1);
    }
}
