using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuPanel; // Reference to the pause menu panel
    public AudioSource[] allAudioSources; // Array to store all audio sources

    private bool isPaused = false;

    void Start()
    {
        // Ensure the pause menu is hidden at start
        pauseMenuPanel.SetActive(false);
    }

    public void TogglePause()
    {
        Debug.Log("Pause clicked");
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f; // Stop time
        pauseMenuPanel.SetActive(true); // Show pause menu

        // Stop all audio sources
        foreach (AudioSource audio in allAudioSources)
        {
            audio.Pause();
        }

        // Disable car controls here if necessary
    }

    void ResumeGame()
    {
        Time.timeScale = 1f; // Resume time
        pauseMenuPanel.SetActive(false); // Hide pause menu

        // Resume all audio sources
        foreach (AudioSource audio in allAudioSources)
        {
            audio.UnPause();
        }

        // Enable car controls here if necessary
    }

    public void QuitGame()
    {
        // Handle quitting the game or returning to main menu
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}