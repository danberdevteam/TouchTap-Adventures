using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InteractableAnim : MonoBehaviour
{
    private Vector3 scale;
    [Range(0, 2)]
    public float scaleFactor;
    [SerializeField] private AudioClip soundEffect;
    private void Start()
    {
        scale = transform.localScale;
    }

    public void PlayEffect()
    {
        transform.DOKill();
        transform.localScale = scale;

        print("aaaaa");
        transform.DOPunchScale((scale * scaleFactor), .7f).OnComplete(() => { transform.localScale = scale; });

        //Play pickup sound
        if (soundEffect != null) AudioManager.Instance?.PlaySound(soundEffect);
        else
        {
            AudioManager.Instance?.InteractableSound();
        }
    }

    public void ChangeLevel()
    {
        GetComponent<LevelViewChanger>().ChangeLevel();
    }
}
