using System.Collections.Generic;
using System.IO;
using UnityEngine;
// using UnityEngine.UI;

public class JsonDataLoader : MonoBehaviour
{
    private AllSessions allSessions;
    public List<List<MinigameStats>> minigameStats = new List<List<MinigameStats>>();



    void Awake()
    {
        // Construct the file path
        string filePath = Path.Combine(Application.persistentDataPath, "allSessionsData.json");

        // Check if the file exists
        if (File.Exists(filePath))
        {
            // Read the content of the file
            string jsonContent = File.ReadAllText(filePath);

            // Deserialize the JSON content into the AllSessions object
            allSessions = JsonUtility.FromJson<AllSessions>(jsonContent);
            Debug.Log("Data Loaded Successfully! Number of sessions found " + allSessions.sessions.Count);

            // Example: Display some loaded data
            if (allSessions.sessions.Count > 0)
            {
                for (int i = 0; i < allSessions.sessions.Count; i++)
                {
                    minigameStats.Add(new List<MinigameStats>());
                    Debug.Log($"Session ID: {allSessions.sessions[i].sessionId}");
                    for (int j = 0; j < allSessions.sessions[i].minigames.Count; j++)
                    {

                        if (allSessions.sessions[i].minigames.Count > 0)
                        {
                            var minigame = allSessions.sessions[i].minigames[j];
                            minigameStats[i].Add(minigame);
                        }
                    }


                }
            }
        }
        else
        {
            Debug.LogError("JSON file not found at: " + filePath);
        }
    }
}
