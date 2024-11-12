using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;

public class DataCollector : MonoBehaviour
{
    public static DataCollector Instance { get; private set; }
    public SessionData CurrentSession { get; private set; }
    public AllSessions allSessions;

    [Serializable]
    public class AllSessions
    {
        public List<SessionData> sessions;

        public AllSessions()
        {
            sessions = new List<SessionData>();
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAllSessionData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

    }

    private void CreateNewSession()
    {
        if (allSessions == null)
        {
            allSessions = new AllSessions();
        }

        if (allSessions.sessions == null)
        {
            allSessions.sessions = new List<SessionData>();
        }

        CurrentSession = new SessionData
        {
            sessionId = Guid.NewGuid().ToString()
        };

        PlayerPrefs.SetString("currentSessionID", CurrentSession.sessionId);
        allSessions.sessions.Add(CurrentSession);
        SaveAllSessionData();
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("currentSessionID");
    }

    public void AddMinigameStats(MinigameStats stats)
    {
        CurrentSession.minigames.Add(stats);
        SaveAllSessionData();
    }

    public void SaveAllSessionData()
    {
        string json = JsonUtility.ToJson(allSessions, true);
        File.WriteAllText(Application.persistentDataPath + "/allSessionsData.json", json);
        UpdateUserData(json, ReadAndCalculateData()); // Call PlayFab to upload the JSON

    }

    private void LoadAllSessionData()
    {
        string path = Application.persistentDataPath + "/allSessionsData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            allSessions = JsonUtility.FromJson<AllSessions>(json);
            StartCoroutine(RetrieveUserDataFromPlayFab());
        }
        else
        {
            allSessions = new AllSessions(); // Initialize if file doesn't exist
        }
    }

    private void LoadCurrentSession(string sessionId)
    {
        foreach (var session in allSessions.sessions)
        {
            if (session.sessionId == sessionId)
            {
                CurrentSession = session;
                return;
            }
        }
        CreateNewSession(); // Create new session if session ID not found (should not happen)
    }

    private void UpdateUserData(string jsonData, float overallScore)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"allSessionsData", jsonData},
                {"overallScore", overallScore.ToString()}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSendComplete, OnError);
    }
    private IEnumerator RetrieveUserDataFromPlayFab()
    {
        bool dataRetrieved = false;
        string retrievedJson = "";

        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey("allSessionsData"))
            {
                retrievedJson = result.Data["allSessionsData"].Value;
                allSessions = JsonUtility.FromJson<AllSessions>(retrievedJson);
                Debug.Log("Data retrieved successfully.");
            }
            else
            {
                // Initialize AllSessions if the key does not exist
                allSessions = new AllSessions();
                retrievedJson = JsonUtility.ToJson(allSessions, true);
                UpdateUserData(retrievedJson, -1);  // Save initialized AllSessions to PlayFab
                Debug.Log("Initialized and saved new allSessionsData.");
            }
            dataRetrieved = true;
            SaveAllSessionData();
        }, error =>
        {
            Debug.LogError("Error retrieving user data: " + error.ErrorMessage);
            dataRetrieved = true; // Also set true to end the coroutine in case of error
        });

        // Wait until the PlayFab call completes
        yield return new WaitUntil(() => dataRetrieved);

        // Additional code to run after data retrieval
        if (!string.IsNullOrEmpty(retrievedJson))
        {
            Debug.Log("JSON Data: " + retrievedJson);
            // Handle the json data as needed
        }

        if (string.IsNullOrEmpty(PlayerPrefs.GetString("currentSessionID")))
        {
            CreateNewSession();
        }
        else
        {
            LoadCurrentSession(PlayerPrefs.GetString("currentSessionID"));
        }
    }
    private void OnDataSendComplete(UpdateUserDataResult result)
    {
        Debug.Log("Session data updated successfully.");
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError("Error while sending session data: " + error.ErrorMessage);
    }


    private AllSessions sessions;
    public List<List<MinigameStats>> miniGameList = new List<List<MinigameStats>>();
    float overallScore;
    float ReadAndCalculateData()
    {
        overallScore = 0;
        string filePath = Path.Combine(Application.persistentDataPath, "allSessionsData.json");

        // Check if the file exists
        if (File.Exists(filePath))
        {
            // Read the content of the file
            string jsonContent = File.ReadAllText(filePath);

            // Deserialize the JSON content into the AllSessions object
            sessions = JsonUtility.FromJson<AllSessions>(jsonContent);
            Debug.Log("Data Loaded Successfully! Number of sessions found " + sessions.sessions.Count);

            // Example: Display some loaded data
            if (sessions.sessions.Count > 0)
            {
                for (int i = 0; i < sessions.sessions.Count; i++)
                {
                    miniGameList.Add(new List<MinigameStats>());
                    Debug.Log($"Session ID: {sessions.sessions[i].sessionId}");
                    for (int j = 0; j < sessions.sessions[i].minigames.Count; j++)
                    {

                        if (sessions.sessions[i].minigames.Count > 0)
                        {
                            var minigame = sessions.sessions[i].minigames[j];
                            miniGameList[i].Add(minigame);
                        }
                    }


                }
            }
        }
        else
        {
            Debug.LogError("JSON file not found at: " + filePath);
        }
        float accuracy;
        int num = 0;
        for (int k = 0; k < 4; k++)
        {
            for (int i = 0; i < miniGameList.Count; i++)
            {
                if (miniGameList[i].Count >= k + 1)
                {
                    if (miniGameList[i][k].wrongColorClicked != -1)
                    {
                        accuracy = miniGameList[i][k].CorrectClicks * 100 / (miniGameList[i][k].CorrectClicks + miniGameList[i][k].IncorrectClicks + miniGameList[i][k].wrongColorClicked);
                    }
                    else
                    {
                        accuracy = miniGameList[i][k].CorrectClicks * 100 / (miniGameList[i][k].CorrectClicks + miniGameList[i][k].IncorrectClicks);
                    }
                    overallScore += accuracy;
                    num++;
                }
            }
        }


        print("Calculated scores" + overallScore + " " + num);

        SaveOverallScoreToLeaderboard(overallScore / num);

        return overallScore / num;
    }


    private void SaveOverallScoreToLeaderboard(float overallScore)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(
            new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate> {
                new StatisticUpdate { StatisticName = "overallScore", Value = (int)overallScore }
                }
            },
            result => Debug.Log("Leaderboard score updated successfully!"),
            error => Debug.LogError("Error updating leaderboard score: " + error.GenerateErrorReport())
        );
    }

}
