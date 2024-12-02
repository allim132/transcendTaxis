using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string demoSceneName = "Demo";

    // Called when "start" button is pressed
    public void OnStartButton()
    {
        StartCoroutine(ReloadDemoScene());
    }

    // Called when "settings" button is pressed
    public void OnSettingsButton()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    private IEnumerator ReloadDemoScene()
    {
        // Check if the Demo scene is already loaded
        if (SceneManager.GetSceneByName(demoSceneName).isLoaded)
        {
            // Unload the Demo scene
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(demoSceneName);
            yield return unloadOperation;
        }

        // Load the Demo scene
        SceneManager.LoadScene(demoSceneName);
    }
}