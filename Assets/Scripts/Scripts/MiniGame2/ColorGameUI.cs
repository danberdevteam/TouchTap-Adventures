using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColorGameUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TMP_Text ScoreText;
    [SerializeField] TMP_Text HeadingText;

    public String TextToWrite;

    public IEnumerator ShowText(string WriteText)
    {
        HeadingText.text = "";
        for (int i = 0; i <= WriteText.Length; i++)
        {
            HeadingText.text = WriteText.Substring(0, i);
            yield return new WaitForSeconds(0.05f);
        }
    }
    void Start()
    {

    }

    void OnEnable()
    {
        StartCoroutine(ShowText(TextToWrite));

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateScore(int Score)
    {
        ScoreText.text = Score.ToString();
    }
}
