using UnityEngine;
using PlayFab;
using UnityEngine.UI;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;
using Lean.Gui;


public class Login : MonoBehaviour
{
    // Start is called before the first frame update

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


    public void Start()
    {
        AttemptAutoLogin();
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
                GetPlayerProfile = true
            }
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnErrorLogin);

    }
    string keyValue = "gameNumber";

    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Successful login.");
        string displayName = result.InfoResultPayload.PlayerProfile.DisplayName;
        string email = emailInputLogin.text;  // Capture the email from the input field

        if (!string.IsNullOrEmpty(email))
        {
            // Store user data in PlayerPrefs
            PlayerPrefs.SetString("DisplayName", displayName);
            PlayerPrefs.SetString("userEmail", email);
            PlayerPrefs.SetString("userToken", result.SessionTicket);
            PlayerPrefs.Save();

            Debug.Log("Email stored: " + PlayerPrefs.GetString("userEmail"));  // Confirm storage immediately
        }
        else
        {
            Debug.LogError("Email input is empty. Cannot save to PlayerPrefs.");
        }

        UserManager.Instance.IsLoggedIn = true;
        UserManager.Instance.DisplayName = displayName;

        SceneManager.LoadScene("MenuScene");  // Consider the impact of scene loading on data access
        // LootLockerSDKManager.StartGoogleSession(result.PlayFabId, (response) =>
        // {
        //     if (response.success)
        //     {
        //         Debug.Log("LootLocker session started successfully." + "Player ID: " + response.player_id);
        //     }
        //     else
        //     {
        //         Debug.LogError("Failed to start LootLocker session: " + response.errorData);
        //     }
        // });


    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Registered..\n" + "You can Login now.";
        messageText.color = Color.green;
        registerPortal.SetActive(false);

    }

    void OnErrorLogin(PlayFabError error)
    {
        print("Errror : " + error);
        messageText.text = "Invalid parameters";
        messageText.color = Color.red;
        loginButton.GetComponent<LeanShake>().Shake(10);
    }
    void OnErrorRegister(PlayFabError error)
    {
        messageRegisterText.text = "Invalid parameters";
        messageText.color = Color.red;
        registerButton.GetComponent<LeanShake>().Shake(10);
    }





    public void SaveAuthToken(string token)
    {
        print("AAAAAAA : " + token);
        PlayerPrefs.SetString("userToken", token);
        PlayerPrefs.Save();
    }

    public string LoadAuthToken()
    {
        return PlayerPrefs.HasKey("userToken") ? PlayerPrefs.GetString("userToken") : string.Empty;
    }

    private void AttemptAutoLogin()
    {
        if (PlayerPrefs.HasKey("userEmail"))
        {
            string storedEmail = PlayerPrefs.GetString("userEmail");
            Debug.Log("Attempting auto-login with stored email: " + storedEmail);

            if (!string.IsNullOrEmpty(storedEmail))
            {
                var request = new LoginWithEmailAddressRequest
                {
                    Email = storedEmail,
                    Password = "mail123",  // Reminder to handle passwords securely
                    InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                    {
                        GetPlayerProfile = true
                    }
                };
                emailInputLogin.text = storedEmail;

                PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnErrorLogin);
            }
            else
            {
                Debug.LogError("Stored email is empty. Cannot proceed with auto-login.");
            }
        }
        else
        {
            Debug.Log("No email stored in PlayerPrefs. Prompting manual login.");
        }
    }

}







