using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance => instance;

    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioSource sfxPlayer;

    [SerializeField] private AudioClip mainSceneMusic;
    [SerializeField] private AudioClip otherSceneMusic;

    [SerializeField] private AudioClip[] sfxSoundClips;

    private float _volume = 1.0f;
    public float Volume
    {
        get => _volume;
        set
        {
            _volume = Mathf.Clamp01(value);

            if (musicPlayer != null)
            {
                musicPlayer.volume = _volume;
                Debug.Log($"MusicPlayer volume set to {_volume}");
            }
            else
            {
                Debug.LogError("MusicPlayer is null, cannot set volume.");
            }

            if (sfxPlayer != null)
            {
                sfxPlayer.volume = _volume;
                Debug.Log($"SfxPlayer volume set to {_volume}");
            }
            else
            {
                Debug.LogError("SfxPlayer is null, cannot set volume.");
            }

            PlayerPrefs.SetFloat(GameConstants.VolumeKey, _volume);
            PlayerPrefs.Save();
            Debug.Log($"Volume saved as {_volume}");
        }
    }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this; // Set the singleton instance
            DontDestroyOnLoad(gameObject); // Ensure it persists across scenes
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Prevent duplicate instances
        }

        // Initialize volume from PlayerPrefs
        _volume = PlayerPrefs.GetFloat(GameConstants.VolumeKey, 0.5f);

        if (musicPlayer != null)
            musicPlayer.volume = _volume;
        if (sfxPlayer != null)
            sfxPlayer.volume = _volume;

        Debug.Log($"AudioManager initialized with volume {_volume}");
    }

    public void Start()
    {
        SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;

        // Explicitly set volume
        Volume = PlayerPrefs.GetFloat(GameConstants.VolumeKey, 0.5f);

        UpdateBackgroundMusic();
    }

    private void SceneManagerOnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        UpdateBackgroundMusic();
    }

    private void UpdateBackgroundMusic()
    {
        // Check the active scene name and decide the music to play
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            // Play the specific clip for MainScene
            PlayMusic(mainSceneMusic);
        }
        else
        {
            // Play the other scene music
            PlayMusic(otherSceneMusic);
        }
    }

    public void SetMusicToggle(bool toggle)
    {
        if (musicPlayer == null)
            return;

        musicPlayer.loop = true;

        if (toggle)
        {
            if (!musicPlayer.isPlaying)
            {
                musicPlayer.Play();
            }
        }
        else
        {
            musicPlayer.Pause();
        }
    }

    public void SetVolume(float volume)
    {
        Volume = volume;
    }

    public void SettingToggleMusic(bool toggle)
    {
        PlayerPrefs.SetInt("musicToggle", toggle ? 1 : 0);
        SetMusicToggle(toggle);
    }

    public void SettingToggleSFX(bool toggle)
    {
        PlayerPrefs.SetInt("sfxToggle", toggle ? 1 : 0);
        if (sfxPlayer)
        {
            sfxPlayer.mute = !toggle;
        }
    }

    public void PlayMusic(AudioClip audioClip)
    {
        if (musicPlayer == null || audioClip == null) return;

        musicPlayer.Stop();
        musicPlayer.clip = audioClip;
        musicPlayer.loop = true;
        musicPlayer.Play();
    }

    public void PlaySound(int idx)
    {
        try
        {
            if (sfxSoundClips != null && idx >= 0 && idx < sfxSoundClips.Length)
            {
                sfxPlayer.PlayOneShot(sfxSoundClips[idx]);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Error playing SFX: {e.Message}");
        }
    }

    public void PlaySound(AudioClip audioClip)
    {
        try
        {
            sfxPlayer.PlayOneShot(audioClip);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }

    public void JumpSound()
    {
        PlaySound(1);
    }

    public void InteractableSound()
    {
        PlaySound(3);
    }

    public void RestartBgMusic()
    {
        SetMusicToggle(false);
        SetMusicToggle(true);
    }

    public void MuteSFX(bool mute)
    {
        if (sfxPlayer != null)
        {
            sfxPlayer.mute = mute;
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (musicPlayer != null)
        {
            musicPlayer.volume = Mathf.Clamp01(volume); // Clamp volume between 0 and 1
        }
    }

    public float GetMusicVolume()
    {
        return musicPlayer != null ? musicPlayer.volume : 0;
    }

    public void SetMusicPlayback(AudioClip audioClip)
    {
        if (musicPlayer == null || audioClip == null) return;

        musicPlayer.Stop();
        musicPlayer.clip = audioClip;
        musicPlayer.Play();
    }
}
