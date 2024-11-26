using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class mainMenu : MonoBehaviour
{
    // called when "start" button is pressed
    public void OnStartButton ()
    {
        SceneManager.LoadScene("Demo");
    }
    // called when "settings" button is pressed
    public void OnSettingsButton ()
    {
        Application.Quit();
    }
}
