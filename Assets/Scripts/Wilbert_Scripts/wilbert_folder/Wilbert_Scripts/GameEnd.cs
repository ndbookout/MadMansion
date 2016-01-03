using UnityEngine;
using UnityEngine.UI; // This is added because we are using the Unity UI in our code to get all the fucntions
using System.Collections;

public class GameEnd : MonoBehaviour
{

    public Button exitGame; //this is for us to exit the game
    public Button retryText; //this is for us to exit the game


    // Use this for initialization
    void Start()
    {
        exitGame = exitGame.GetComponent<Button>(); // same at the top
        retryText = retryText.GetComponent<Button>(); // same at the top
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ExitGame() //This will exit the game and pop the exit menu when hit
    {
        Application.Quit();
    }

    public void RetryGame() //This will exit the game and pop the exit menu when hit
    {
        Application.LoadLevel(1);
    }
}
