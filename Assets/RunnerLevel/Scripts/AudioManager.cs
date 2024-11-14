using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            return instance;
        }
    }
    
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioSource sfxPlayer;
    [Space(15)]
    [SerializeField] private AudioClip backgroundMusicClip;
    
    [SerializeField] private AudioClip[] sfxSoundClips;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void Start()
    {
        SetMusicToggle(true);
        SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
    }

    private void SceneManagerOnsceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        SetMusicToggle(true);
        
    }

    public void SettingToggleMusic(bool toggle)
    {
        if (toggle) //music on
        {
            PlayerPrefs.SetInt("musicToggle", 1);
            
        }
        else //music off
        {
            PlayerPrefs.SetInt("musicToggle", 0);
            
        }
        SetMusicToggle(toggle);
    }
    
    public void SettingToggleSFX(bool toggle)
    {
        if (toggle) //music on
        {
            PlayerPrefs.SetInt("sfxToggle", 1);
            
        }
        else //music off
        {
            PlayerPrefs.SetInt("sfxToggle", 0);
            
        }

        if (sfxPlayer)
        {
            if (toggle == true)
            {
                sfxPlayer.mute = false;
            }
            else
            {
                sfxPlayer.mute = true;
            }
        }
    }

    public void SetMusicToggle(bool toggle)
    {
        if (backgroundMusicClip != null)
        {
            musicPlayer.loop = true;
        }
        else
        {
            return;
        }

        if (toggle == true)
        {
            musicPlayer.clip = backgroundMusicClip;
            musicPlayer.Play();
        }
        else
        {
            musicPlayer.Stop();
        }
    }

    public void SetMusicPlayback(AudioClip audioClip)
    {
        if (musicPlayer == null && audioClip == null)    return;
        musicPlayer.Stop();
        musicPlayer.clip = audioClip;
        musicPlayer.Play();
    }
    
    private void PlaySound(int idx)
    {
        try
        {
            sfxPlayer.PlayOneShot(sfxSoundClips[idx]);
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogWarning(e.Message);
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
            UnityEngine.Debug.LogWarning(e.Message);
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

    public static implicit operator AudioManager(Audio_Manager v)
    {
        throw new NotImplementedException();
    }
}
