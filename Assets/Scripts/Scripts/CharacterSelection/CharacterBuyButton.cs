using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBuyButton : MonoBehaviour
{
    public bool isBought = false;


    public List<GameObject> ThingstoRemove;

    void Start()
    {
        if (isBought)
        {
            ButtonBoughtCondition();
        }
    }


    public void ButtonBoughtCondition()
    {
        this.GetComponent<Button>().GetComponent<Image>().color = Color.blue;
        this.GetComponent<Button>().enabled = false;
    }

    public void ResetButton()
    {
        this.GetComponent<Button>().GetComponent<Image>().color = Color.white;
        this.GetComponent<Button>().enabled = true;
    }

    public void removePrice()
    {
        foreach(var image in ThingstoRemove){
            image.SetActive(false);
        }
    }



}
