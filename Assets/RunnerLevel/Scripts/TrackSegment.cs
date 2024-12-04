using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSegment : MonoBehaviour
{
    public TrackManager _TrackManager;
    public Transform pickupPointRoot;

    public bool isMoving = true;
    public float moveSpeed = 5f;
    public bool disablePickups = true;

    public ObstacleSpawnParent[] obstacleSpawnParent;
    public List<List<ObstacleSpawnPoint>> spawnPointsSubParent = new List<List<ObstacleSpawnPoint>>();
    public List<ObstacleSpawnPoint> spawnPoints;
    bool ShouldSpawnObstacles;

    // int NumberOfObstaclesInEachSegment=0;

    void Start()
    {
        disablePickups=true;
        int shouldGenerateObstacle = UnityEngine.Random.Range(0, 100);
        obstacleSpawnParent = GetComponentsInChildren<ObstacleSpawnParent>();
        int i = 0;
        foreach (var parent in obstacleSpawnParent)
        {

            foreach (var point in parent.GetComponentsInChildren<ObstacleSpawnPoint>())
            {
                spawnPoints.Add(point);
            }
            spawnPointsSubParent.Add(spawnPoints);
            // spawnPoints.Clear();
            i++;
        }

        if (shouldGenerateObstacle % GameManager.Instance.howOftenShouldObstacleSpawn == 0)
        {
            // SpawnObstacles();
            disablePickups=false;
            SpawnPickups();
        }
        if (GameManager.Instance.TimeElapsed > 10 && shouldGenerateObstacle % GameManager.Instance.howOftenShouldObstacleSpawn == 0)
        {
            SpawnObstacles();
            disablePickups=false;
            // SpawnPickups();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _TrackManager?.SpawnNewSegment();
        _TrackManager?.SpawnNewSegment();


    }

    public void SpawnObstacles()
    {

        int randomNumber = UnityEngine.Random.Range(0, 3);
        int randForObstacle = UnityEngine.Random.Range(0, GameManager.Instance.obstaclePrefab.Count);
        if (randForObstacle == 1)
        {
            Instantiate(GameManager.Instance.obstaclePrefab[randForObstacle], spawnPoints[1].transform);
        }

        else
        {
            Instantiate(GameManager.Instance.obstaclePrefab[randForObstacle], spawnPoints[randomNumber].transform);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        _TrackManager.ResetSegment(this);
        Destroy(gameObject);
    }

    [ContextMenu("SPawn pickup in this segment")]
    public void SpawnPickups()
    {
        if (disablePickups) return;

        _TrackManager?.SpawnConsumables(pickupPointRoot);
    }

    public void Update()
    {
        Move();
    }

    public void Move()
    {
        if (isMoving == false) return;

        transform.position += Vector3.forward * -(Time.deltaTime * moveSpeed);
    }
}
