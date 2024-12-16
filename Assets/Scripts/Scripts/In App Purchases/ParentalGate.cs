using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ParentalGate : MonoBehaviour
{
    public GameObject parentalGatePanel; // The UI Panel for the parental gate
    public TMP_InputField answerInputField; // Input field for the answer
    public TMP_Text questionText;           // Text to display the question
    public Button submitButton;         // Button to submit the answer

    private int correctAnswer;

    private System.Action onGateSuccess; // Callback for when the gate is successfully passed


    void Start()
    {
        // Generate a random math problem
        GenerateMathProblem();

        // Add listener for the Submit button
        submitButton.onClick.AddListener(VerifyAnswer);
    }

    void GenerateMathProblem()
    {
        int number1 = Random.Range(1, 10); // Random number between 1 and 10
        int number2 = Random.Range(1, 10);
        correctAnswer = number1 + number2;

        // Display the question
        questionText.text = $"What is {number1} + {number2}?";
    }

    public void VerifyAnswer()
    {
        if (int.TryParse(answerInputField.text, out int userAnswer) && userAnswer == correctAnswer)
        {
            Debug.Log("Access Granted");
            parentalGatePanel.SetActive(false); // Hide the parental gate panel
            answerInputField.text = ""; // Clear input field
            onGateSuccess?.Invoke(); // Trigger the success callback
        }
        else
        {
            Debug.Log("Access Denied");
            answerInputField.text = ""; // Clear input field
        }
    }

    public void ShowParentalGate(System.Action successCallback)
    {
        onGateSuccess = successCallback;
        GenerateMathProblem();
        parentalGatePanel.SetActive(true); // Show the parental gate panel
    }

    public void CancelButtonClicked()
    {
        answerInputField.text = ""; // Clear input field
        parentalGatePanel.SetActive(false);
    }
}
