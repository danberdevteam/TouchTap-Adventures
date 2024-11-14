using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Audio_Manager : MonoBehaviour
{
    public static Audio_Manager instance;

    public AudioSource musicSource;
    public List<AudioClip> musicClips;

    void Awake()
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
        }

        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the sceneLoaded event
        PlayMusic(0);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to prevent memory leaks
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the newly loaded scene's index is 2
        if (scene.buildIndex == 2)
        {
            musicSource.Stop();
        }
        else
        {
            // Optionally restart music or change track if needed
            PlayMusic(0); // You can adjust this to play different music based on the scene
        }
    }

    public void PlayMusic(int index)
    {
        if (!musicSource.isPlaying)
        {
            musicSource.clip = musicClips[index];
            musicSource.Play();
        }
    }
}
