using System.Collections;
using System.Collections.Generic;
using PlayFab;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseManager : MonoBehaviour
{
    public IAPManager manager;
    public MenuUI menuUI;
    [SerializeField] Button dashboardLockedButton;
    [SerializeField] Button leaderBoardLockedButton;
    public bool isDashboardPurchased = false;

    public const string DASHBOARD = "menuitem1";
    public const string LEADERBOARD = "menuitem2";

    void OnEnable()
    {
        IAPManager.OnDashboardPurchaseSuccess += ButtonAddListnerConditionDashboard;
        IAPManager.OnLeaderboardPurchaseSuccess += ButtonAddListnerConditionLeaderboard;
    }

    void OnDisable()
    {
        IAPManager.OnDashboardPurchaseSuccess -= ButtonAddListnerConditionDashboard;
        IAPManager.OnLeaderboardPurchaseSuccess -= ButtonAddListnerConditionLeaderboard;
    }

    // void DashboardButtonClick()
    // {
    //     Debug.Log("Dashboard purchased successfully!");
    //     menuUI.LoadDashBoard();
    // }
    // Start is called before the first frame update
    void Start()
    {
        dashboardLockedButton.onClick.AddListener(DashboardButtonClick);
        leaderBoardLockedButton.onClick.AddListener(LeaderboardButtonClick);
        FetchPurchaseStateFromPlayFab();
    }

    void DashboardButtonClick()
    {
        // Pass the button to the IAPManager for processing
        manager.BuyDashboard(dashboardLockedButton);
    }
    void LeaderboardButtonClick()
    {
        manager.BuyLeaderboard(leaderBoardLockedButton);
    }



    void ButtonAddListnerConditionDashboard()
    {
        dashboardLockedButton.onClick.AddListener(menuUI.LoadDashBoard);
    }
    void ButtonAddListnerConditionLeaderboard()
    {
        leaderBoardLockedButton.onClick.AddListener(menuUI.LoadLeaderBoard);
    }




    private void FetchPurchaseStateFromPlayFab()
    {
        PlayFabClientAPI.GetUserData(new PlayFab.ClientModels.GetUserDataRequest(),
        result =>
        {
            if (result.Data != null)
            {
                if (result.Data.ContainsKey(DASHBOARD) && result.Data[DASHBOARD].Value == "purchased")
                {
                    UnlockButton(dashboardLockedButton);
                    ButtonAddListnerConditionDashboard();
                }

                if (result.Data.ContainsKey(LEADERBOARD) && result.Data[LEADERBOARD].Value == "purchased")
                {
                    UnlockButton(leaderBoardLockedButton);
                    ButtonAddListnerConditionLeaderboard();
                }
            }
        },
        error => Debug.LogError($"Failed to fetch purchase state from PlayFab: {error.ErrorMessage}"));
    }


    public void UnlockButton(Button buttonToUnLock)
    {
        buttonToUnLock.GetComponentsInChildren<Image>()[1].gameObject.SetActive(false);
        buttonToUnLock.GetComponentsInChildren<TMP_Text>()[0].gameObject.SetActive(false);
        buttonToUnLock.onClick.RemoveAllListeners();
    }




    // Update is called once per frame
    // void Update()
    // {
    //     if(isDashboardPurchased)
    //     {
    //         dashboardLockedButton.onClick.AddListener(ButtonAddListnerConditionDashboard);
    //     }
    // }
}
