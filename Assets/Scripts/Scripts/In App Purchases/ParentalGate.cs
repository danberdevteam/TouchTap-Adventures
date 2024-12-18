using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class ParentalGate : MonoBehaviour
{
    // Singleton instance
    public static ParentalGate Instance { get; private set; }

    public GameObject parentalGatePanel; // The UI Panel for the parental gate
    public TMP_InputField answerInputField; // Input field for the answer
    public TMP_Text questionText;           // Text to display the question
    public Button submitButton;         // Button to submit the answer
    public Button cancelButton;         // Button to cancel the parental gate

    private int correctAnswer;

    // Store the state of other buttons to restore later
    private Button[] allButtons;
    private Dictionary<Button, bool> buttonInteractableStates = new Dictionary<Button, bool>();

    private void Awake()
    {
        // Implement the Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy the duplicate instance
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: Keep the instance across scenes

        // Subscribe to scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe from scene loaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Refresh button references when a new scene is loaded
        RefreshButtons();
    }

    void Start()
    {
        // Generate a random math problem
        GenerateMathProblem();

        // Add listener for the Submit button
        submitButton.onClick.AddListener(VerifyAnswer);
        cancelButton.onClick.AddListener(CancelParentalGate);

        // Find all buttons in the scene
        allButtons = FindObjectsOfType<Button>();
    }

    void GenerateMathProblem()
    {
        int number1 = Random.Range(1, 10); // Random number between 1 and 10
        int number2 = Random.Range(1, 10);
        correctAnswer = number1 + number2;

        // Display the question
        questionText.text = $"What is {number1} + {number2}?";
    }

    void VerifyAnswer()
    {
        // Check if the entered answer is correct
        if (int.TryParse(answerInputField.text, out int userAnswer) && userAnswer == correctAnswer)
        {
            Debug.Log("Access Granted");
            parentalGatePanel.SetActive(false); // Hide the parental gate
            EnableOtherButtons(); // Re-enable other buttons
            // Proceed to the restricted content
        }
        else
        {
            Debug.Log("Access Denied");
            answerInputField.text = ""; // Clear the input field
        }
    }

    public void ShowParentalGate(System.Action onAccessGranted)
    {
        parentalGatePanel.SetActive(true); // Show the parental gate panel
        DisableOtherButtons(); // Disable other buttons

        // Modify the VerifyAnswer method to include the callback
        submitButton.onClick.RemoveAllListeners(); // Clear any existing listeners
        submitButton.onClick.AddListener(() =>
        {
            if (int.TryParse(answerInputField.text, out int userAnswer) && userAnswer == correctAnswer)
            {
                Debug.Log("Access Granted");
                parentalGatePanel.SetActive(false); // Hide the parental gate
                onAccessGranted?.Invoke(); // Execute the callback if access is granted
            }
            else
            {
                Debug.Log("Access Denied");
                answerInputField.text = ""; // Clear the input field
            }
        });

        // Set up the Cancel button to close the panel
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(CancelParentalGate);
    }

    public void CancelParentalGate()
    {
        Debug.Log("Parental Gate Cancelled");
        parentalGatePanel.SetActive(false); // Hide the parental gate panel
        EnableOtherButtons(); // Re-enable other buttons
    }

    public void ResetParentalGate()
    {
        // Reset the input field and question
        answerInputField.text = ""; 
        GenerateMathProblem(); // Generate a new question
        Debug.Log("Parental Gate has been reset.");
    }

    private void DisableOtherButtons()
    {
        buttonInteractableStates.Clear(); // Clear the previous states

        foreach (Button button in allButtons)
        {
            if (!parentalGatePanel.GetComponentsInChildren<Button>().Contains(button))
            {
                // Save the current interactable state
                buttonInteractableStates[button] = button.interactable;
                // Disable the button
                button.interactable = false;
            }
        }
    }

    private void EnableOtherButtons()
    {
        foreach (var kvp in buttonInteractableStates)
        {
            kvp.Key.interactable = kvp.Value; // Restore the original interactable state
        }

        buttonInteractableStates.Clear(); // Clear stored states
    }

    private void RefreshButtons()
    {
        // Find all buttons in the current scene
        allButtons = FindObjectsOfType<Button>();
        Debug.Log($"Found {allButtons.Length} buttons in the current scene.");
    }
}
