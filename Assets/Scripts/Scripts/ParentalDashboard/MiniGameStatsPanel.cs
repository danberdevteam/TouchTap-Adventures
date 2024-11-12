
using TMPro;
using UnityEngine;

public class MiniGameStatsPanel : MonoBehaviour
{
    public MinigameStats minigameStats;
    [SerializeField] TMP_Text TextPrefab;
    [SerializeField] GameObject TextHolder;
    // Start is called before the first frame update
    void Start()
    {
        if (minigameStats.CorrectClicks != -1)
        {
            TMP_Text Clicktext = Instantiate(TextPrefab, TextHolder.transform);
            Clicktext.text = "Correct Clicks: " + minigameStats.CorrectClicks.ToString();
        }
        if (minigameStats.IncorrectClicks != -1)
        {
            TMP_Text IncorrClicktext = Instantiate(TextPrefab, TextHolder.transform);
            IncorrClicktext.text = "Incorrect Clicks: " + minigameStats.IncorrectClicks.ToString();
        }

        if (minigameStats.timeSpent != -1)
        {
            TMP_Text TimeSpentText = Instantiate(TextPrefab, TextHolder.transform);
            TimeSpentText.text = "Time Spent: " + minigameStats.timeSpent.ToString() + " sec";
        }

        if (minigameStats.wrongColorClicked != -1)
        {
            TMP_Text WrongClickText = Instantiate(TextPrefab, TextHolder.transform);
            WrongClickText.text = "Wrong Color Clicks: " + minigameStats.wrongColorClicked.ToString();
        }

        if (minigameStats.alphabetTraceComplete != -1)
        {
            TMP_Text alphabetComplete = Instantiate(TextPrefab, TextHolder.transform);
            alphabetComplete.text = "Alphabet completed count: " + minigameStats.alphabetTraceComplete.ToString();
        }
        if (minigameStats.alphabetTraceMissed != -1)
        {
            TMP_Text alphabetMissed = Instantiate(TextPrefab, TextHolder.transform);
            alphabetMissed.text = "Alphabet missed count: " + minigameStats.alphabetTraceMissed.ToString();
        }
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
