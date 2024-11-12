using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] MovingUIButton moveButton;
    [SerializeField] GameManager gameManager;
    public HoppingAgent hoppingAgent;
    public float MovementSpeed=10;



    void Start()
    {

    }
    void Update()
    {
        if (!gameManager.isMiniGameStarted)
        {
            gameObject.transform.Translate(Vector3.forward * MovementSpeed * Time.deltaTime);
        }
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MiniGameActivator>())
        {
            StartMiniGame();
        }

    }

    void StartMiniGame()
    {
        // moveButton.isPressed = false;
        // moveButton.gameObject.SetActive(false);
    }
    public void EndMiniGame(int gameNumber)
    {
        // moveButton.gameObject.SetActive(true);
        // gameManager.EndGame(gameNumber);
        
    }

}
