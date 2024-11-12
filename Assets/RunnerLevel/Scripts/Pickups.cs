using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    [Range(0, 1)]
    public float chance = 0.5f;

    [SerializeField] private AudioClip soundEffect;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.LogWarning($"Triger entered{other.name}");
        if (other.TryGetComponent(out MoveCat cat) == false)    return;
        
        try
        {
            Destroy(transform.GetComponentInChildren<PickupItem>().gameObject);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
        GetComponentInChildren<ParticleSystem>().Play();
        
        //Play pickup sound
        if (soundEffect != null)    AudioManager.Instance?.PlaySound(soundEffect);
        
        // StartCoroutine(DelayDestroy());
    }
    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
