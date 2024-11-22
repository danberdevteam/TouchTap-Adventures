
using System.Collections.Generic;
using MagneticScrollView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ChangeLetters : MonoBehaviour
{
    public List<OrderController> letters;
    public int m_index;
    public GameObject animObj;

    public ParticleSystem finishParticleEffect;

    // public GameObject Pointer;
    public float TimeScale = 0;
    public MagneticScrollRect magneticScrollRect;
    public int NumberOfLettersDone = 0;
    [SerializeField] Text titleText;

    private enum E_Direction
    {
        Left, Right
    }
    public static ChangeLetters m_ChangeLetters;
    void Awake()
    {
        m_ChangeLetters = this;
        animObj.SetActive(false);
        onClickChangeSelectedIndex(E_Direction.Right);

        // StartCoroutine(On_OffStarPanel());
    }
    void Update()
    {
        // ChangeUIValues(m_index);

        // if (TimeScale > 400)
        // {
        //     Pointer.SetActive(true);
        // }
        // else
        // {
        //     Pointer.SetActive(false);
        // }
    }
    private void onClickChangeSelectedIndex(E_Direction direction)
    {
        switch (direction)
        {
            case E_Direction.Left:
                m_index--;

                break;
            case E_Direction.Right:
                m_index++;

                break;
            default:
                m_index--;

                break;
        }
        if (m_index >= 26)
        {
            m_index = 26;
        }
        else if (m_index <= 0)
        {
            m_index = 1;
        }
        // ChangeUIValues(m_index);
    }


    // public void ChangeUIValues(int index)
    // {
    //     txtCurrentLevel.text = "Level " + index.ToString();
    //     txtCharCount.text = index.ToString() + "/26";
    //     string str = "\"" + chars[index - 1] + "\"";
    //     txtCurrentLetter.text = "Select Letter " + str;
    // }


    public void IncreaeIndex()
    {
        onClickChangeSelectedIndex(E_Direction.Right);

    }
    public void DecreaseIndex()
    {
        onClickChangeSelectedIndex(E_Direction.Left);

    }

    public void ScrollToNextWord()
    {
        magneticScrollRect.ScrollForward();
        if (NumberOfLettersDone == 3)
        {
            titleText.text = "Well Done";
            // StartCoroutine(DelayEnd());
            GameManager.Instance.EndGame("MainScene");

        }
    }



    // IEnumerator DelayEnd()
    // {
    //     yield return new WaitForSeconds(4);
    //     // player.EndMiniGame(2);
    //     GameManager.Instance.SetPlayerPrefs(1);
    //     GameManager.Instance.EndGame("MainScene");
    // }

    // public void ShowStars()
    // {
    //     // Show stars for finish letters 

    //     if (m_index <= letters.Count && letters[m_index - 1].success)
    //     {
    //         animObj.SetActive(true);
    //         StartCoroutine(On_OffStarPanel());
    //     }
    //     else
    //     {
    //         animObj.SetActive(false);
    //     }
    // }

    // public IEnumerator On_OffStarPanel()
    // {
    //     animObj.SetActive(false);
    //     yield return new WaitForSeconds(0.1f);
    //     animObj.SetActive(true);
    // }
}
