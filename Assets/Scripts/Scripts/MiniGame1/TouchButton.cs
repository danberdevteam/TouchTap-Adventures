using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class TouchButton : MonoBehaviour
{
    // Start is called before the first frame update

    public enum EffectType
    {
        Vibration,
        Rotation

    }


    public ParticleSystem visualAnimation;
    public LeanSelectableByFinger touch;

    // vibration variables
    public float vibrationAmount = 0.1f; // How much the sprite will vibrate
    public float vibrationSpeed = 20f;   // Speed of vibration

    private bool isVibrating = false;    // Check if vibration should happen
    private Vector3 originalPosition;    // Store the original position of the sprite
    private Coroutine selectedCoroutine; // To stop the coroutine when needed

    //rotation variables
    public float rotationSpeed = 100f;



    // Public variable to choose which effect to trigger
    public EffectType selectedEffect;
    bool isRotating = false;

    void Start()
    {
        touch = GetComponent<LeanSelectableByFinger>();
        visualAnimation = GetComponentInChildren<ParticleSystem>();
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        HandleTouchEvents();
    }


    //play/stop particle effect
    public void playAnimation()
    {
        visualAnimation.Play();
    }
    void stopanimation()
    {
        visualAnimation.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    //effect type = vibrate
    IEnumerator Vibrate()
    {
        while (isVibrating)
        {
            float xOffset = Random.Range(-1f, 1f) * vibrationAmount;
            float yOffset = Random.Range(-1f, 1f) * vibrationAmount;

            transform.localPosition = new Vector3(originalPosition.x + xOffset, originalPosition.y + yOffset, originalPosition.z);

            yield return new WaitForSeconds(1f / vibrationSpeed);
        }
    }


    // haptics for vibration
    void HapticTouch()
    {
#if UNITY_ANDROID
        Handheld.Vibrate();
#endif
    }



    //effet type = rotation
    void RotateSprite()
    {
        if (isRotating)
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime); // Rotate around the Z-axis
        }
    }


    void HandleTouchEvents()
    {
        // if object is touched and held (mouse down)
        if (touch.IsSelected)
        {
            GetComponent<ShakeAndResize>().enabled = false;
            if (!isVibrating)
            {
                isVibrating = true;
                playAnimation();
                if (selectedEffect == EffectType.Vibration)
                {
                    selectedCoroutine = StartCoroutine(Vibrate());
                }

                HapticTouch();
            }
            if (selectedEffect == EffectType.Rotation)
            {
                isRotating = true;
                RotateSprite();
            }
        }


        //if object is left (mouse up)
        else
        {
            if (isVibrating)
            {

                isVibrating = false;
                isRotating = false;
                if (selectedCoroutine != null)
                {

                    StopCoroutine(selectedCoroutine); // Stop the vibration
                }
                transform.localPosition = originalPosition;
                stopanimation();

            }
        }
    }
}
