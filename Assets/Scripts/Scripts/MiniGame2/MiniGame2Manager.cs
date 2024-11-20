
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Touch;
using TMPro;
using UnityEngine;

public class MiniGame2Manager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] List<ColorButton> buttons;
    [SerializeField] TMP_Text targetColorText;
    [SerializeField] GameObject ButtonCanvas;
    [SerializeField] List<ColorButton> colorButtons;
    [SerializeField] ColorGameUI colorGameUI;
    [SerializeField] ParticleSystem indicatorParticles;

    [SerializeField] GameObject GameBase;
    [SerializeField] Material RedMaterial;
    [SerializeField] Material BlueMaterial;
    [SerializeField] Material GreenMaterial;

    [SerializeField] LeanSelectableByFinger Base;

    private Material BaseDefaultMaterial;

    public int ColorGameScore = 0;


    private float[] xPositions = { -4, -2, 0, 2, 4 };
    private float[] zPositions = { 3, 5, 7, 9 };




    Stack<ColorButton> InstantiatedButtonRed = new Stack<ColorButton>();
    Stack<ColorButton> InstantiatedButtonBlue = new Stack<ColorButton>();
    Stack<ColorButton> InstantiatedButtonGreen = new Stack<ColorButton>();

    // Vector3 CameraInitialPosition = new Vector3();




    ///////////////////////Parental Data///////////////////
    private DataCollector dataCollector;
    int CorrectColorSelected = 0;
    int wrongColorClicked = 0;
    int ClickedSomeWhereElse = 0;
    float TimeSpent = 0;




    public ButtonColor TargetColor;
    public int NumberOfButtons = 25;

    public bool isGameRunning = true;
    void Start()
    {
        dataCollector = FindObjectOfType<DataCollector>();
        // CameraInitialPosition = mainCamera.transform.position;
        BaseDefaultMaterial = GameBase.GetComponent<Renderer>().material;
        InitiateButtons();
        // player.hoppingAgent.DialogToSay = "Match the correct colors!!";
        // StartCoroutine(player.hoppingAgent.ShowText("Match the correct colors!!",0.5f));
        // player.hoppingAgent.transform.DOScale(5, 2);
        ChooseTargetColor();
    }
    bool callOnce = true;
    bool wrongClickCallOnce = true;

    // Update is called once per frame
    void Update()
    {
        TimeSpent += Time.deltaTime;
        if (!isGameRunning && callOnce)
        {
            EndGame();
            // isGameRunning = true;
            callOnce = false;
        }
        if (Base.IsSelected && wrongClickCallOnce)
        {
            // print("Misclicked");
            ClickedSomeWhereElse++;
            wrongClickCallOnce = false;
        }

        if (!Base.IsSelected)
        {
            wrongClickCallOnce = true;
            // numberOfIncorrectClicks++;
        }

    }


    void ChooseTargetColor()
    {
        int num = Random.Range(0, 3);
        // int num = 1;
        // print(num);
        if (InstantiatedButtonRed.Count == 0 && InstantiatedButtonGreen.Count == 0 && InstantiatedButtonBlue.Count == 0)
        {
            TargetColor = ButtonColor.None;
            isGameRunning = false;
        }
        if (isGameRunning)
        {
            switch (num)
            {
                case 0:
                    TargetColor = ButtonColor.Red;
                    targetColorText.text = "Red";
                    indicatorParticles.startColor = Color.red;
                    targetColorText.color = Color.red;
                    GameBase.GetComponent<Renderer>().material = RedMaterial;
                    break;
                case 1:
                    TargetColor = ButtonColor.Blue;
                    targetColorText.text = "Blue";
                    indicatorParticles.startColor = Color.blue;
                    targetColorText.color = Color.blue;
                    GameBase.GetComponent<Renderer>().material = BlueMaterial;
                    break;
                case 2:
                    TargetColor = ButtonColor.Green;
                    targetColorText.text = "Green";
                    targetColorText.color = Color.green;
                    indicatorParticles.startColor = Color.green;
                    GameBase.GetComponent<Renderer>().material = GreenMaterial;
                    break;
            }

            if (TargetColor == ButtonColor.Red && InstantiatedButtonRed.Count == 0)
            {
                ChooseTargetColor();
            }
            if (TargetColor == ButtonColor.Blue && InstantiatedButtonBlue.Count == 0)
            {
                ChooseTargetColor();
            }
            if (TargetColor == ButtonColor.Green && InstantiatedButtonGreen.Count == 0)
            {
                ChooseTargetColor();
            }
        }



        print("Called");

    }

    public void OnCorrectButtonPressed()
    {
        if (ColorGameScore == NumberOfButtons - 1)
        {
            GameBase.GetComponent<Renderer>().material = BaseDefaultMaterial;
            EndGame();
        }
        else
        {

            // WinParticleEffect.Play();
            PopColorsFromStack();
            ChooseTargetColor();
            ColorGameScore++;
            CorrectColorSelected++;
            colorGameUI.UpdateScore(ColorGameScore);
        }
        // print("Right button pressed");
    }
    public void OnWrongButtonPressed()
    {
        wrongColorClicked++;
        // LoseParticleEffect.Play();
        print("Wrong Button pressed");

    }


    void EndGame()

    {
        TimeSpent = (float)System.Math.Round(TimeSpent, 2);
        MinigameStats minigameStats = new MinigameStats
        {
            gameName = "Color",
            CorrectClicks = CorrectColorSelected,
            IncorrectClicks = wrongColorClicked,
            timeSpent = TimeSpent,
            wrongColorClicked = ClickedSomeWhereElse

        };
        dataCollector.AddMinigameStats(minigameStats);
        // gameBase.transform.DOScale(0, 2);

        StartCoroutine(colorGameUI.ShowText("Well Done!"));
        // StartCoroutine(player.hoppingAgent.ShowText("Well done, Onto the next one!!",0.5f));
        StartCoroutine(DelayInEnd());
    }

    IEnumerator DelayInEnd()
    {
        StartCoroutine(colorGameUI.ShowText("Well Done!"));
        yield return new WaitForSeconds(4);
        // player.EndMiniGame(1);
        GameManager.Instance.SetPlayerPrefs(2);
        GameManager.Instance.EndGame("MainScene");

    }

    void PopColorsFromStack()
    {
        switch (TargetColor)
        {
            case ButtonColor.Red:
                InstantiatedButtonRed.Pop();
                break;
            case ButtonColor.Blue:
                InstantiatedButtonBlue.Pop();
                break;
            case ButtonColor.Green:
                InstantiatedButtonGreen.Pop();
                break;

        }

        print("Popped");
    }

    void InitiateButtons()
    {

        foreach (float x in xPositions)
        {
            foreach (float z in zPositions)
            {
                Vector3 position = new Vector3(x, 0.5f, z);
                int randomNumber = Random.Range(0, 3);
                ColorButton button = Instantiate(colorButtons[randomNumber], ButtonCanvas.transform);
                button.transform.SetLocalPositionAndRotation(position, Quaternion.identity);
                switch (randomNumber)
                {
                    case 0:
                        InstantiatedButtonRed.Push(button);
                        break;
                    case 1:
                        InstantiatedButtonBlue.Push(button);
                        break;
                    case 2:
                        InstantiatedButtonGreen.Push(button);
                        break;
                }

            }
        }


    }


}
