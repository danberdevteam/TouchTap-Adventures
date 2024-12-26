using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Audio_Manager : MonoBehaviour
{
    public static Audio_Manager instance;

    public AudioSource musicSource;
    public List<AudioClip> musicClips;
    private const string VolumeKey = "AudioVolume"; // Key for storing volume in PlayerPrefs

    public void Awake()
    {
        // Singleton pattern to ensure only one instance exists.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the sceneLoaded event
        InitializeVolume(); // Initialize volume from PlayerPrefs
        PlayMusic(0);
    }

    public void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to prevent memory leaks
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Stop music in specific scenes if needed
        if (scene.buildIndex == 2)
        {
            musicSource.Stop();
        }
        else
        {
            // Optionally restart music or change track if needed
            PlayMusic(0); // Adjust as needed to play different tracks
        }
    }

    public void PlayMusic(int index)
    {
        if (index >= 0 && index < musicClips.Count)
        {
            musicSource.clip = musicClips[index];
            musicSource.Play();
        }
    }

    public void SetVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat(VolumeKey, volume); // Save volume in PlayerPrefs
        PlayerPrefs.Save();
    }

    private void InitializeVolume()
    {
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 0.5f); // Default volume is 0.5
        musicSource.volume = savedVolume;
    }
}
