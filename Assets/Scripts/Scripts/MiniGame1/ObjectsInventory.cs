using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectsInventory : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] List<CountableObject> Star;
    [SerializeField] int numberOfStars;
    [SerializeField] CountGameUI gameUI;
    private DataCollector dataCollector;

    void Start()
    {
        dataCollector = FindObjectOfType<DataCollector>();

        foreach (CountableObject s in Star)
        {
            s.gameObject.SetActive(false);
        }
        StartCoroutine(EnableStars());
    }

    bool CallOnce = false;
    void Update()
    {
        if (gameUI.initialStarScore == numberOfStars)
        {
            if (!CallOnce)
            {
                StartCoroutine(EndGame());
                CallOnce = true;
                // print("Call end game");
            }
        }

    }

    IEnumerator EndGame()
    {
        gameUI.timeSpent = (float)Math.Round(gameUI.timeSpent, 2);
        MinigameStats minigameStats = new MinigameStats
        {
            gameName = "Count",
            CorrectClicks = gameUI.numberOfCorrectClicks,
            IncorrectClicks = gameUI.numberOfIncorrectClicks,
            timeSpent = gameUI.timeSpent
        };
        // print(gameUI.numberOfCorrectClicks + "   " + gameUI.numberOfIncorrectClicks + "  " + gameUI.timeSpent);
        dataCollector.AddMinigameStats(minigameStats);
        // dataCollector.AddMinigameStats(minigameStats);
        StartCoroutine(gameUI.ShowText("Well Done!"));
        yield return new WaitForSeconds(1f);
        GameManager.Instance.SetPlayerPrefs(1);
        GameManager.Instance.EndGame("MainScene");
    }

    IEnumerator EnableStars()
    {
        foreach (CountableObject s in Star)
        {
            yield return new WaitForSeconds(0.2f);
            s.gameObject.SetActive(true);
        }
    }


}
