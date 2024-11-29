using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlphabetGameManager : MonoBehaviour
{

    int missedLetter = 0;
    int completedLetter = 0;
    float TimeSpent=0;
    [SerializeField] DataCollector dataCollector;
    // Start is called before the first frame update

    public static AlphabetGameManager Instance { get; private set; }
    void Start()
    {
        Instance = this;
        dataCollector = FindObjectOfType<DataCollector>();
    }

    // Update is called once per frame
    void Update()
    {
TimeSpent=+Time.deltaTime;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void IncreaseMissedLetter()
    {
        missedLetter++;
        print("Letter missed");
    }
    public void IncreseCorrectLetter()
    {
        completedLetter++;
        // print(completedLetter + "Completed letter" + missedLetter + "Missed letter");
    }

    public void EndGame()
    {
        TimeSpent=(float)Math.Round(TimeSpent, 2);
        MinigameStats minigameStats = new MinigameStats
        {
            gameName = "Alphabet",
            CorrectClicks = completedLetter,
            IncorrectClicks = missedLetter,
            timeSpent=TimeSpent,
            alphabetTraceComplete=completedLetter,
            alphabetTraceMissed=missedLetter
        };
        dataCollector.AddMinigameStats(minigameStats);
        print(completedLetter + "Completed letter" + missedLetter + "Missed letter");
        GameManager.Instance.IncreasePickupLimit();
        // GameManager.Instance.EndGame("MainScene");
    }



}
