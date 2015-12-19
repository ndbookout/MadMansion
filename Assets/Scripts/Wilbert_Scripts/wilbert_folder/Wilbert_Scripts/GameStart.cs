using UnityEngine;
using UnityEngine.UI; // This is added because we are using the Unity UI in our code to get all the fucntions
using System.Collections;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour {

    public Button startText; //This is our game start button 
    public Button exitText; //this is for us to exit the game
    public Button retryText; //this is for us to exit the game


    // Use this for initialization
    void Start ()
    {
        startText = startText.GetComponent<Button>(); // Same as the top
        exitText = exitText.GetComponent<Button>(); // same at the top
        exitText = retryText.GetComponent<Button>(); // same at the top
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartLevel() //this will load the first stage when play is hit
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame() //This will exit the game and pop the exit menu when hit
    {
        Application.Quit();
    }
}
