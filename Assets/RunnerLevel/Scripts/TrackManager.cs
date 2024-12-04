using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TrackManager : MonoBehaviour
{
    public TrackSegment trackPrefab;
    public Transform trackRoot;
    public Transform segmentSpawnPoint;
    public Transform pickupPointsParent;
    [SerializeField] private Light directionalLight;
    [SerializeField] private float nightLight;
    [Range(0, 1)]
    public float pickupDensity;
    public List<TrackSegment> allTrackSegments;
    public List<TrackSegment> activeSegments;

    public List<Pickups> PickupsList;

    public float trackSpeed = 0.5f;
    public Vector3 segmentDistanceOffset = Vector3.forward;
    public bool isMove = true;
    public bool canInteract = true;
    public ObstacleSpawnParent obstacleSpawnParent;
    public AudioClip trackEndMusic;

    private void Awake()
    {
        foreach (var particleSystem in effects)
        {

        }
        SetTrackTheme(0);

    }

    public void Start()
    {
        foreach (Transform item in trackRoot)
        {
            int i = 0;
            TrackSegment segment = item.GetComponent<TrackSegment>();
            if (segment != null)
            {
                if (i == 0) segment.disablePickups = true;

                segment.gameObject.SetActive(true);
                activeSegments.Add(segment);
                segment.SpawnPickups();
                i++;
            }
        }

        OnGameEnd_TrackManager += OnGameEnd;
    }

    private void OnDisable()
    {
        OnGameEnd_TrackManager -= OnGameEnd;
    }

    public int theme = 0;
    public List<List<TrackSegment>> trackThemes;
    public List<MyClass> tracks;
    [System.Serializable]
    public class MyClass
    {
        public string name;
        public List<TrackSegment> SegmentsGroup;
    }

    public bool isDay = true;
    private bool isFirstTime = true;
    public void SetTrackTheme(int _theme)
    {
        if (tracks.Count == 0) return;
        // theme = _theme;
        allTrackSegments = new List<TrackSegment>();

        allTrackSegments = tracks[_theme].SegmentsGroup;

        //SkyboxNight();
        if (isFirstTime == false)
        {
            if (isDay)
            {
                SkyboxNight();
                isDay = false;
            }
            else
            {
                SkyboxDay();
                isDay = true;
            }
        }
        isFirstTime = false;

    }

    public void SpawnNewSegment()
    {
        int index = UnityEngine.Random.Range(1, 100);
        string msg = index.ToString();
        index = index % allTrackSegments.Count;
        //msg += $"\t count= {index}"; 
        // print(msg);
        Vector3 spawnPos = activeSegments[activeSegments.Count - 1].transform.position + segmentDistanceOffset;

        var trackSegment = Instantiate(allTrackSegments[index], trackRoot);
        trackSegment.transform.position = spawnPos;

        trackSegment._TrackManager = this;
        activeSegments.Add(trackSegment);
        // themeCounter++;
        //trackSegment.disablePickups = true;
        trackSegment.gameObject.SetActive(true);
        //trackSegment.SpawnPickups();
    }

    // private int counter = 0;
    // public int themeCounter
    // {
    //     get { return counter;}
    //     set
    //     {
    //         counter++;
    //         if (counter % 15 == 0)
    //         {
    //             theme++;
    //             theme = theme % tracks.Count; 
    //             SetTrackTheme(theme);
    //         }
    //     }
    // }

  public void SpawnConsumables(Transform pickUpTarget = null)
{
    if (pickUpTarget == null) 
        pickUpTarget = pickupPointsParent;

    int pointCount = pickUpTarget.childCount;
    if (pointCount < 1)
        return;

    // Calculate the number of pickups to spawn (clamped between 1 and 20)
    int size = Mathf.Clamp(Mathf.RoundToInt(pointCount * pickupDensity), 1, 20);

    // Generate a list of indices and shuffle it
    List<int> randomIndices = Enumerable.Range(0, pointCount).OrderBy(x => Random.value).ToList();

    // Spawn pickups at the shuffled indices
    for (int i = 0; i < size; i++)
    {
        int index = randomIndices[i];
        Transform spawnPoint = pickUpTarget.GetChild(index);

        // Ensure the item and game manager are valid
        if (GameManager.Instance != null && GameManager.Instance.MiniGameNumber < PickupsList.Count)
        {
            Pickups item = Instantiate(PickupsList[GameManager.Instance.MiniGameNumber], spawnPoint);
            SetupPickup(item);
        }
        else
        {
            Debug.LogWarning("Invalid game manager or pickup list configuration.");
        }
    }
}


    [SerializeField] private int alphabetCount = 0;
    [SerializeField] private int shapeCount = 0;
    void SetupPickup(Pickups item)
    {
        var pickupItem = item.GetComponentInChildren<PickupItem>(true);
        switch (pickupItem.specialType)
        {
            case PickupType.simple:
                pickupItem.gameObject.SetActive(true);
                break;
            case PickupType.shape:
                pickupItem.SetSpecialPickup(shapeCount % pickupItem.specialPickupObjects.Length);
                pickupItem.gameObject.SetActive(true);
                shapeCount++;
                break;
            case PickupType.alphabet:
                pickupItem.SetSpecialPickup(alphabetCount % pickupItem.specialPickupObjects.Length);
                pickupItem.gameObject.SetActive(true);
                alphabetCount++;
                break;
        }
    }

    public void ResetSegment(TrackSegment trackSegment)
    {
        activeSegments.Remove(trackSegment);

    }

    // Start is called before the first frame update
    void Update()
    {
        if (isMove)
        {
            MoveTrack();
        }

        if (canInteract)
        {
            if (Input.GetMouseButtonDown(0))
            {
                FireRayOnMap();
            }
        }
    }

    public void MoveTrack()
    {
        trackRoot.position += Vector3.forward * -(Time.deltaTime * trackSpeed);
    }

    public void FireRayOnMap()
    {

        // Get the mouse position on the screen
        Vector3 mousePosition = Input.mousePosition;

        // Create a ray from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Raycast_Scan")))
        {
            // If the ray hits an object, log the object's name
            Debug.Log("Hit: " + hit.transform.name);

            // Optionally, you can do something with the hit object
            // For example, you could change its color:
            hit.transform.GetComponent<InteractableAnim>().PlayEffect();
            foreach (var particleSystem in effects)
            {
                particleSystem.startColor = Color.green;
                particleSystem.gameObject.SetActive(true);
                particleSystem.Stop();

            }

            int r = UnityEngine.Random.Range(0, effects.Length);
            //effects[r].transform.localPosition = hit.transform.position;
            //effects[r].transform.localScale = hit.transform.lossyScale;
            //effects[r].Play();

            ParticleSystem particle = Instantiate(effects[r], hit.transform);
            particle.gameObject.SetActive(true);
        }
        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("LevelViewChange_Layer")))
        {
            // If the ray hits an object, log the object's name
            Debug.Log("Hit: " + hit.transform.name);

            // Optionally, you can do something with the hit object
            // For example, you could change its color:
            hit.transform.GetComponent<InteractableAnim>().PlayEffect();
            hit.transform.GetComponent<InteractableAnim>().ChangeLevel();

            foreach (var particleSystem in effects)
            {
                particleSystem.startColor = Color.blue;
                particleSystem.gameObject.SetActive(true);
                particleSystem.Stop();

            }

            int r = UnityEngine.Random.Range(0, effects.Length);
            //effects[r].transform.localPosition = hit.transform.position;
            //effects[r].transform.localScale = hit.transform.lossyScale;
            //effects[r].Play();

            ParticleSystem particle = Instantiate(effects[r], hit.transform);
            particle.startColor = Color.blue;
            particle.gameObject.SetActive(true);
        }
    }

    public static Action OnGameEnd_TrackManager;

    public void OnGameEnd()
    {
        StopTheGame();
    }

    [ContextMenu("Finish")]
    public void StopTheGame()
    {
        isMove = false;
        var catController = FindObjectOfType<MoveCat>();
        catController.StopTheGame();
        catController.animator.SetTrigger("Idle");
        // var ballAnim = FindObjectOfType<BallController>();
        // ballAnim.GameFinish();
        AudioManager.Instance?.SetMusicPlayback(trackEndMusic);
    }

    #region Skybox transitions

    //public Animator SkyboxAnimator;

    public Animator SkyboxAnimator;
    [ContextMenu("day sky")]
    public void SkyboxDay()
    {
        directionalLight.DOIntensity(.75f, 3f);
        SkyboxAnimator.SetTrigger("day");
    }

    [ContextMenu("night sky")]
    public void SkyboxNight()
    {
        directionalLight.DOIntensity(nightLight, 5f);
        SkyboxAnimator.SetTrigger("night");
    }


    #endregion

    public ParticleSystem[] effects;
}