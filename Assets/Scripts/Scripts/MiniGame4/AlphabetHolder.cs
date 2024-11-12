using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AlphabetHolder : MonoBehaviour
{
    // Start is called before the first frame update
    public Letter letter;
    public GameObject MovingLetter;
    public GameObject objectToSpawn;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<AlphabetSelect>().letter == letter)
        {
            print("Collided");
            Destroy(other.gameObject);
            MovingLetter.SetActive(true);
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            MiniGame4Manager.GetThePlacedLetter?.Invoke(letter, objectToSpawn);
        }
    }

}
