using System;
using System.Collections.Generic;
[Serializable]
public class MinigameStats
{
    public string gameName = "NA";
    public int CorrectClicks = -1;
    public int IncorrectClicks = -1;
    public float timeSpent = -1;
    public int wrongColorClicked = -1;

    public int alphabetTraceMissed=-1;
    public int alphabetTraceComplete=-1;
}

[Serializable]
public class SessionData
{
    public string sessionId;
    public List<MinigameStats> minigames = new List<MinigameStats>();
}

[Serializable]
public class AllSessions
{
    public List<SessionData> sessions = new List<SessionData>();
}

// [Serializable]
// public class GameData
// {
//     public Dictionary<string, SessionData> sessions = new Dictionary<string, SessionData>();
// }
