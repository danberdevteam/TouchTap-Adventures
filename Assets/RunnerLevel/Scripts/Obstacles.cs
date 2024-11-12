using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Obstacles : MonoBehaviour
{

    [SerializeField] private AudioClip soundEffect;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MoveCat>())
        {
            MoveCat player = other.GetComponent<MoveCat>();
            player.StopTheGame();
            print("Obstacle collided");
            player.animator.SetTrigger("Die");
            player.DeathParticleEffect.Play();
            
            if (soundEffect != null)    AudioManager.Instance?.PlaySound(soundEffect);
            
            Camera.main.GetComponent<CameraShake>().shakeDuration = 0.3f;
            GameManager.Instance.EndGame("MainScene");
            this.GetComponent<BoxCollider>().enabled=false;
        }
    }




}
