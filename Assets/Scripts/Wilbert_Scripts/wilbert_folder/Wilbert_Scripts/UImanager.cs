using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UImanager : MonoBehaviour {

    public GameObject pausePanel;

    public bool isPaused;

	// Use this for initialization
	void Start ()
    {
        isPaused = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isPaused)
        {
            PauseGame(true);
        }
        else
        {
            PauseGame(false);
        }

        if(Input.GetButtonDown("pause"))
        {
            SwitchPause();
        }
	}

    void PauseGame(bool state)
    {
        if (state)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0.0f; //The Game is Paused
        }
        else
        {
            Time.timeScale = 1.0f; //The Game is UnPaused
            pausePanel.SetActive(false);
        }
    }

    public void SwitchPause()
    {
        if (isPaused)
        {
            isPaused = false; //change value
        }
        else
        {
            isPaused = true;
        }
    }

    public void reStartLevel() //this will load the first stage when play is hit
    {
        Application.LoadLevel(1);
    }
}
