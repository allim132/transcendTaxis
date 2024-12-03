using UnityEngine;
using System.Collections.Generic;

/* 
 * This component was developed by the Lead Developer Alex.
 * 
 * Lead Developer Note:
 * This component satisfies the "Music" part of the "Music and Sound Effects" component outlined in the SRA document.
 */
public class MusicManager : MonoBehaviour
{
    public List<AudioSource> gameplayMusicList;
    public List<AudioSource> pauseMusicList;

    private int currentMusicIndex = 0;

    private void Start()
    {
        if (gameplayMusicList.Count > 0)
        {
            PlayGameplayMusic(0);
        }
    }

    private void Update()
    {
        if (!gameplayMusicList[currentMusicIndex].isPlaying && Time.timeScale > 0f)
        {
            PlayNextGameplayMusic();
        }
    }

    private void PlayGameplayMusic(int index)
    {
        StopAllMusic();
        currentMusicIndex = index;
        gameplayMusicList[currentMusicIndex].Play();
    }

    private void PlayNextGameplayMusic()
    {
        currentMusicIndex = (currentMusicIndex + 1) % gameplayMusicList.Count;
        PlayGameplayMusic(currentMusicIndex);
    }

    public void PauseMusic()
    {
        // Time.timeScale = 0f;
        gameplayMusicList[currentMusicIndex].Pause();
        int pauseMusicIndex = Mathf.Min(currentMusicIndex / 2, pauseMusicList.Count - 1);
        pauseMusicList[pauseMusicIndex].Play();
    }

    public void ResumeMusic()
    {
        // Time.timeScale = 1f;
        StopAllPauseMusic();
        gameplayMusicList[currentMusicIndex].UnPause();
    }

    private void StopAllMusic()
    {
        foreach (var music in gameplayMusicList)
        {
            music.Stop();
        }
        StopAllPauseMusic();
    }

    private void StopAllPauseMusic()
    {
        foreach (var music in pauseMusicList)
        {
            music.Stop();
        }
    }
}