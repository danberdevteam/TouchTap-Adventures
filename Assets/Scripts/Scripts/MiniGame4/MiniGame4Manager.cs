using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MiniGame4Manager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject AlphabetPlaceholders;
    public TMP_Text AlphabetTextPlaceHolder;
    public GameObject ObjectToSpawnPlaceholder;
    GameObject previousGameobject;

    public GameObject AlphabetsHolder;
    public AlphabetGameUI gameUI;


    public static Action<Letter, GameObject> GetThePlacedLetter;
    void Start()
    {
        AlphabetTextPlaceHolder.text = "";
        // player.GetComponent<BoxCollider>().enabled = false;
        GetThePlacedLetter += ChangeTheLetter;
        // player.hoppingAgent.transform.DOScale(10, 2);
        foreach (AlphabetHolder alphabet in AlphabetPlaceholders.GetComponentsInChildren<AlphabetHolder>())
        {
            alphabet.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
      
    }


   

    void ChangeTheLetter(Letter letterPlaced, GameObject ObjectToSpawn)
    {
        print(letterPlaced);

        switch (letterPlaced)
        {
            case Letter.a:
                SaySentenceForLetter("Wow a yummy apple!!");
                break;
            case Letter.b:
                SaySentenceForLetter("Thats a big ball, would love to play on beach");
                break;
            case Letter.c:
                SaySentenceForLetter("Thats a nice Car!!");
                break;
            case Letter.d:
                SaySentenceForLetter("Duck goes Quack Quack!!");
                break;
            case Letter.e:
                SaySentenceForLetter("Elephants are the largest living land animals!");
                break;
            case Letter.f:
                SaySentenceForLetter("Fish go blub blub..");
                break;
            case Letter.g:
                SaySentenceForLetter("You sure love gifts!!");
                break;
            case Letter.h:
                SaySentenceForLetter("Hat looks good on everyone's head!");
                break;
            case Letter.i:
                SaySentenceForLetter("Who doesn't love a nice ice-cream!");
                break;
            case Letter.j:
                SaySentenceForLetter("Sure, can put lots of sweets in a jar!");
                break;
            case Letter.k:
                SaySentenceForLetter("We turn the key to open the lock.");
                break;
            case Letter.l:
                SaySentenceForLetter("Lock sure can protect your gifts from stolen..");
                break;
            case Letter.m:
                SaySentenceForLetter("Money is a tool people use to buy things");
                break;
            case Letter.n:
                SaySentenceForLetter("Where do you think the birds live?");
                break;
            case Letter.o:
                SaySentenceForLetter("An octopus has three hearts!");
                break;
            case Letter.p:
                SaySentenceForLetter("A pencil can draw lots of fun pictures!!");
                break;
            case Letter.q:
                SaySentenceForLetter("All bow down to  Queen!");
                break;
            case Letter.r:
                SaySentenceForLetter("A ring is a special circle that people wear on their fingers!");
                break;
            case Letter.s:
                SaySentenceForLetter("Thats a big ship, it sures can sail on sea!");
                break;
            case Letter.t:
                SaySentenceForLetter("Trees help us breathe by producing oxygen!");
                break;
            case Letter.u:
                SaySentenceForLetter("An umbrella keeps you dry when it rains!");
                break;
            case Letter.v:
                SaySentenceForLetter("Violin is a musical instrument that sounds like a singing voice!");
                break;
            case Letter.w:
                SaySentenceForLetter("A watch is a special tool that helps you see what time it is!");
                break;
            case Letter.x:
                SaySentenceForLetter("A xylophone is a fun musical instrument!");
                break;
            case Letter.y:
                SaySentenceForLetter("A yo-yo is a toy that goes up and down on a string!!");
                break;
            case Letter.z:
                SaySentenceForLetter("Zebra have black and white strips, looks like a big puzzle!");
                break;
        }
        if (ObjectToSpawn)
        {
            if (previousGameobject != null)
            {
                Destroy(previousGameobject);
            }
            previousGameobject = Instantiate(ObjectToSpawn, ObjectToSpawnPlaceholder.transform);
        }
        AlphabetTextPlaceHolder.text = ObjectToSpawn.name;


        // if (AlphabetsHolder.GetComponentsInChildren<AlphabetHolder>().Length == 0)
        // {

        //     StartCoroutine(gameUI.ShowText("Well done!!"));
        // }

    }

    void SaySentenceForLetter(String sentence)
    {
        // player.hoppingAgent.randomEffects[UnityEngine.Random.Range(0, player.hoppingAgent.randomEffects.Count)].Play();
        // StartCoroutine(player.hoppingAgent.ShowText(sentence, 2));
    }
}
