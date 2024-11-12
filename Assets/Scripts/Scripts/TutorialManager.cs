
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Image pointer;
    Animator pointerAnimator;
    string keyValue = "isTutorialDone";

    //0- false, 1-true;
    public int isTutorialDone = 0;
    public bool tutorialDoneByPlayer = false;

    int numberOfControls = 3;

    public Action<string> MovementDetected;

    [SerializeField] SwipeDetector swipeDetector;
    [SerializeField] MainGameUI mainGameUI;
    void Start()
    {
        if (PlayerPrefs.HasKey(keyValue))
        {
            isTutorialDone = PlayerPrefs.GetInt(keyValue);
        }
        else
        {
            PlayerPrefs.SetInt(keyValue, 1);
        }

        if (isTutorialDone == 0)
        {
            swipeDetector.OnSwipeRight.AddListener(RightSwiped);
            StartCoroutine(mainGameUI.ShowText("Swipe Right !"));
            pointer.gameObject.SetActive(true);
            pointerAnimator = pointer.GetComponent<Animator>();
            if (!tutorialDoneByPlayer)
            {
                MovementDetected = TouchDetected;
            }
        }
        else
        {
            pointer.gameObject.SetActive(false);
            tutorialDoneByPlayer = true;
            StartCoroutine(mainGameUI.ShowText(""));
        }


    }

    // Update is called once per frame
    void Update()
    {

    }

    void TouchDetected(string triggerName)
    {
        print(numberOfControls + "number of controls");
        if (numberOfControls <= 1)
        {
            tutorialDoneByPlayer = true;
            pointer.gameObject.SetActive(false);
        }
        else
        {

            pointerAnimator.SetTrigger(triggerName);
            numberOfControls--;
        }
    }

    public void RightSwiped()
    {
        MovementDetected.Invoke("SwipeLeft");
        swipeDetector.OnSwipeLeft.AddListener(LeftSwiped);
        StartCoroutine(mainGameUI.ShowText("Swipe Left!"));
        swipeDetector.OnSwipeRight.RemoveListener(RightSwiped);
    }
    public void LeftSwiped()
    {
        MovementDetected.Invoke("Tap");
        swipeDetector.OnTap.AddListener(Tap);
        StartCoroutine(mainGameUI.ShowText("Tap or swipe up to Jump!"));
        swipeDetector.OnSwipeLeft.RemoveListener(LeftSwiped);
    }
    public void Tap()
    {
        MovementDetected.Invoke("Tap");
        swipeDetector.OnSwipeRight.RemoveListener(RightSwiped);
        swipeDetector.OnSwipeLeft.RemoveListener(LeftSwiped);
        swipeDetector.OnTap.RemoveListener(Tap);
        StartCoroutine(mainGameUI.ShowText("Well Done!"));
        StartCoroutine(FinishTutorial());
    }

    IEnumerator FinishTutorial()
    {
        yield return new WaitForSeconds(3);
        StartCoroutine(mainGameUI.ShowText("You are ready to go...."));
        yield return new WaitForSeconds(5);
        StartCoroutine(mainGameUI.ShowText(""));
    }

}
