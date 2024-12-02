using UnityEngine;
using System.Collections;

public class WeatherManager : MonoBehaviour
{
    public PrometeoCarController prometeoCarController;
    
    public ParticleSystem rainParticleSystem;
    public ParticleSystem snowParticleSystem;

    public float gameDuration = 45f; // 45 seconds
    public float weatherDuration = 15f; // 15 seconds
    public float weatherGap = 10f; 

    private bool weatherActivated = false;
    
    void Start()
    {
        // Start the coroutine to manage weather
        StartCoroutine(ManageWeather());
    }

    IEnumerator ManageWeather()
    {
        // Game starts with default (sunny) condition
        DeactivateWeather();
        prometeoCarController.setDriftNormal();

        // Wait for a random time before activating weather
        float randomStartTime = Random.Range(0, weatherGap);
        Debug.Log("randomStartTime: " +  randomStartTime);
        yield return new WaitForSeconds(randomStartTime);
        
        // Randomly choose between rain and snow
        if (Random.value > 0.5f)
        {
            //DeactivateWeather();
            ActivateWeather(rainParticleSystem); // Rain
            prometeoCarController.setDriftRain();
            Debug.Log("Raining!");
        }
        else
        {
            //DeactivateWeather();
            ActivateWeather(snowParticleSystem); // Snow
            prometeoCarController.setDriftIce();
            Debug.Log("Snowing!");
        }

        // Wait for the duration of the weather effect
        yield return new WaitForSeconds(weatherDuration);

        // Deactivate all weather effects
        prometeoCarController.setDriftNormal();
        DeactivateWeather();

        StartCoroutine(ManageWeather());
    }

    void ActivateWeather(ParticleSystem activate)
    {
        if (!activate.isPlaying)
        {
            activate.Play();
            weatherActivated = true;
        }
    }

    void DeactivateWeather()
    {
        if (rainParticleSystem != null && rainParticleSystem.isPlaying)
        {
            rainParticleSystem.Stop();
        }

        if (snowParticleSystem != null && snowParticleSystem.isPlaying)
        {
            snowParticleSystem.Stop();
        }

        weatherActivated = false;
    }
}