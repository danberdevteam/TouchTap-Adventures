using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayfabManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Login();
        // AttemptAutoLogin();
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Login Successful : " + result.SessionTicket);
        // SaveAuthToken(result.SessionTicket);
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error Occured due to " + error);
        Debug.Log(error.GenerateErrorReport());
    }

    // public void SaveAuthToken(string token)
    // {
    //     print("AAAAAAA");
    //     PlayerPrefs.SetString("userToken", token);
    //     PlayerPrefs.Save();
    // }

    // public string LoadAuthToken()
    // {
    //     return PlayerPrefs.HasKey("userToken") ? PlayerPrefs.GetString("userToken") : string.Empty;
    // }

    // void LoginWithToken(string token)
    // {
    //     print("Auto Login with token");
    //     PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest
    //     {
    //         CustomId = token,
    //         CreateAccount = true // Set to false if you do not want to create a new account if the token does not exist
    //     }, result =>
    //     {
    //         Debug.Log("Logged in with token");
    //         // Proceed with your game logic
    //     }, error =>
    //     {
    //         Debug.Log("Error logging in with token: " + error.ErrorMessage);
    //         // Handle error, possibly ask for login again
    //     });
    // }

    // private void AttemptAutoLogin()
    // {
    //     string storedToken = LoadAuthToken();
    //     print("Auto Login : " + storedToken);
    //     if (!string.IsNullOrEmpty(storedToken))
    //     {
    //         LoginWithToken(storedToken);
    //     }
    //     else
    //     {
    //         // Prompt login UI or handle guest login
    //     }
    // }

}
