using UnityEngine;
using PlayFab;
using UnityEngine.UI;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;
using Lean.Gui;

public class Login : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text messageText;
    public TMP_Text TitleID;
    public TMP_Text messageRegisterText;
    public TMP_InputField emailInputLogin;
    public TMP_InputField usernameInputLogin;
    public TMP_InputField emailInputRegister;
    public TMP_InputField passwordInputRegister;
    public TMP_InputField nameInput;
    public GameObject registerPortal;
    public Button loginButton;
    public Button registerButton;

    void Start()
    {
        UserManager.IsLogoutTriggered = false; // Reset the logout trigger on app launch
        AttemptAutoLogin();

        // Add listeners to clear message text when input fields are modified
        emailInputLogin.onValueChanged.AddListener(delegate { ClearMessageText(); });
        usernameInputLogin.onValueChanged.AddListener(delegate { ClearMessageText(); });
        emailInputRegister.onValueChanged.AddListener(delegate { ClearMessageRegisterText(); });
        nameInput.onValueChanged.AddListener(delegate { ClearMessageRegisterText(); });
    }

    private void ClearMessageText()
    {
        messageText.text = string.Empty;
        messageText.color = Color.black; // Reset to default color if needed
    }

    private void ClearMessageRegisterText()
    {
        messageRegisterText.text = string.Empty;
        messageRegisterText.color = Color.black; // Reset to default color if needed
    }

    public void RegisterButton()
    {
        var request = new RegisterPlayFabUserRequest
        {
            Username = nameInput.text,
            DisplayName = nameInput.text,
            Email = emailInputRegister.text,
            Password = "mail123",
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnErrorRegister);
    }

    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInputLogin.text,
            Password = "mail123",
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true,
                GetUserData = true // Request user data to check for deletion flag
            }
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnErrorLogin);
    }

    void OnLoginSuccess(LoginResult result)
    {
        // Check if the account is marked for deletion
        if (result.InfoResultPayload.UserData != null &&
            result.InfoResultPayload.UserData.ContainsKey("AccountMarkedForDeletion"))
        {
            Debug.Log("Account is marked for deletion. Preventing login.");
            messageText.text = "Your account is marked for deletion. Contact support for more details.";
            messageText.color = Color.black;

            // Optionally log the user out
            PlayFabClientAPI.ForgetAllCredentials();
            return;
        }

        Debug.Log("Successful login.");
        string displayName = result.InfoResultPayload.PlayerProfile.DisplayName;
        string email = emailInputLogin.text;

        if (!string.IsNullOrEmpty(email))
        {
            // Store user data in PlayerPrefs
            PlayerPrefs.SetString("DisplayName", displayName);
            PlayerPrefs.SetString("userEmail", email);
            PlayerPrefs.SetString("userToken", result.SessionTicket);
        }

        UserManager.Instance.IsLoggedIn = true;
        UserManager.Instance.DisplayName = displayName;

        SceneManager.LoadScene("MenuScene");
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Registered..\nYou can login now.";
        messageText.color = Color.green;

        // Clear the login fields
        emailInputLogin.text = string.Empty;
        usernameInputLogin.text = string.Empty;

        // Clear the registration fields
        emailInputRegister.text = string.Empty;
        nameInput.text = string.Empty;

        // Hide the registration portal
        registerPortal.SetActive(false);
    }


    void OnErrorLogin(PlayFabError error)
    {
        Debug.LogError($"Login Error: {error.GenerateErrorReport()}");

        switch (error.Error)
        {
            case PlayFabErrorCode.InvalidEmailAddress:
                messageText.text = "Invalid email address.";
                break;
            case PlayFabErrorCode.AccountNotFound:
                messageText.text = "Account not found. Please register first.";
                break;
            case PlayFabErrorCode.InvalidPassword:
                messageText.text = "Invalid password. Please try again.";
                break;
            case PlayFabErrorCode.EmailAddressNotAvailable:
                messageText.text = "This email is already associated with an account.";
                break;
            default:
                messageText.text = "Login failed. Please check your input.";
                break;
        }

        messageText.color = Color.black;
        loginButton.GetComponent<LeanShake>().Shake(10);
    }

    void OnErrorRegister(PlayFabError error)
    {
        Debug.LogError($"Register Error: {error.GenerateErrorReport()}");

        // Extract the specific message from the error report
        string errorMessage = ExtractErrorMessage(error.GenerateErrorReport());

        switch (error.Error)
        {
            case PlayFabErrorCode.InvalidEmailAddress:
                messageRegisterText.text = errorMessage;
                break;
            case PlayFabErrorCode.UsernameNotAvailable:
                messageRegisterText.text = errorMessage;
                break;
            case PlayFabErrorCode.InvalidUsername:
                messageRegisterText.text = errorMessage;
                break;
            case PlayFabErrorCode.InvalidPassword:
                messageRegisterText.text = errorMessage;
                break;
            case PlayFabErrorCode.EmailAddressNotAvailable:
                messageRegisterText.text = errorMessage;
                break;
            default:
                messageRegisterText.text = errorMessage;
                break;
        }

        messageRegisterText.color = Color.black;
        registerButton.GetComponent<LeanShake>().Shake(10);
    }

    // Function to extract the specific error message
    private string ExtractErrorMessage(string fullErrorReport)
    {
        int colonIndex = fullErrorReport.LastIndexOf(":");
        if (colonIndex != -1 && colonIndex + 2 < fullErrorReport.Length)
        {
            return fullErrorReport.Substring(colonIndex + 2).Trim();
        }
        return fullErrorReport; // Fallback in case parsing fails
    }

    public void SaveAuthToken(string token)
    {
        PlayerPrefs.SetString("userToken", token);
        PlayerPrefs.Save();
    }

    public string LoadAuthToken()
    {
        return PlayerPrefs.HasKey("userToken") ? PlayerPrefs.GetString("userToken") : string.Empty;
    }

    private void AttemptAutoLogin()
    {
        if (UserManager.IsLogoutTriggered)
        {
            Debug.Log("Auto-login skipped due to logout trigger.");
            return;
        }

        if (PlayerPrefs.HasKey("userEmail"))
        {
            string storedEmail = PlayerPrefs.GetString("userEmail");

            if (!string.IsNullOrEmpty(storedEmail))
            {
                var request = new LoginWithEmailAddressRequest
                {
                    Email = storedEmail,
                    Password = "mail123",
                    InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                    {
                        GetPlayerProfile = true,
                        GetUserData = true
                    }
                };

                PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnErrorLogin);
            }
        }
        else
        {
            Debug.Log("No stored credentials for auto-login.");
        }
    }
}
