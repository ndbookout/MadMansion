using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartButtonController : MonoBehaviour {

	public void startPressed()
    {
        LoadingScreen.show();
        SceneManager.LoadScene(1);
    }

    public void quitPressed()
    {
        Application.Quit();
    }
}
