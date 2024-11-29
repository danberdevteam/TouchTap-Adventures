using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MaskTransitions;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager Instance;
    public bool isMiniGameStarted = false;

    // List<int> pickups;
    public List<Obstacles> obstaclePrefab;
    public float TimeElapsed = 0;
    [Range(1, 5)]
    public int howOftenShouldObstacleSpawn = 5;
    public int MiniGameNumber = 0;
    string keyValue = "gameNumber";
    public int pickupLimit=10;




    [SerializeField] public TutorialManager tutorialManager;

    void Start()
    {

    }
    void Update()
    {
        if (PlayerPrefs.HasKey(keyValue))
        {
            // print("got the player pref");
            MiniGameNumber = PlayerPrefs.GetInt(keyValue);
        }
        if (tutorialManager)
        {

            if (tutorialManager.tutorialDoneByPlayer)
            {
                TimeElapsed += Time.deltaTime;
            }
        }
        else
        {

            TimeElapsed += Time.deltaTime;

        }
        // print(TimeElapsed);
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }



    public void EndGame(String sceneName)
    {
        StartCoroutine(DelayStart(sceneName));
    }

    public void InstantLoadLevel(String sceneName)
    {
        TransitionManager.Instance.LoadLevel(sceneName);
        DOTween.Clear(true);
        TimeElapsed = 0;
    }

    IEnumerator DelayStart(String sceneName)
    {

        yield return new WaitForSeconds(0.3f);
        DOTween.Clear(true);
        TimeElapsed = 0;
        TransitionManager.Instance.LoadLevel(sceneName);
        // SceneManager.LoadScene(sceneName);
        print("level loaded");
    }

    void OnApplicationQuit()
    {
        // PlayerPrefs.DeleteAll();
        PlayerPrefs.DeleteKey(keyValue);
        PlayerPrefs.Save();
    }

    public void SetPlayerPrefs(int gamenumber)
    {
        PlayerPrefs.SetInt(keyValue, gamenumber);
    }

    public void IncreasePickupLimit()
    {
        pickupLimit=1000000;
    }




}
