using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveCat : MonoBehaviour
{
    //Movement Variables
    public float MovementSpeed = 8;
    public Animator animator;
    [SerializeField] GameObject Cat;
    [SerializeField] ParticleSystem MoveParticleEffect;
    [SerializeField] GameObject platform;
    [SerializeField] BallController ballController;

    public float movementIncrease = 2;
    public float maxSpeed = 10;
    public float minSpeed = 6;
    CharacterController cc;
    Vector3 movec = Vector3.zero;
    bool canmove = true;
    int line = 1;
    int targetLine = 1;
    /////////////////////////////////////////
    int PickupCount = 0;
    bool isSceneOn = false;

    public ParticleSystem DeathParticleEffect;

    public float InitialYposition;

    bool isSwipedCorrect = false;


    [SerializeField] MainGameUI mainGameUI;

    void Start()
    {
        pickupLimit=GameManager.Instance.pickupLimit;
        animator = Cat.GetComponent<Animator>();

        cc = gameObject.GetComponent<CharacterController>();
        InitialYposition = transform.position.y;

    }
    public float jumpHeight = 2.0f;
    public float jumpDuration = 0.5f;
    private float gravity = -9.81f;  // Adjust gravity as needed for your game
    private bool isJumping = false;
    private float verticalSpeed = 0.0f;

    // Update is called once per frame
    void Update()
    {
        // print(this.transform.position.y);
        if (!isSceneOn)
        {

            platform.gameObject.GetComponent<TrackManager>().trackSpeed = MovementSpeed;


            Vector3 pos = gameObject.transform.position;
            if (!line.Equals(targetLine))
            {
                if (targetLine == 0 && pos.x < -0.5f)
                {
                    gameObject.transform.position = new Vector3(-0.5f, pos.y, pos.z);
                    line = targetLine;
                    movec.x = 0;
                    canmove = true;
                }
                else if (targetLine == 1 && (pos.x > 0 || pos.x < 0))
                {
                    if (line == 0 && pos.x > 0)
                    {
                        gameObject.transform.position = new Vector3(0, pos.y, pos.z);
                        line = targetLine;
                        movec.x = 0;
                        canmove = true;
                    }
                    else if (line == 2 && pos.x < 0)
                    {
                        gameObject.transform.position = new Vector3(0, pos.y, pos.z);
                        line = targetLine;
                        movec.x = 0;
                        canmove = true;
                    }
                }
                else if (targetLine == 2 && pos.x > 0.5f)
                {
                    gameObject.transform.position = new Vector3(0.5f, pos.y, pos.z);
                    line = targetLine;
                    movec.x = 0;
                    canmove = true;
                }
            }
            // checkInputs();
            // if (!cc.isGrounded)
            // {
            //     movec.y = -4;
            // }


            //////JUMP Logic

            cc.Move(movec * Time.deltaTime);
        }


        // Update character position based on gravity and vertical speed
        if (!cc.isGrounded)
        {
            verticalSpeed += gravity * Time.deltaTime;
        }
        else if (!isJumping)
        {
            verticalSpeed = 0;
        }

        Vector3 velocity = new Vector3(0, verticalSpeed, 0);
        cc.Move(velocity * Time.deltaTime);
    }



    /// <summary>
    /// Movement Controls
    /// </summary>
    public void IncreaseSpeed()
    {
        if (MovementSpeed < maxSpeed)
        {

            MovementSpeed += movementIncrease;
        }
        animator.SetTrigger("Run");
        print("Speed Increased");
        MoveParticleEffect.Play();
    }
    public void DecreaseSpeed()
    {
        if (MovementSpeed > minSpeed)
        {

            MovementSpeed -= movementIncrease;
        }
        animator.SetTrigger("Walk");
        print("Speed Decreased");
        MoveParticleEffect.Play();

    }
    public void MoveRight()
    {
        if (canmove && line < 2)
        {
            targetLine++;
            canmove = false;
            movec.x = 1.5f;
            MoveParticleEffect.Play();
        }
        // GameManager.Instance.righ
    }
    public void MoveLeft()
    {
        if (canmove && line > 0)
        {
            targetLine--;
            canmove = false;
            movec.x = -1.5f;
            MoveParticleEffect.Play();
        }

    }

    public Ease LeanEase;
    [Range(0f, 0.5f)] public float duration;
    [Range(0f, 0.5f)] public float durationOut;
    private bool isJumpCoolDown = true;


    public void Tap()
    {
        if (isJumpCoolDown)
        {
            StartCoroutine(Jump());
            animator.SetTrigger("Jump");
            AudioManager.Instance?.JumpSound();
        }
    }

    IEnumerator Jump()
    {

        // Disable new jumps until this one completes
        if (cc.isGrounded)
        {
            isJumping = true;
            animator.SetTrigger("Jump");
            AudioManager.Instance?.JumpSound();

            float elapsedTime = 0;
            float initialVelocity = Mathf.Sqrt(jumpHeight * -2 * gravity);

            while (elapsedTime < jumpDuration)
            {
                verticalSpeed = initialVelocity + gravity * elapsedTime;
                elapsedTime += Time.deltaTime;
                yield return null; // Wait for the next frame
            }

            // After reaching the peak, continue with normal gravity effect
            isJumping = false;
        }

    }

    public void ResetPosition()
    {
        // Immediately stop any ongoing jump coroutine
        StopAllCoroutines();  // Stops all coroutines including the Jump coroutine

        // Reset player position to the initial position
        transform.position = new Vector3(this.transform.position.x, InitialYposition, this.transform.position.z);

        // Reset jumping state and vertical speed to ensure no ongoing physics effects
        isJumping = false;
        verticalSpeed = 0;

        // Assuming you have an Idle state, adjust as necessary
    }
public int pickupLimit=100;
    private void OnTriggerEnter(Collider other)
    {
        if (PickupCount == pickupLimit)
        {
            animator.SetTrigger("Idle");
            StopTheGame();
            Cat.transform.DORotate(new Vector3(0, -180, 0), 1);

            TrackManager.OnGameEnd_TrackManager?.Invoke();

            DOVirtual.DelayedCall(3f, () =>
            {
                GameManager.Instance.EndGame("MiniGame" + (GameManager.Instance.MiniGameNumber + 1));

            });
            ballController.ballAnimator.enabled = true;
            ballController.GameFinish();
        }
        if (other.GetComponent<Pickups>())
        {
            PickupCount++;
            mainGameUI.UpdateScore(PickupCount);
            print(PickupCount);
        }
    }

    public void INcreasepickupLimit()
    {
        pickupLimit=200000;
    }

    public void StopTheGame()
    {
        platform.gameObject.GetComponent<TrackManager>().trackSpeed = 0;
        isSceneOn = true;
    }

    public void TriggerLevel()
    {
        GameManager.Instance.InstantLoadLevel("MiniGame" + (GameManager.Instance.MiniGameNumber + 1));
    }
    public void GoToMainMenu()
    {
        //  DOTween.Clear(true);
        GameManager.Instance.TimeElapsed = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
        // GameManager.Instance.EndGame("MenuScene");
    }





}
