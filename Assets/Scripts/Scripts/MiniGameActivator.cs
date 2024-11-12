using System.Collections;
using DG.Tweening;
using UnityEngine;

public class MiniGameActivator : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject GameCanvas;
    int AnimationTime = 1;
    ParticleSystem triggerAnimation;

    GameManager gameManager;
    void Start()
    {
        triggerAnimation = GetComponentInChildren<ParticleSystem>();
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        print("Collided");
        StartCoroutine(HideTheTrigger());
    }

    IEnumerator HideTheTrigger()
    {
        this.transform.DOScale(0, AnimationTime);
        if (triggerAnimation)
        {
            triggerAnimation.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        gameManager.isMiniGameStarted = true;
        yield return new WaitForSeconds(AnimationTime - 0.4f);
        Destroy(this.gameObject);
        GameCanvas.SetActive(true);

    }


}
