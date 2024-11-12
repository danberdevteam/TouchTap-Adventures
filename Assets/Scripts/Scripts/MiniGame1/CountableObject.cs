using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using Lean.Touch;
using UnityEngine;

public class CountableObject : MonoBehaviour
{

    // Start is called before the first frame update
    public static event Action OnCountableObjectClickedStar;
    public static event Action OnCountableObjectClickedBomb;
    LeanSelectableByFinger touchResponse;
    float elapsedTime = 0;
    public float maximumTouchTime = 2;
    // MinigameData minigameData;


    bool callOnce = true;
    void Start()
    {
        touchResponse = GetComponent<LeanSelectableByFinger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (touchResponse.IsSelected)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > maximumTouchTime)
            {
                SizeOutTheObject();
            }
        }
        else
        {
            elapsedTime = 0;
        }
    }


    void SizeOutTheObject()
    {
        if (callOnce)
        {
            this.transform.DOScale(0, 2);
            OnCountableObjectClickedStar?.Invoke();
            StartCoroutine(DisableObject(2));
            callOnce = false;

            // print("Star touched");
        }
    }


    IEnumerator DisableObject(float timeToDestroy)
    {
        yield return new WaitForSeconds(timeToDestroy);
        // Destroy(this.gameObject);
        if (this.GetComponentInChildren<ParticleSystem>())
        {
            this.GetComponentInChildren<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        yield return new WaitForSeconds(1);
        // Destroy(this.gameObject);
        this.GetComponent<BoxCollider2D>().enabled = false;
    }

}
