using System;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ParentalManager : MonoBehaviour
{
    [SerializeField] public JsonDataLoader dataCollector;
    [SerializeField] GameObject SessionsTabHolder;
    [SerializeField] GameObject SessionsTabButton;
    [SerializeField] GameObject graphHolderPrefab;
    [SerializeField] GameObject GraphHolder;
    [SerializeField] GameObject GraphSwitcherTabHolder;
    [SerializeField] GameObject GraphSwitcherTabprefab;
    [SerializeField] TMP_Text TitleText;
    public int numberOfMinigames;

    [SerializeField] String[] nameOfMiniGames;

    [SerializeField] GraphSwitchManager graphSwitchManager;



    void Start()
    {
        if (!UserManager.Instance.IsLoggedIn && PlayerPrefs.HasKey("DisplayName"))
        {
            UserManager.Instance.DisplayName = PlayerPrefs.GetString("DisplayName");
            UserManager.Instance.IsLoggedIn = true; // Make sure to validate this state is correct!
        }

        TitleText.text = UserManager.Instance.DisplayName + "'s Info ";
        Debug.Log("Display Name: " + UserManager.Instance.DisplayName);
        // TitleText.text = PlayerPrefs.GetString("userName", usernameInputLogin.text) + "'s Info ";
        for (int i = 0; i < dataCollector.minigameStats.Count; i++)
        {
            String sessionNumber = "Session " + i;
            GameObject tab = Instantiate(SessionsTabButton, SessionsTabHolder.transform);
            tab.GetComponentInChildren<TMP_Text>().text = sessionNumber;
        }

        for (int i = 0; i < numberOfMinigames; i++)
        {
            GameObject switcherInstantiated = Instantiate(graphHolderPrefab, GraphHolder.transform);
            switcherInstantiated.GetComponent<GraphHandler>().numberOfMinigames = numberOfMinigames;
            switcherInstantiated.GetComponent<GraphHandler>().miniGameNumber = i;
            Instantiate(GraphSwitcherTabprefab, GraphSwitcherTabHolder.transform).GetComponentInChildren<TMP_Text>().text = nameOfMiniGames[i];
        }
        // print("ParentalManagercalledde");
        graphSwitchManager.gameObject.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
