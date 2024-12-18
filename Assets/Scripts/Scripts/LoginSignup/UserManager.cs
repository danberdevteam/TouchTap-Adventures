using UnityEngine;
using PlayFab;

public class UserManager : MonoBehaviour
{
    public static UserManager Instance;

    public string DisplayName { get; set; }
    public bool IsLoggedIn { get; set; }

    public static bool IsLogoutTriggered = false;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        print("Display Name : " + DisplayName);
    }

    // public void OnLoginSuccess(LoginResult result)
    // {
    //     IsLoggedIn = true;
    //     PlayFabId = result.PlayFabId;
    //     DisplayName = result.InfoResultPayload.AccountInfo.Username;
    //     Debug.Log("Logged In. User: " + DisplayName);
    // }

    // public void OnRegisterSuccess(RegisterPlayFabUserResult result)
    // {
    //     IsLoggedIn = true;
    //     PlayFabId = result.PlayFabId;
    //     DisplayName = result.Username;
    //     Debug.Log("Registration Successful. User: " + DisplayName);
    // }

    // public void OnLogout()
    // {
    //     IsLoggedIn = false;
    //     DisplayName = string.Empty;
    //     PlayFabId = string.Empty;
    // }

    public void Logout(System.Action onLogoutSuccess, System.Action<string> onLogoutFailure)
    {
        try
        {
            // Clear PlayFab credentials (clear local session ticket and any cached data)
            PlayFabClientAPI.ForgetAllCredentials();

            // Optional: Clear cached user data
            PlayerPrefs.DeleteKey("PlayFabUserId");
            PlayerPrefs.DeleteKey("PlayFabSessionTicket");
            PlayerPrefs.DeleteKey("userEmail");
            PlayerPrefs.DeleteKey("userToken");

            // Set logout trigger flag
            IsLogoutTriggered = true;

            // Reset user-specific data in your application
            IsLoggedIn = false;
            DisplayName = string.Empty;

            Debug.Log("User logged out successfully!");
            onLogoutSuccess?.Invoke();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Logout failed: {ex.Message}");
            onLogoutFailure?.Invoke(ex.Message);
        }
    }
}
