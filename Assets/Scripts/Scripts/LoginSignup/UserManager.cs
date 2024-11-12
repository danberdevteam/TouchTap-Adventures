using UnityEngine;
using PlayFab;

public class UserManager : MonoBehaviour
{
    public static UserManager Instance;

    public string DisplayName { get;  set; }
    public bool IsLoggedIn { get; set; }

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
}
