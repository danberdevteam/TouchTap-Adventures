using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphHandler : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject graphPrefab;
    JsonDataLoader dataLoader;
    // [SerializeField] GameObject graphHolder;
    List<List<MinigameStats>> minigameStats = new List<List<MinigameStats>>();
    List<string> sessionNumber = new List<string>();
    List<List<float>> InCorrectClicks = new List<List<float>>();
    List<List<float>> Accuracy = new List<List<float>>();
    List<List<float>> TimeSpent = new List<List<float>>();
    public int numberOfMinigames;

    public int miniGameNumber;



    void Start()
    {
        dataLoader = FindObjectOfType<JsonDataLoader>();
        int session = 0;

        // print(dataLoader.minigameStats.Count + "graph sessions number");
        //Loading sessionwise data
        foreach (var stat in dataLoader.minigameStats)
        {
            sessionNumber.Add("Session " + session);
            // print(sessionNumber[session] + "graph sessions");
            minigameStats.Add(stat);
            session++;
        }

        for (int k = 0; k < numberOfMinigames; k++)
        {

            InCorrectClicks.Add(new List<float>());
            TimeSpent.Add(new List<float>());
            Accuracy.Add(new List<float>());

            for (int i = 0; i < minigameStats.Count; i++)
            {
                if (minigameStats[i].Count >= k + 1)
                {
                    InCorrectClicks[k].Add(minigameStats[i][k].IncorrectClicks);
                    // print(minigameStats[i][k].IncorrectClicks + "CorrectClicks" + k);
                    TimeSpent[k].Add(minigameStats[i][k].timeSpent);
                    float accuracy;
                    if (minigameStats[i][k].wrongColorClicked != -1)
                    {
                        accuracy = minigameStats[i][k].CorrectClicks * 100 / (minigameStats[i][k].CorrectClicks + minigameStats[i][k].IncorrectClicks + minigameStats[i][k].wrongColorClicked);
                    }
                    else
                    {
                        if ((minigameStats[i][k].CorrectClicks + minigameStats[i][k].IncorrectClicks) == 0)
                        {
                            accuracy = 0;
                        }
                        else
                        {
                            accuracy = minigameStats[i][k].CorrectClicks * 100 / (minigameStats[i][k].CorrectClicks + minigameStats[i][k].IncorrectClicks);
                        }
                    }
                    Accuracy[k].Add(accuracy);
                }
                else
                {
                    InCorrectClicks[k].Add(-1);
                    TimeSpent[k].Add(-1);
                    Accuracy[k].Add(-1);
                }

            }

        }
        // Instantiate(this,graphHolder.transform);

        GameObject instantiatedGraphIncorrect = Instantiate(graphPrefab, this.transform);
        instantiatedGraphIncorrect.GetComponent<GraphData>().InCorrectClicks = InCorrectClicks[miniGameNumber];
        instantiatedGraphIncorrect.GetComponent<GraphData>().graphType = GraphType.IncorrectClicks;
        for (int i = 0; i < InCorrectClicks.Count; i++)
        {
            instantiatedGraphIncorrect.GetComponent<GraphData>().SessionNumber = sessionNumber;
        }

        GameObject instantiatedGraphTimeSpent = Instantiate(graphPrefab, this.transform);
        instantiatedGraphTimeSpent.GetComponent<GraphData>().TimeSpent = TimeSpent[miniGameNumber];
        instantiatedGraphTimeSpent.GetComponent<GraphData>().graphType = GraphType.TImeSpent;
        for (int i = 0; i < TimeSpent.Count; i++)
        {
            instantiatedGraphTimeSpent.GetComponent<GraphData>().SessionNumber = sessionNumber;
        }


        GameObject instantiatedGraphAccuracy = Instantiate(graphPrefab, this.transform);
        instantiatedGraphAccuracy.GetComponent<GraphData>().Accuracy = Accuracy[miniGameNumber];
        instantiatedGraphAccuracy.GetComponent<GraphData>().graphType = GraphType.Accuracy;
        for (int i = 0; i < Accuracy.Count; i++)
        {
            instantiatedGraphAccuracy.GetComponent<GraphData>().SessionNumber = sessionNumber;
        }


    }

    // Update is called once per frame
    void Update()
    {
        // print(dataLoader.minigameStats.Count + "graph sessions number");
    }
}
