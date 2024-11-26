using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class CharacterSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    // [SerializeField] LeanSelectionManager
    [SerializeField] SelectChar selectorManager;
    [SerializeField] public IAPManager iAPManager;
    public List<GameObject> characters;

    public Action changeButton;

    static string CatIDPlayeprefKey = "CatSelected";

    void OnEnable()
    {
        IAPManager.OnCatPurchaseSuccess += AddlistnerCondition;
    }


    void OnDisable()
    {
        IAPManager.OnCatPurchaseSuccess -= AddlistnerCondition;
    }


    void Start()
    {
        
        changeButton += ChangeButton;
        // buyButton.onClick.AddListener(AddlistnerToButton);
    }

    void ChangeButton()
    {
        // ResetButton();
        foreach (var buton in selectorManager.Buybuttons)
        {
            buton.gameObject.SetActive(false);
        }
        selectorManager.Buybuttons[selectorManager.currentChar].gameObject.SetActive(true);

        // buyButton.onClick.AddListener(AddlistnerToButton);
    }





    void AddlistnerCondition()
    {
        selectorManager.Buybuttons[selectorManager.currentChar].onClick.AddListener(saveIDToPLayerPref);
    }

    void saveIDToPLayerPref()
    {
        PlayerPrefs.SetInt(CatIDPlayeprefKey, selectorManager.currentChar);
    }


    // Update is called once per frame

}
