using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class LeaderBoardManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject NameHolder;
    [SerializeField] GameObject AccuracyHolder;
    [SerializeField] TMP_Text Name;
    [SerializeField] TMP_Text Accuracy;


    public int numberOfMinigames;
    void Start()
    {



        GetLeaderboard();
        // if (NameHolder.GetComponentsInChildren<TMP_Text>().Length > 3)
        // {
        //     for (int i = 0; i < 3; i++)
        //     {
        //         NameHolder.GetComponentsInChildren<TMP_Text>()[i].GetComponent<ColorLooper>().enabled = true;
        //         AccuracyHolder.GetComponentsInChildren<TMP_Text>()[i].GetComponent<ColorLooper>().enabled = true;
        //     }
        // }

    }

    // Update is called once per frame
    void Update()
    {

    }

    void GetLeaderboard()
    {
        print("Leaderboard called");
        PlayFabClientAPI.GetLeaderboard(
            new GetLeaderboardRequest
            {
                StatisticName = "overallScore",
                StartPosition = 0, // starting position of leaderboard
                MaxResultsCount = 10 // number of entries you want to retrieve
            },
            result =>
            {
                foreach (var entry in result.Leaderboard)
                {
                    Debug.Log($"Rank: {entry.Position} | Player: {entry.PlayFabId} | Score: {entry.StatValue}");
                    int rank=entry.Position;
                    rank++;
                    TMP_Text name = Instantiate(Name, NameHolder.transform);
                    name.text = entry.DisplayName;
                    name.GetComponentInChildren<LeaderboardName>().rank.text = "#" + rank;
                    Instantiate(Accuracy, AccuracyHolder.transform).text = entry.StatValue.ToString() + "%";
                }
                if (NameHolder.GetComponentsInChildren<TMP_Text>().Length > 0)
                {
                    NameHolder.GetComponentsInChildren<TMP_Text>()[0].GetComponent<ColorLooper>().enabled = true;
                }

            },
            error => Debug.LogError("Error retrieving leaderboard: " + error.GenerateErrorReport())
        );
    }
}
