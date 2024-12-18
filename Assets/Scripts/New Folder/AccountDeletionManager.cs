using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels; // For Client API
using System.Collections.Generic;
using UnityEngine.SceneManagement; // For Dictionary

public class AccountDeletionManager : MonoBehaviour
{
    public GameObject confirmationPanel; // Reference to the Confirmation Panel
    public Button confirmButton;         // Confirm button
    public Button cancelButton;          // Cancel button

    private string playFabId; // Variable to store the logged-in player's PlayFab ID

    void Start()
    {
        // Attach button click listeners
        confirmButton.onClick.AddListener(OnConfirmDeletion);
        cancelButton.onClick.AddListener(OnCancelDeletion);

        // Ensure the panel is inactive initially
        confirmationPanel.SetActive(false);

        // Fetch the logged-in player's PlayFab ID
        FetchCurrentPlayerPlayFabId();
    }

    public void ShowConfirmationPanel()
    {
        confirmationPanel.SetActive(true);
    }

    private void OnConfirmDeletion()
    {
        confirmationPanel.SetActive(false);

        if (!string.IsNullOrEmpty(playFabId))
        {
            // Proceed with account deletion
            DeleteAccount();
        }
        else
        {
            Debug.LogError("PlayFab ID is null or empty. Cannot delete account.");
        }
    }

    private void OnCancelDeletion()
    {
        confirmationPanel.SetActive(false);
    }

    private void DeleteAccount()
    {
        // Step 1: Mark the account for deletion
        MarkAccountForDeletion(() =>
        {
            // Step 2: Delete user data
            DeleteUserData(() =>
            {
                // Step 3: Log out the user
                LogoutUser();
            });
        });
    }

    private void MarkAccountForDeletion(System.Action onSuccess)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "AccountMarkedForDeletion", "true" }
            }
        },
        result =>
        {
            Debug.Log("Account successfully marked for deletion.");
            onSuccess?.Invoke();
        },
        error =>
        {
            Debug.LogError($"Failed to mark account for deletion: {error.ErrorMessage}");
        });
    }

    private void DeleteUserData(System.Action onSuccess)
    {
        // Replace "Key1" and "Key2" with actual keys to remove, or keep it empty if removing all keys
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            KeysToRemove = new List<string> { "", "" }
        },
        result =>
        {
            Debug.Log("User data deleted successfully.");
            onSuccess?.Invoke();
        },
        error =>
        {
            Debug.LogError($"Failed to delete user data: {error.ErrorMessage}");
        });
    }

    private void LogoutUser()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        Debug.Log("User logged out.");
        
        // Optionally, you can navigate the user to the login screen or exit the app
        SceneManager.LoadScene("LoginScene");
    }

    private void FetchCurrentPlayerPlayFabId()
    {
        var request = new GetAccountInfoRequest();

        PlayFabClientAPI.GetAccountInfo(request,
            result =>
            {
                playFabId = result.AccountInfo.PlayFabId;
                Debug.Log($"Retrieved PlayFab ID: {playFabId}");
            },
            error =>
            {
                Debug.LogError($"Failed to retrieve PlayFab ID: {error.ErrorMessage}");
            });
    }
}
