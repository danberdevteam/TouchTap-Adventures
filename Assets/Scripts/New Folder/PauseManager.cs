using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel; // Reference to the Pause Panel
    public Button pauseButton;    // Reference to the Pause Button
    public Button playButton;     // Reference to the Play Button

    public UnityEngine.UI.Slider volumeSlider;   // Reference to the Volume Slider

    private bool isPaused = false;

    public void Start()
    {
        // Initialize slider value with current volume
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioManager.Instance.Volume;

            // Add listener to update volume when slider changes
            volumeSlider.onValueChanged.AddListener(value =>
            {
                AudioManager.Instance.Volume = value;
            });
        }

        pausePanel.SetActive(false);
        pauseButton.onClick.AddListener(PauseGame);
        playButton.onClick.AddListener(ResumeGame);
    }

    // Function to pause the game
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0; // Pause the game
        pausePanel.SetActive(true); // Show the panel
        // Mute SFX when paused
        // AudioManager.Instance.MuteSFX(true);
    }

    // Function to resume the game
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1; // Resume the game
        pausePanel.SetActive(false); // Hide the panel

        // Unmute SFX if it was muted during pause
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.MuteSFX(false);
        }
    }


    // Function to adjust the music volume
    private void AdjustVolume(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
        PlayerPrefs.SetFloat(GameConstants.VolumeKey, value);
    }
}
