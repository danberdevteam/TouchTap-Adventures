using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Touch;
using UnityEngine;


public enum ButtonShape
{
    Cube, Sphere, Cone, Pyramid
}

public class ShapeButton : MonoBehaviour
{

    public ButtonShape buttonShape;
    LeanSelectableByFinger selector;
    PickupItem pickupItem;
    MiniGame3Manager game3Manager;
    [SerializeField] ParticleSystem rightFlash;
    [SerializeField] ParticleSystem wrongFlash;
    // Start is called before the first frame update
    void Start()
    {
        selector = GetComponent<LeanSelectableByFinger>();
        pickupItem = GetComponent<PickupItem>();
        game3Manager = FindObjectOfType<MiniGame3Manager>();
    }
    bool chooseOnce = true;

    // Update is called once per frame
    void Update()
    {
        if (selector.IsSelected)
        {
            if (game3Manager.TargetShape == buttonShape)
            {
                Handheld.Vibrate();
                print("Shape Selected");
                CorrectClick();
                this.enabled = false;
            }
            else
            {
                if (!wrongFlash.isPlaying)
                {
                    game3Manager.WrongShapeChosen();
                    wrongFlash.Play();
                }
            }
        }
        else
        {
            pickupItem.enabled = true;
            chooseOnce = true;

        }
    }

    void CorrectClick()
    {
        if (chooseOnce)
        {
            game3Manager.ShapeChoosen(buttonShape);
            game3Manager.OnCorrectShapePicked();
            // this.GetComponent<Renderer>().material = game3Manager.DefaultMaterial;
            pickupItem.enabled = false;
            rightFlash.Play();
            GetComponent<MeshRenderer>().enabled = false;
            chooseOnce = false;

        }
    }
}
