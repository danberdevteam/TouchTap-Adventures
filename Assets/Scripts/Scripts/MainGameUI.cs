using System.Collections;
using TMPro;
using UnityEngine;

public class MainGameUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text timeElapsed;
    [SerializeField] TMP_Text tutorialText;

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    void Update()
    {
        timeElapsed.text = ((int)GameManager.Instance.TimeElapsed).ToString();
    }

     public IEnumerator ShowText(string WriteText)
    {
        tutorialText.text="";
        for (int i = 0; i <= WriteText.Length; i++)
        {
            tutorialText.text = WriteText.Substring(0, i);
            yield return new WaitForSeconds(0.1f);
        }
    }


}
