using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorAnimations : MonoBehaviour
{
    public float moveSpeed = 5.0f;  // Speed at which the GameObject moves
    public GameObject minBounds;       // Minimum bounds of the confined space
    public GameObject maxBounds;       // Maximum bounds of the confined space
    public float changeTargetDelay = 2.0f; // Time in seconds between target changes

    private Vector3 targetPosition;

    void Start()
    {
        StartCoroutine(MoveToRandomPosition());
    }

    IEnumerator MoveToRandomPosition()
    {
        while (true)
        {
            print("Shoulld play");
            // Generate a new random position within the bounds
            targetPosition = new Vector3(
                Random.Range(minBounds.transform.position.x, maxBounds.transform.position.x),
                Random.Range(minBounds.transform.position.y, maxBounds.transform.position.y),
                Random.Range(minBounds.transform.position.z, maxBounds.transform.position.z)
            );

            // Wait until the GameObject reaches the target position
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                // Move towards the target position
                transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // Wait for a delay before choosing the next target position
            yield return new WaitForSeconds(changeTargetDelay);
        }
    }
}
