using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class GameConstants
{
    public const string VolumeKey = "AudioVolume";
}

public class SettingsMenu : MonoBehaviour
{
    public AccountDeletionManager accountDeletionManager;
    [SerializeField] private GameObject controlsPanel; // Dynamic reference to the controls panel
    [SerializeField] private UnityEngine.UI.Slider volumeSlider;      // Dynamic reference to the volume slider

    public void Start()
    {
        StartCoroutine(WaitForAudioManager());
    }

    private IEnumerator WaitForAudioManager()
    {
        while (AudioManager.Instance == null)
        {
            Debug.LogWarning("Waiting for AudioManager instance...");
            yield return null; // Wait for the next frame
        }

        if (volumeSlider != null)
        {
            volumeSlider.value = AudioManager.Instance.Volume;

            // Add listener to update volume when slider changes
            volumeSlider.onValueChanged.AddListener(value =>
            {
                Debug.Log($"Slider changed: {value}");
                AudioManager.Instance.Volume = value;
            });
        }
        else
        {
            Debug.LogError("Volume Slider is not assigned in SettingsMenu.");
        }
    }

    public void Update()
    {
        // Continuously check if the controls panel has been activated
        if (controlsPanel != null && controlsPanel.activeInHierarchy && volumeSlider == null)
        {
            AssignVolumeSlider();
            InitializeVolume();
        }
    }

    private void AssignVolumeSlider()
    {
        // Ensure the controls panel is active
        if (controlsPanel == null || !controlsPanel.activeInHierarchy)
        {
            Debug.Log("Controls panel is not active in the scene.");
            return;
        }

        // Find the slider within the controls panel
        volumeSlider = controlsPanel.GetComponentInChildren<UnityEngine.UI.Slider>();

        if (volumeSlider == null)
        {
            Debug.LogError("No Slider component found within the active controls panel.");
            return;
        }

        // Add a listener to handle volume changes
        volumeSlider.onValueChanged.RemoveAllListeners(); // Avoid duplicate listeners
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    private void InitializeVolume()
    {
        if (volumeSlider == null) return;

        float savedVolume = PlayerPrefs.GetFloat(GameConstants.VolumeKey, 0.5f);

        volumeSlider.value = savedVolume;

        if (Audio_Manager.instance != null)
        {
            Audio_Manager.instance.SetVolume(savedVolume);
        }
    }

    private void OnVolumeChanged(float value)
    {
        IAPManager.instance.ClearRestoreMessage();
        if (Audio_Manager.instance != null)
        {
            Audio_Manager.instance.SetVolume(value);
        }

        PlayerPrefs.SetFloat(GameConstants.VolumeKey, value);
    }

    public void OnDeleteAccountButtonClick()
    {
        IAPManager.instance.ClearRestoreMessage();
        accountDeletionManager.ShowConfirmationPanel();
    }
}
