using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
// using UnityEditor.SceneManagement;
using UnityEngine;

public class AnimationInventory : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] List<TouchButton> InteractableButtons;
    List<ParticleSystem> Animations;


    

    void Start()
    {
        GetAnimations();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Get animations for interactable buttons
    // [System.Obsolete]
    void GetAnimations()
    {
        foreach (var button in InteractableButtons)
        {
            if (button.GetComponentInChildren<ParticleSystem>())
            {
                // button.GetComponentInChildren<ParticleSystem>().playOnAwake=false;
                Animations.Add(button.GetComponentInChildren<ParticleSystem>());
            }
        }
    }


}
