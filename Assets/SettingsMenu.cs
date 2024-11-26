using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex); // Apply the selected quality level
        PlayerPrefs.SetInt("QualitySetting", qualityIndex); // Save the setting
        PlayerPrefs.Save();
    }

    private void Start()
    {
        // Load the saved quality level at the start of the game
        if (PlayerPrefs.HasKey("QualitySetting"))
        {
            int savedQuality = PlayerPrefs.GetInt("QualitySetting");
            QualitySettings.SetQualityLevel(savedQuality);
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene("mainMenuScreen");
    }
}