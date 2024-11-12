using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Vector3[] lanes = new Vector3[3];
    public float timeInterval = 5f;

    private int laneIndex = 0;
    // private Animator ballAnimator;
    private void Awake()
    {
        // ballAnimator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    // void Start()
    public float rotationSpeed = 100.0f; // Speed of rotation
    public float jumpHeight = 2.0f; // Maximum height of the jump
    public float jumpDuration = 0.5f; // Duration of the jump
    public bool doJump = false; // Trigger for the jump

    private bool isJumping = false;
    private Vector3 originalPosition;

    private float originalY;
    float timeElapsed = 0;
    public Animator ballAnimator;
    public List<ParticleSystem> winEffects;
    bool shouldRotate = true;
    [SerializeField] GameObject originalBall;

    void Start()
    {
        // Store the original y position of the ball
        originalY = transform.position.y;
        originalPosition = transform.position;
        TriggerLaneChange();
    }

    void Update()
    {
        if (shouldRotate)
        {
            // Rotate the ball around its Y-axis
            transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0, Space.World);
        }
        else
        {
            originalBall.transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0, Space.World);

        }

        // Calculate the new Y position using a sine wave

        // transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        // timeElapsed +=  Time.deltaTime;
        // print((int)timeElapsed + "time");
        // if ((int)timeElapsed % 9 == 0)
        // {
        //     print("trigger lanechange");
        //     TriggerLaneChange();
        // }
        if (doJump && !isJumping)
        {
            StartCoroutine(PerformJump());
            doJump = false; // Reset jump trigger
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            print("jump the ball");
            // doJump = true;
            ballAnimator.SetTrigger("jump");

        }
    }

    IEnumerator PerformJump()
    {
        // Mark as jumping
        isJumping = true;
        float elapsedTime = 0;

        while (elapsedTime < jumpDuration)
        {
            // Calculate vertical position using a sine wave for smooth rise and fall
            float height = jumpHeight * Mathf.Sin(Mathf.PI * elapsedTime / jumpDuration);
            transform.position = new Vector3(transform.position.x, originalPosition.y + height, transform.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset position and mark as not jumping
        transform.position = originalPosition;
        isJumping = false;
    }

    [ContextMenu("end game")]
    public void GameFinish()
    {
        shouldRotate = false;
        ballAnimator.SetTrigger("end");
        // this.enabled=false;
        foreach (ParticleSystem ps in winEffects)
        {
            ps.gameObject.SetActive(true);
        }
    }

    public void TriggerLaneChange()
    {
        // ballAnimator.enabled = false;
        laneCoroutine = StartCoroutine(LaneChangeRoutine());
    }

    public void TriggerGameEndAnimation()
    {
        StopCoroutine(laneCoroutine);
        transform.DOKill();

        transform.DOMoveX(0, 0.5f).OnComplete((() =>
        {
            // ballAnimator.enabled = true;
            // ballAnimator.SetTrigger("end");
        }));
    }

    private Coroutine laneCoroutine;
    private IEnumerator LaneChangeRoutine()
    {
        do
        {
            yield return new WaitForSeconds(timeInterval);
            ChangeLane();
        } while (true);
    }

    void ChangeLane()
    {
        print("Change lane");
        int randomIdx = UnityEngine.Random.Range(0, lanes.Length);
        if (laneIndex == randomIdx)
        {
            ChangeLane();
            return;
        }

        Debug.LogWarning($"old index= {laneIndex}\tnew index= {randomIdx}\t{lanes[randomIdx]}");
        // this.GetComponent<Animator>().enabled=false;
        transform.DOMoveX(lanes[randomIdx].x, 0.5f).SetEase(Ease.InOutSine);
           
        laneIndex = randomIdx;

    }
}
