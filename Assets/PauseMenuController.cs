using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuPanel; // Reference to the pause menu panel
    public GameObject pauseButton;

    public AudioSource[] allAudioSources; // Array to store all audio sources
    public MusicManager musicManager; // For starting and pausing music
   

    private bool isPaused = false;

    void Start()
    {
        // Ensure the pause menu is hidden at start
        pauseMenuPanel.SetActive(false);
    }

    public void TogglePause()
    {
        // Debug.Log("Pause clicked");
        isPaused = !isPaused;

        if (isPaused)
        {
            musicManager.PauseMusic();
            PauseGame();
            Debug.Log("Game is paused");
        }
        else
        {
            musicManager.ResumeMusic();
            ResumeGame();
            Debug.Log("Game is unpaused");
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // Stop time
        pauseMenuPanel.SetActive(true); // Show pause menu
        pauseButton.SetActive(false); // Hide pause button


        // Stop all audio sources
        foreach (AudioSource audio in allAudioSources)
        {
            audio.Pause();
        }

    }

    public void StopGame()
    {
        Time.timeScale = 0f; // Stop time
        pauseButton.SetActive(false); // Hide pause button

        // Stop all audio sources
        foreach (AudioSource audio in allAudioSources)
        {
            audio.Pause();
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume time
        pauseMenuPanel.SetActive(false); // Hide pause menu
        pauseButton.SetActive(true); // Show pause button
        // Resume all audio sources
        foreach (AudioSource audio in allAudioSources)
        {
            audio.UnPause();
        }

        // Enable car controls here if necessary
    }

    public void QuitGame()
    {
        ResumeGame();
        SceneManager.LoadScene("mainMenuScreen");
    }
}