using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;

public class DemoStore : MonoBehaviour
{
    string dashboardIAPID = "menuitem1";
    [SerializeField] Button restoreButton;
    // Start is called before the first frame update
    // void Start()
    // {

    // }

    private void Awake()
    {
        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            restoreButton.gameObject.SetActive(false);
        }
    }

    public string environment = "production";

    public void OnPurchaseComplete(Product product)
    {
        if (product.definition.id == dashboardIAPID)
        {
            Debug.Log("Dashboard Purchased");
        }
    }
    public void OnPurchaseFailed(Product product, PurchaseFailureDescription purchaseFailureReason)
    {
        Debug.Log("Purchase failed" + product.definition.id + purchaseFailureReason);
    }

}
