using System.Collections;
using System.Collections.Generic;
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
        if (result.InfoResultPayload.PlayerProfile.DisplayName == usernameInputLogin.text)
        {
            Debug.Log("Successful login.");
            PlayerPrefs.DeleteKey(keyValue);
            UserManager.Instance.IsLoggedIn = true;
            UserManager.Instance.DisplayName = result.InfoResultPayload.PlayerProfile.DisplayName;
            SceneManager.LoadScene("MenuScene");
        }
        else
        {
            messageText.text = "Username does not match.";
            messageText.color = Color.red;
            loginButton.GetComponent<LeanShake>().Shake(10);
        }
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Registered..\n" + "You can Login now.";
        messageText.color = Color.green;
        registerPortal.SetActive(false);

    }

    void OnErrorLogin(PlayFabError error)
    {
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
    // void Start()
    // {

    // }

    // // Update is called once per frame
    // void Update()
    // {

    // }
}
