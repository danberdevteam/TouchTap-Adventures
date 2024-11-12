using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlphabetGameUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TMP_Text HeadingText;
    void Start()
    {
        StartCoroutine(ShowText("Drag the alphabets to correct place!"));
    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator ShowText(string WriteText)
    {
        HeadingText.text = "";
        for (int i = 0; i <= WriteText.Length; i++)
        {
            HeadingText.text = WriteText.Substring(0, i);
            yield return new WaitForSeconds(0.05f);
        }

    }
}
