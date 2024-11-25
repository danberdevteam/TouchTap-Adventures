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
    [SerializeField] IAPManager iAPManager;
    public Button buyButton;
    public List<GameObject> characters;

    public Action changeButton;

    


    void Start()
    {
        changeButton += ChangeButton;
    }

    void ChangeButton()
    {
        buyButton.GetComponentInChildren<TMP_Text>().text = characters[selectorManager.currentChar].GetComponent<CharacterSelector>().Name + "\n" + "$" + characters[selectorManager.currentChar].GetComponent<CharacterSelector>().Price;
    ResetButton();
    }

    void ResetButton()
    {
        buyButton.onClick.RemoveAllListeners();
    }

    // Update is called once per frame

}
