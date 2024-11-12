using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShapeGameUI : MonoBehaviour
{
    // Start is called before the first frame update

    public TMP_Text IntroductionText;
    public string TextToWrite;
    public float typingSpeed = 0.05f;
    [SerializeField] MiniGame3Manager gameManager;
    [SerializeField] TMP_Text scoreText;


    void OnEnable()
    {
        StartCoroutine(ShowText(TextToWrite));
    }


    public IEnumerator ShowText(string WriteText)
    {
        IntroductionText.text = "";
        for (int i = 0; i <= WriteText.Length; i++)
        {
            IntroductionText.text = WriteText.Substring(0, i);
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = gameManager.Score.ToString();
    }
}
