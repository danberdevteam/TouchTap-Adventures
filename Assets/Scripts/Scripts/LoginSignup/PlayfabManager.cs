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
        Debug.Log("Login Successful");
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error Occured due to " + error);
        Debug.Log(error.GenerateErrorReport());
    }
}
