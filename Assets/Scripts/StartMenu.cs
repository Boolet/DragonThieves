using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {

	public void StartButton()
    {
        //Replace 1 with name/index of main game scene later
        SceneManager.LoadScene(1);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
