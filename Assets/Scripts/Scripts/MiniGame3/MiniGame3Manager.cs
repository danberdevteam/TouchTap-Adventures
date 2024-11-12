using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using UnityEngine;
using Lean.Touch;

public class MiniGame3Manager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Material WinterSkybox;
    [SerializeField] GameObject ButtonCanvas;
    [SerializeField] GameObject ShapePlaceholder;
    [SerializeField] GameObject CubePlaceholderObject;
    [SerializeField] GameObject SpherePlaceholderObject;
    [SerializeField] GameObject ConePlaceholderObject;
    [SerializeField] GameObject PyramidPlaceholderObject;
    [SerializeField] ShapeGameUI shapeGameUI;
    [SerializeField] GameObject gameBase;

    List<ButtonShape> buttonsSpawned = new List<ButtonShape>();

    [SerializeField] List<ShapeButton> ShapeButtons;
    [SerializeField] LeanSelectableByFinger MisClickPlane;


    bool isGameRunning = true;
    Vector3 CameraInitialPosition = new Vector3();
    public ButtonShape TargetShape;
    public TMP_Text TargetText;
    public int Score;

    private float[] xPositions = { -30, -10, 10, 30 };
    private float[] zPositions = { -25, 0, 25 };

    private static System.Random random = new System.Random();



    private DataCollector dataCollector;
    int CorrectClicked = 0;
    int wrongClicked = 0;
    int ClickedSomeWhereElse = 0;
    float TimeSpent = 0;


    void Start()
    {
        InstantiateButton();
        TargetShape = GetRandomButtonShape();
        dataCollector = FindObjectOfType<DataCollector>();
        // RenderSettings.skybox = WinterSkybox;

        // player.hoppingAgent.transform.DOScale(5, 2);
        // StartCoroutine(player.hoppingAgent.ShowText("Choose the correct Shape!!",0.5f));


    }

    bool wrongClickCallOnce = true;

    // Update is called once per frame
    void Update()
    {
        TimeSpent += Time.deltaTime;
        // ChangeCameraPosition();
        if (MisClickPlane.IsSelected && wrongClickCallOnce)
        {
            // print("Misclicked");
            ClickedSomeWhereElse++;
            wrongClickCallOnce = false;
        }

        if (!MisClickPlane.IsSelected)
        {
            wrongClickCallOnce = true;
            // numberOfIncorrectClicks++;
        }

    }




    void InstantiateButton()
    {
        foreach (float x in xPositions)
        {
            foreach (float z in zPositions)
            {
                Vector3 position = new Vector3(x, -27, z);
                int randomNumber = UnityEngine.Random.Range(0, ShapeButtons.Count);
                ShapeButton button = Instantiate(ShapeButtons[randomNumber], ButtonCanvas.transform);
                buttonsSpawned.Add(button.buttonShape);
                button.transform.SetLocalPositionAndRotation(position, Quaternion.Euler(button.transform.rotation.x - 90, 0, 0));
            }
        }
    }



    public ButtonShape GetRandomButtonShape()
    {
        ButtonShape randomShapeChoosen = buttonsSpawned[UnityEngine.Random.Range(0, buttonsSpawned.Count)];
        SpawnPlaceHolderShape(randomShapeChoosen);
        return randomShapeChoosen;
    }


    // On correct shape 
    public void OnCorrectShapePicked()
    {
        if (Score > 10)
        {
            EndGame();
        }
        else
        {
            CorrectClicked++;
            TargetShape = GetRandomButtonShape();
            Score++;
        }
    }
    public void WrongShapeChosen()
    {
        wrongClicked++;
    }
    // Spawn hint shape
    public void SpawnPlaceHolderShape(ButtonShape buttonShape)
    {
        Destroy(ShapePlaceholder.GetComponentInChildren<MeshRenderer>());
        switch (buttonShape)
        {
            case ButtonShape.Cube:
                Instantiate(CubePlaceholderObject, ShapePlaceholder.transform);
                TargetText.text = "Cube";
                break;
            case ButtonShape.Cone:
                Instantiate(ConePlaceholderObject, ShapePlaceholder.transform);
                TargetText.text = "Cone";
                break;
            case ButtonShape.Sphere:
                Instantiate(SpherePlaceholderObject, ShapePlaceholder.transform);
                TargetText.text = "Sphere";
                break;
            case ButtonShape.Pyramid:
                Instantiate(PyramidPlaceholderObject, ShapePlaceholder.transform);
                TargetText.text = "Pyramid";
                break;
        }
    }


    // Pop the choosen color from stack
    public void ShapeChoosen(ButtonShape buttonShape)
    {
        buttonsSpawned.Remove(buttonShape);
    }


    void EndGame()
    {

        MinigameStats minigameStats = new MinigameStats
        {
            gameName = "Shape",
            CorrectClicks = CorrectClicked,
            IncorrectClicks = ClickedSomeWhereElse,
            timeSpent = TimeSpent,
            wrongColorClicked = wrongClicked

        };
        dataCollector.AddMinigameStats(minigameStats);

        gameBase.transform.DOScale(0, 2);
        isGameRunning = false;
        StartCoroutine(shapeGameUI.ShowText("Well Done!"));
        // print("End game called");
        // StartCoroutine(player.hoppingAgent.ShowText("Well done, Onto the next one!!",0.5f));

        StartCoroutine(DelayEnd());
    }
    IEnumerator DelayEnd()
    {
        yield return new WaitForSeconds(4);
        // player.EndMiniGame(2);
        GameManager.Instance.SetPlayerPrefs(3);
        GameManager.Instance.EndGame("MainScene");
    }

}
