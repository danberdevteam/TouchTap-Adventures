using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using TMPro;
using UnityEngine;

public class CountGameUI : MonoBehaviour
{

    // Start is called before the first frame update
    [SerializeField] TMP_Text tapStarScoreText;
    public int initialStarScore = 0;
    [SerializeField] LeanSelectableByFinger misClickBoard;



    public TMP_Text IntroductionText;
    public string TextToWrite;
    public float typingSpeed = 0.05f;
    public float timeSpent = 0;
    public int numberOfCorrectClicks = 0;
    public int numberOfIncorrectClicks = 0;



    //Show Typewriter Text
    public IEnumerator ShowText(string WriteText)
    {
        IntroductionText.text = "";
        for (int i = 0; i <= WriteText.Length; i++)
        {
            IntroductionText.text = WriteText.Substring(0, i);
            yield return new WaitForSeconds(typingSpeed);
        }
    }




    private void OnEnable()
    {
        // Subscribe to the event
        CountableObject.OnCountableObjectClickedStar += UpdateStarScore;
        StartCoroutine(ShowText(TextToWrite));

    }

    private void OnDisable()
    {
        // Unsubscribe from the event when the script is disabled to avoid memory leaks
        CountableObject.OnCountableObjectClickedStar -= UpdateStarScore;
    }


    void UpdateStarScore()
    {
        initialStarScore++;
        tapStarScoreText.text = initialStarScore.ToString();
        numberOfCorrectClicks++;
    }


    // Update is called once per frame
    bool callOnce = true;
    void Update()
    {
        timeSpent += Time.deltaTime;
        if (misClickBoard.IsSelected && callOnce)
        {
            // print("Misclicked");
            numberOfIncorrectClicks++;
            callOnce = false;
        }

        if (!misClickBoard.IsSelected)
        {
            callOnce = true;
            // numberOfIncorrectClicks++;
        }


    }
}
