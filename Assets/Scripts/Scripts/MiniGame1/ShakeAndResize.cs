using System.Collections;
using UnityEngine;

public class ShakeAndResize : MonoBehaviour
{
    // Shaking parameters
    public float shakeDuration = 1.0f;    // Total duration of the shake


    // Resizing parameters
    public Vector3 minScale = new Vector3(0.8f, 0.8f, 0.8f); // Minimum scale
    public Vector3 maxScale = new Vector3(1.2f, 1.2f, 1.2f); // Maximum scale
    public float resizeSpeed = 2.0f;      // Speed of resizing


    private float elapsedTime;

    void Start()
    {
        // this.gameObject.SetActive(false);
    }

    void Update()
    {

        // Handle resizing (pulsating effect)
        elapsedTime += Time.deltaTime * resizeSpeed;
        float scaleLerp = Mathf.PingPong(elapsedTime, 1.0f);
        transform.localScale = Vector3.Lerp(minScale, maxScale, scaleLerp);
    }

    // Optional: If you want to manually trigger the shake
    public void TriggerShake(float duration)
    {
        shakeDuration = duration;
    }
}
