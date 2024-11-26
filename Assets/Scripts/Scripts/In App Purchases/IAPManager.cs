using System.Collections;
using System.Collections.Generic;
using PlayFab;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;


public class IAPManager : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
    private static Product test_product = null;

    public const string DASHBOARD = "menuitem1";
    public const string LEADERBOARD = "menuitem2";
    public static string SUB1 = "subscription1";

    private static TMP_Text myText;

    private bool return_complete = true;

    private Button dashboardButton;
    private Button leaderboardButton;
    private Button catBuyButton;
    public delegate void PurchaseSuccessCallback();
    public static event PurchaseSuccessCallback OnDashboardPurchaseSuccess;
    public static event PurchaseSuccessCallback OnLeaderboardPurchaseSuccess;
    public static event PurchaseSuccessCallback OnCatPurchaseSuccess;

    public string catIDnumber;

    void Start()
    {

        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
    }
    ConfigurationBuilder builder;
    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }



        builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(DASHBOARD, ProductType.NonConsumable);
        builder.AddProduct(LEADERBOARD, ProductType.NonConsumable);

        for (int i = 0; i < 11; i++)
        {
            builder.AddProduct("cat" + i, ProductType.NonConsumable);
        }

        // builder.AddProduct(SUB1, ProductType.Subscription);

        UnityPurchasing.Initialize(this, builder);
    }

    // public void AddToBuilder(string buildId)
    // {
    //     builder.AddProduct(buildId, ProductType.NonConsumable);
    // }


    private bool IsInitialized()
    {

        return m_StoreController != null && m_StoreExtensionProvider != null;

    }

    public void BuyDashboard(Button lockedButton)
    {
        dashboardButton = lockedButton; // Store the reference to the button
        BuyProductID(DASHBOARD);
    }

    public void BuyLeaderboard(Button lockedButton)
    {
        print("Buy leaderboard function called");
        leaderboardButton = lockedButton; // Store the reference to the button
        BuyProductID(LEADERBOARD);
    }

    public void BuyCharacter(Button lockedButton, string catIDNum)
    {
        catBuyButton = lockedButton;
        catIDnumber = catIDNum;
        BuyProductID(catIDNum);

    }

    public void UnlockButton(Button buttonToUnLock)
    {
        buttonToUnLock.GetComponentsInChildren<Image>()[1].gameObject.SetActive(false);
        buttonToUnLock.GetComponentsInChildren<TMP_Text>()[0].gameObject.SetActive(false);
        buttonToUnLock.onClick.RemoveAllListeners();
    }

    // public void BuyNoAds()
    // {
    //     BuyProductID(NO_ADS);
    // }

    public void CompletePurchase()
    {
        if (test_product == null)
            MyDebug("Cannot complete purchase, product not initialized.");
        else
        {
            m_StoreController.ConfirmPendingPurchase(test_product);
            MyDebug("Completed purchase with " + test_product.transactionID.ToString());
        }

    }

    public void ToggleComplete()
    {
        return_complete = !return_complete;
        MyDebug("Complete = " + return_complete.ToString());

    }


    bool BuyProductID(string productId)
    {
        print("Product to be bought " + productId);
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            print("product ID : " + product.transactionID);

            if (product != null && product.availableToPurchase)
            {
                MyDebug(string.Format("Purchasing product:" + product.definition.id.ToString()));
                m_StoreController.InitiatePurchase(product);
                return true;
            }
            else
            {
                MyDebug("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                return false;
            }
        }
        else
        {
            MyDebug("BuyProductID FAIL. Not initialized.");
            return false;
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        MyDebug("OnInitialized: PASS");

        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;

        CheckPurchasedItems();
    }

    private void CheckPurchasedItems()
    {
        if (m_StoreController == null) return;

        foreach (var product in m_StoreController.products.all)
        {
            if (product.hasReceipt)
            {
                Debug.Log($"Product already purchased: {product.definition.id}");
                // Save purchase status locally
                PlayerPrefs.SetInt(product.definition.id, 1); // 1 = Purchased
            }
            else
            {
                Debug.Log($"Product not purchased: {product.definition.id}");
                PlayerPrefs.SetInt(product.definition.id, 0); // 0 = Not purchased
            }
        }
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        MyDebug("OnInitializeFailed InitializationFailureReason:" + error);
    }



    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Product purchasedProduct = args.purchasedProduct;
        string purchasedProductId = purchasedProduct.definition.id;

        if (purchasedProductId == DASHBOARD)
        {
            Debug.Log($"Purchase successful: {purchasedProductId}");
            SavePurchaseToPlayFab(DASHBOARD);
            DisplayProductMetadata(purchasedProduct);
            if (dashboardButton != null)
            {
                UnlockButton(dashboardButton);
            }
            OnDashboardPurchaseSuccess?.Invoke();
        }
        else if (purchasedProductId == LEADERBOARD)
        {
            Debug.Log($"Purchase successful: {purchasedProductId}");
            SavePurchaseToPlayFab(LEADERBOARD);
            DisplayProductMetadata(purchasedProduct);
            if (leaderboardButton != null)
            {
                UnlockButton(leaderboardButton);
            }
            OnLeaderboardPurchaseSuccess?.Invoke();
        }
        else if (purchasedProductId == catIDnumber)
        {
            Debug.Log($"Purchase successful: {purchasedProductId}");
            // SavePurchaseToPlayFab(catIDnumber);
            DisplayProductMetadata(purchasedProduct);
            if (catBuyButton != null)
            {
                UnlockButton(catBuyButton);
            }
            OnCatPurchaseSuccess?.Invoke();
        }
        else
        {
            Debug.Log($"Unknown product purchased: {purchasedProductId}");
        }

        return PurchaseProcessingResult.Complete;
    }

    private void DisplayProductMetadata(Product product)
    {
        if (product != null)
        {
            Debug.Log($"Product Title: {product.metadata.localizedTitle}");
            Debug.Log($"Product Description: {product.metadata.localizedDescription}");
            Debug.Log($"Product Price: {product.metadata.localizedPriceString}");
            Debug.Log($"Currency Code: {product.metadata.isoCurrencyCode}");
            Debug.Log($"Transaction ID: {product.transactionID}");
        }
        else
        {
            Debug.Log("Product metadata not available.");
        }
    }



    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        MyDebug(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    private void MyDebug(string debug)
    {

        Debug.Log(debug);
        // myText.text += "\r\n" + debug;
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new System.NotImplementedException();
    }

    private void SavePurchaseToPlayFab(string productId)
    {
        var data = new Dictionary<string, string>
    {
        { productId, "purchased" }
    };

        PlayFabClientAPI.UpdateUserData(new PlayFab.ClientModels.UpdateUserDataRequest
        {
            Data = data
        },
        result => Debug.Log($"Successfully saved purchase state to PlayFab for {productId}."),
        error => Debug.LogError($"Failed to save purchase state to PlayFab: {error.ErrorMessage}"));
    }















}
