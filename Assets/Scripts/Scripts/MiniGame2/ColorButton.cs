using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Common;
using Lean.Touch;
using UnityEngine;

public enum ButtonColor
{
    Red, Blue, Green, None
}

public class ColorButton : MonoBehaviour
{
    // Start is called before the first frame update



    LeanSelectableByFinger selector;
    public ButtonColor buttonColor;

    MiniGame2Manager gameManager;
    ParticleSystem PressAnimation;
    ParticleSystem RightPressAnimation;
    public Material defaultMaterial;

    bool callOnce = true;
    bool correctPress = false;
    bool wrongCallOnce = true;
    void Start()
    {
        selector = GetComponentInChildren<LeanSelectableByFinger>();
        gameManager = FindObjectOfType<MiniGame2Manager>();
        PressAnimation = GetComponentsInChildren<ParticleSystem>()[0];
        // RightPressAnimation = GetComponentsInChildren<ParticleSystem>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (selector.IsSelected)
        {
            Handheld.Vibrate();
            if (buttonColor == gameManager.TargetColor)
            {
                if (callOnce)
                {
                    OnCorrectColor();
                }
                selector.GetComponent<BoxCollider>().enabled = false;
                buttonColor = ButtonColor.None;
                selector.GetComponent<Renderer>().material = defaultMaterial;
                selector.GetComponent<LeanSelectableByFinger>().enabled = false;
                PressAnimation.Play();
                this.GetComponentInChildren<ButtonTop>().gameObject.SetActive(false);
                this.enabled=false;
                // selector.transform.DOLocalMove(new Vector3(0, -0.5f, 0), 1f);
                // selector.GetComponent<Material>()
            }
            else
            {
                selector.transform.DOLocalMove(new Vector3(0, -0.23f, 0), 0.3f);
                if (wrongCallOnce)
                {
                    OnWrongColor();
                }
            }
        }
        else
        {
            if (!correctPress)
            {
                selector.transform.DOLocalMove(new Vector3(0, 0, 0), 0.3f);
            }
            wrongCallOnce = true;

        }
    }

    void OnCorrectColor()
    {
        callOnce = false;
        correctPress = true;

        gameManager.OnCorrectButtonPressed();
        // selector.transform.DOLocalMove(new Vector3(0, -0.5f, 0), 1f);


    }
    void OnWrongColor()
    {
        // callOnce=false;
        PressAnimation.Play();
        gameManager.OnWrongButtonPressed();
        wrongCallOnce = false;

    }



}
