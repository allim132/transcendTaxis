using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer; // Reference to the Audio Mixer
    public Slider masVol; // Master volume slider
    public Slider musVol; // Music volume slider
    public Slider sfxVol; // SFX volume slider
    public TMP_Dropdown qualityDropdown; // Quality dropdown

    private void Start()
    {

        // Load saved quality level
        int savedQuality = PlayerPrefs.GetInt("QualitySetting", QualitySettings.GetQualityLevel());
        QualitySettings.SetQualityLevel(savedQuality);
        if (qualityDropdown != null)
        {
            qualityDropdown.value = savedQuality;
        }

        // Load saved volumes
        masVol.value = PlayerPrefs.GetFloat("masterVolume", 0.75f);
        musVol.value = PlayerPrefs.GetFloat("musicVolume", 0.75f);
        sfxVol.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        SetMasterVolume(masVol.value);
        SetMusicVolume(musVol.value);
        SetSFXVolume(sfxVol.value);
    }

    public void SetQualityLevel(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualitySetting", qualityIndex);
        PlayerPrefs.Save(); // Save changes

        Debug.Log($"Quality Level changed to: {QualitySettings.names[qualityIndex]} ({qualityIndex})");
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20); // Logarithmic scaling
        PlayerPrefs.SetFloat("masterVolume", volume);
        PlayerPrefs.Save(); // Save changes
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20); // Logarithmic scaling
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.Save(); // Save changes
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20); // Logarithmic scaling
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save(); // Save changes
    }

    public void Exit()
    {
        SceneManager.LoadScene("mainMenuScreen"); // Load the main menu scene
    }
}