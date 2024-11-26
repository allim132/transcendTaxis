using UnityEngine;
using System.Collections;

public class WeatherManager : MonoBehaviour
{
    public ParticleSystem rainParticleSystem;
    public ParticleSystem snowParticleSystem;
    public float gameDuration = 420f; // 7 minutes in seconds
    public float weatherDuration = 60f; // 1 minute in seconds

    private bool weatherActivated = false;

    void Start()
    {
        // Start the coroutine to manage weather
        StartCoroutine(ManageWeather());
    }

    IEnumerator ManageWeather()
    {
        DeactivateWeather();
        // Wait for a random time before activating weather
        float randomStartTime = Random.Range(0, gameDuration - weatherDuration);
        yield return new WaitForSeconds(randomStartTime);

        // Randomly choose between rain and snow
        if (Random.value > 0.5f)
        {
            ActivateWeather(rainParticleSystem, snowParticleSystem);
        }
        else
        {
            ActivateWeather(snowParticleSystem, rainParticleSystem);
        }

        // Wait for the duration of the weather effect
        yield return new WaitForSeconds(weatherDuration);

        // Deactivate all weather effects
        DeactivateWeather();
    }

    void ActivateWeather(ParticleSystem activate, ParticleSystem deactivate)
    {
        if (deactivate.isPlaying)
        {
            deactivate.Stop();
        }

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